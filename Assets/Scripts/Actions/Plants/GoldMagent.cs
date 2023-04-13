using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class GoldMagent : Plant
{
    public override PlantType PlantType => PlantType.GoldMagent;

    [Tooltip("��ȡ���Ĭ�ϸ���")]
    public int CoinGoldCount = 4;
    [Tooltip("������ȴʱ��")]
    public float CoolTime = 4f;
    [Tooltip("���ճ���ʱ��")]
    public float DurationTime = 2f;

    public AudioSource audioSource;

    private float timer;
    private Dictionary<Vector3, Coin> coins = new Dictionary<Vector3, Coin>();
    private bool isAbsorbing;

    private int finalCount;
    private float finalCoolTime;
    private float finalDurationTime;

    private readonly int LevelCoinCount = 1;
    private readonly float LevelTime = 0.2f;

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
        finalCount = CoinGoldCount;
        finalDurationTime = DurationTime;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 3 Ϊ��ȡ����
                case 3:
                    finalCount += (int)fieldInfo.GetValue(plantAttribute) * LevelCoinCount;
                    break;
                // 4 ��ȴʱ��
                case 4:
                    finalCoolTime -= (int)fieldInfo.GetValue(plantAttribute) * LevelTime;
                    break;
                // 5 ����ʱ��
                case 5:
                    finalDurationTime += (int)fieldInfo.GetValue(plantAttribute) * LevelTime;
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
            if (GameManager.Instance.Coins.Count > 0)
            {
                timer = Time.time;
                audioSource.Play();
                animator.SetBool("IsAttack", true);
                Invoke("StartAbsorbing", 0.5f);
            }
        }
        if (isAbsorbing && Time.time - timer > finalDurationTime)
        {
            timer = Time.time;
            isAbsorbing = false;
            animator.SetBool("IsAttack", false);
            foreach (var item in coins)
            {
                item.Value.Collect();
            }
            coins.Clear();
        }
        if (isAbsorbing)
        {
            if (coins.Count < finalCount)
            {
                var allCoin = GameManager.Instance.Coins;
                if (allCoin.Count > 0)
                {
                    int index = Random.Range(0, allCoin.Count);
                    var coin = allCoin[index];
                    allCoin.Remove(coin);
                    coins.Add(coin.transform.position, coin);
                }             
            }

            foreach (var item in coins)
            {
                float process = (Time.time - timer) / finalDurationTime;
                var lerp = Vector3.Lerp(item.Key, transform.position, process);
                item.Value.transform.position = new Vector3(lerp.x, lerp.y, 0);
            }
        }
    }

    private void StartAbsorbing()
    {
        isAbsorbing = true;
        timer = Time.time;
    }
}
