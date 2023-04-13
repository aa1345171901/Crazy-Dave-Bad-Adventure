using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Marigold : Plant
{
    public override PlantType PlantType => PlantType.Marigold;

    [Tooltip("������ȴʱ��")]
    public float CoolTime = 12f;
    public Coin Sliver;
    public Coin Gold;
    public Coin Diamond;

    public AudioSource audioSource;

    private float timer;

    private float finalCoolTime;
    private float finalGoldCoinRate;
    private float finalTwinRate;
    private float finalDiamond;

    private List<ItemJump> itemJumps = new List<ItemJump>();  // Ǯ��������������

    private readonly float LevelRate = 0.03f;
    private readonly float LevelDiamondRate = 0.003f;
    private readonly float LevelTime = 0.4f;

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
        finalGoldCoinRate = 0;
        finalTwinRate = 0;
        finalDiamond = 0;
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
                // 3 �����Ҹ���
                case 3:
                    finalGoldCoinRate += (int)fieldInfo.GetValue(plantAttribute) * LevelRate;
                    break;
                // 4 ����˫������
                case 4:
                    finalTwinRate += (int)fieldInfo.GetValue(plantAttribute) * LevelRate;
                    break;
                // 5 ������ʯ����
                case 5:
                    finalDiamond += (int)fieldInfo.GetValue(plantAttribute) * LevelDiamondRate;
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            itemJumps.Clear();
            timer = Time.time;
            animator.SetTrigger("Attack");
            Invoke("DelayCreate", 0.5f);
        }
        foreach (var item in itemJumps)
        {
            if (Time.time - timer < item.time)
            {
                Vector3 newPos = this.transform.position + item.offsetSpeed * (Time.time - timer);
                newPos.y += Mathf.Cos((Time.time - timer) / item.time * Mathf.PI - Mathf.PI / 2) * item.height;
                item.item.transform.position = newPos;
            }
        }
    }

    private void DelayCreate()
    {
        audioSource.Play();
        CreateCoin();
        if (finalTwinRate > 0)
        {
            if (Random.Range(0, 1f) < finalTwinRate)
                CreateCoin();
        }
    }

    private void CreateCoin()
    {
        Coin coin = Sliver;
        if (finalGoldCoinRate > 0)
        {
            coin = Random.Range(0, 1f) < finalGoldCoinRate ? Gold : coin;
        }
        if (finalDiamond > 0)
        {
            coin = Random.Range(0, 1f) < finalDiamond ? Diamond : coin;
        }

        var targetCoin = GameObject.Instantiate(coin);
        targetCoin.transform.position = this.transform.position;
        var itemJump = new ItemJump(targetCoin);
        Vector3 offset = new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-1, 1), targetCoin.transform.rotation.z);
        itemJump.height = Random.Range(0.3f, 0.6f);
        itemJump.time = Random.Range(0.4f, 0.6f);
        itemJump.offsetSpeed = offset / itemJump.time;
        itemJumps.Add(itemJump);
        timer = Time.time;
    }
}