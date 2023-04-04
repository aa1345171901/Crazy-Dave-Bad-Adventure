using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

/// <summary>
/// Ǯ�Һ�����ӽ�ʬ��������ʱ�Ķ�������
/// </summary>
public class ItemJump
{
    public MoneyClick item;
    // Ǯ�ҵ��䶯������
    public float height;  // ��Ծ�߶�
    public float time;  // ��Ծ����ʱ��
    public Vector3 offsetSpeed;  // ��Ծƫ���ٶ�

    public ItemJump(MoneyClick item)
    {
        this.item = item;
    }
}

public class CoinJump : MonoBehaviour
{
    [Header("Ԥ����")]
    public GameObject SliverCoin;
    public GameObject GoldCoin;
    public GameObject Diamond;
    public GameObject Sun;

    private float curTime;

    private GameObject ExplosionGO;  // ���ܻᱬ��Ǯ��
    private List<ItemJump> targets = new List<ItemJump>();  // ����Ǯ�Һ�����

    public void DeadExplosionRate()
    {
        /*
        * �Ƿ�������ң���ң���ʯ�������˹ҹ���  
        * �������Ϊ  ���� ��15 + ���ˣ�%  , 
        * ��ң����� / 6��%�� 
        * ��ʯ������ / 30��%
        */
        // ����*30���Ӿ���
        targets.Clear();
        int lucky = GameManager.Instance.UserData.Lucky;
        int random = Random.Range(0, 3001);
        ExplosionGO = null;
        if (random < 30 * lucky / 30)
            ExplosionGO = Diamond;
        else if (random < 30 * lucky / 6)
            ExplosionGO = GoldCoin;
        else if (random < (lucky + 15) * 30)
            ExplosionGO = SliverCoin;

        if (ExplosionGO != null)
        {
            var targetCoin = GameObject.Instantiate(ExplosionGO).GetComponent<MoneyClick>();
            targetCoin.transform.position = this.transform.position;
            ItemJump itemJump = new ItemJump(targetCoin);
            // �����ڽ�ʬ��Χ��Χ
            Vector3 offset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            itemJump.height = Random.Range(0.3f, 0.6f);
            itemJump.time = Random.Range(0.4f, 0.6f);
            itemJump.offsetSpeed = offset / itemJump.time;
            targets.Add(itemJump);
        }
        curTime = 0;

        /* ������䣬�������˾���
         * ������������1-6�����ȣ��������˾��������� ����С��10��1����60���
         */

        int sunCount = lucky / 10 + 1;
        sunCount = sunCount > 6 ? 6 : sunCount;
        for (int i = 0; i < sunCount; i++)
        {
            var targetSun = GameObject.Instantiate(Sun).GetComponent<MoneyClick>();
            targetSun.transform.position = this.transform.position;
            ItemJump itemJump = new ItemJump(targetSun);
            // �����ڽ�ʬ��Χ��Χ
            Vector3 offset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            itemJump.height = Random.Range(0.3f, 0.6f);
            itemJump.time = Random.Range(0.4f, 0.6f);
            itemJump.offsetSpeed = offset / itemJump.time;
            targets.Add(itemJump);
        }
    }

    void Update()
    {
        if (targets.Count > 0)
        {
            foreach (var item in targets)
            {
                if (!item.item.IsExit)
                {
                    if (curTime < item.time)
                    {
                        curTime += Time.deltaTime;
                        Vector3 newPos = this.transform.position + item.offsetSpeed * curTime;
                        newPos.y += Mathf.Cos(curTime / item.time * Mathf.PI - Mathf.PI / 2) * item.height;
                        item.item.transform.position = newPos;
                    }
                }
            }
        }
    }
}
