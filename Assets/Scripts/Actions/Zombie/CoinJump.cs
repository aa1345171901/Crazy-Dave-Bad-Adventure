using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

/// <summary>
/// 钱币和阳光从僵尸身体跳出时的动画参数
/// </summary>
public class ItemJump
{
    public MoneyClick item;
    // 钱币掉落动画参数
    public float height;  // 跳跃高度
    public float time;  // 跳跃持续时间
    public Vector3 offsetSpeed;  // 跳跃偏移速度

    public ItemJump(MoneyClick item)
    {
        this.item = item;
    }
}

public class CoinJump : MonoBehaviour
{
    [Header("预制体")]
    public GameObject SliverCoin;
    public GameObject GoldCoin;
    public GameObject Diamond;
    public GameObject Sun;

    private float curTime;

    private GameObject ExplosionGO;  // 可能会爆的钱币
    private List<ItemJump> targets = new List<ItemJump>();  // 包含钱币和阳光

    public void DeadExplosionRate()
    {
        /*
        * 是否掉落银币，金币，钻石，与幸运挂钩，  
        * 掉落概率为  银币 （15 + 幸运）%  , 
        * 金币（幸运 / 6）%， 
        * 钻石（幸运 / 30）%
        */
        // 幸运*30增加精度
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
            // 掉落在僵尸周围范围
            Vector3 offset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            itemJump.height = Random.Range(0.3f, 0.6f);
            itemJump.time = Random.Range(0.4f, 0.6f);
            itemJump.offsetSpeed = offset / itemJump.time;
            targets.Add(itemJump);
        }
        curTime = 0;

        /* 阳光掉落，更加幸运决定
         * 掉落阳光数量1-6个不等，根据幸运决定数量， 幸运小于10掉1，到60最大
         */

        int sunCount = lucky / 10 + 1;
        sunCount = sunCount > 6 ? 6 : sunCount;
        for (int i = 0; i < sunCount; i++)
        {
            var targetSun = GameObject.Instantiate(Sun).GetComponent<MoneyClick>();
            targetSun.transform.position = this.transform.position;
            ItemJump itemJump = new ItemJump(targetSun);
            // 掉落在僵尸周围范围
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
