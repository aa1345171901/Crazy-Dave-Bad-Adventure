using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class MagnetShroom : Plant
{
    public override PlantType PlantType => PlantType.MagentShroom;

    [Tooltip("��ȡ����ƷĬ�ϸ���")]
    public int CutterCount = 1;
    [Tooltip("������ȴʱ��")]
    public float CoolTime = 12f;
    [Tooltip("���ճ���ʱ��")]
    public float DurationTime = 2f;
    public AudioSource audioSource;

    private float timer;
    private bool isAbsorbing;  // ������ȡ

    private int finalCount;
    private float finalCoolTime;
    private float finalDurationTime;
    private int finalChangeCoin;

    private readonly float LevelCutterCount = 0.34f;
    private readonly float LevelTime = 0.4f;
    private readonly int LevelCoin = 10;

    public Dictionary<Vector3, GameObject> targets = new Dictionary<Vector3, GameObject>();

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    public override void Reuse()
    {
        base.Reuse();

        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalCoolTime = CoolTime;
        finalCount = CutterCount;
        finalDurationTime = DurationTime;
        finalChangeCoin = 0;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 2 ��ȴʱ��
                case 2:
                    finalCoolTime -= (int)fieldInfo.GetValue(plantAttribute) * LevelTime;
                    break;
                // 3 ��ȡ�����
                case 3:
                    finalChangeCoin += (int)fieldInfo.GetValue(plantAttribute) * LevelCoin;
                    break;
                // 4 ��ȡ����
                case 4:
                    int level = (int)fieldInfo.GetValue(plantAttribute);
                    finalCount += (int)(level * LevelCutterCount);
                    if (level == 10)
                        finalCount = 5;
                    break;
                // 5 ����ʱ��
                case 5:
                    finalDurationTime += (int)fieldInfo.GetValue(plantAttribute) * LevelTime / 2;
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        if (!isAbsorbing && Time.time - timer > finalCoolTime)
        {
            animator.SetBool("IsCoolling", false);
            if (targets.Count > 0)
            {
                ShopManager.Instance.Money += finalChangeCoin * targets.Count;
                foreach (var item in targets)
                {
                    Destroy(item.Value);
                }
                targets.Clear();
            }
            if (JudgeAttack())
            {
                audioSource.Play();
                timer = Time.time;
                animator.SetBool("IsAttack", true);
                StartAbsorbing();
            }
        }
        if (isAbsorbing && Time.time - timer > finalDurationTime)
        {
            timer = Time.time;
            isAbsorbing = false;
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsCoolling", true);
        }
        if (isAbsorbing)
        {
            // ��������Ʒ
            if (targets.Count < finalCount)
            {
                JudgeAttack();
            }

            foreach (var item in targets)
            {
                float process = (Time.time - timer) / finalDurationTime;
                var lerp = Vector3.Lerp(item.Key, transform.position, process);
                item.Value.transform.position = new Vector3(lerp.x, lerp.y, 0);
            }
        }
    }

    private bool JudgeAttack()
    {
        bool result = false;
        List<Character> zombies = new List<Character>();
        zombies.AddRange(LevelManager.Instance.Enemys.Get(ZombieType.Bucket).Zombies);
        zombies.AddRange(LevelManager.Instance.Enemys.Get(ZombieType.Screendoor).Zombies);
        zombies.AddRange(LevelManager.Instance.Enemys.Get(ZombieType.Football).Zombies);
        foreach (var item in zombies)
        {
            ZombieProp zombieProp = item.GetComponentInChildren<ZombieProp>();
            if (zombieProp != null && !zombieProp.IsFall)
            {
                targets.Add(zombieProp.transform.position, zombieProp.MagnetShroomAttack());
                result = true;
                if (targets.Count >= finalCount)
                    break;
            }
        }
        return result;
    }

    private void StartAbsorbing()
    {
        isAbsorbing = true;
        timer = Time.time;
    }
}
