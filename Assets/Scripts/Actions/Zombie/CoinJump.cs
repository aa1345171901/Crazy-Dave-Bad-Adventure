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

    PlantSeed plantSpeed;

    private float curTime;

    [Range(0, 2)]
    public int Price; // 额外掉落 ==1 为金币 == 2为钻石

    private GameObject ExplosionGO;  // 可能会爆的钱币
    private List<ItemJump> targets = new List<ItemJump>();  // 包含钱币和阳光

    public void DeadExplosionRate(DamageType damageType)
    {
        // 大嘴花\墓碑吃掉不扣
        if (damageType == DamageType.Chomper || damageType == DamageType.Gravebuster)
            return;
        /*
        * 是否掉落银币，金币，钻石，与幸运挂钩，  
        * 掉落概率为  银币 （25 + 幸运）%  , 
        * 金币（幸运 / 6）%， 
        * 钻石（幸运 / 30）%
        */
        // 幸运*30增加精度
        targets.Clear();
        int lucky = GameManager.Instance.UserData.Lucky;
        int random = Random.Range(0, 3001);
        ExplosionGO = null;
        if (random < 30 * ConfManager.Instance.confMgr.moneyParam.GetWeightByKey("diamond") * lucky / 30)
            ExplosionGO = Diamond;
        else if (random < 30 * ConfManager.Instance.confMgr.moneyParam.GetWeightByKey("goldCoin") * lucky / 30)
            ExplosionGO = GoldCoin;
        else if (random < (lucky + 25) * 30)
            ExplosionGO = SliverCoin;

        if (ExplosionGO != null)
        {
            CreateCoin(ExplosionGO);
        }
        if (Price == 1)
        {
            CreateCoin(GoldCoin);
        }
        if (Price == 2)
        {
            CreateCoin(Diamond);
        }

        CreateSun();
        CreateSeed();
        curTime = 0;
    }

    private void CreateCoin(GameObject gameObject)
    {
        var targetCoin = GameObject.Instantiate(gameObject).GetComponent<MoneyClick>();
        targetCoin.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, targetCoin.transform.position.z);
        ItemJump itemJump = new ItemJump(targetCoin);
        // 掉落在僵尸周围范围
        Vector3 offset = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), targetCoin.transform.position.z);
        itemJump.height = Random.Range(0.3f, 0.6f);
        itemJump.time = Random.Range(0.4f, 0.6f);
        itemJump.offsetSpeed = offset / itemJump.time;
        targets.Add(itemJump);
    }

    private void CreateSun()
    {
        /* 阳光掉落，更加幸运决定
 * 掉落阳光数量1-6个不等，根据幸运决定数量， 幸运小于10掉1，到60最大
 */
        int lucky = GameManager.Instance.UserData.Lucky;
        int sunCount = lucky / 10 + 1;
        sunCount = sunCount > 6 ? 6 : sunCount;
        sunCount = Random.Range(1, sunCount + 1);
        for (int i = 0; i < sunCount; i++)
        {
            var targetSun = GameObject.Instantiate(Sun).GetComponent<MoneyClick>();
            targetSun.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, targetSun.transform.position.z);
            ItemJump itemJump = new ItemJump(targetSun);
            // 掉落在僵尸周围范围
            Vector3 offset = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), targetSun.transform.position.z);
            itemJump.height = Random.Range(0.3f, 0.6f);
            itemJump.time = Random.Range(0.4f, 0.6f);
            itemJump.offsetSpeed = offset / itemJump.time;
            targets.Add(itemJump);
        }
    }

    private void CreateSeed()
    {
        var confSeedWeightItem = ConfManager.Instance.confMgr.seedWeight.GetPlantSeedRandom();

        if (confSeedWeightItem.plantType != 0)
        {
            plantSpeed = Resources.Load<PlantSeed>("Prefabs/Effects/plantSeed");
            var newPlantSpeed = GameObject.Instantiate(plantSpeed);
            newPlantSpeed.plantType = confSeedWeightItem.plantType;
            newPlantSpeed.quality = confSeedWeightItem.quality;
            newPlantSpeed.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, newPlantSpeed.transform.position.z);
            ItemJump itemJump = new ItemJump(newPlantSpeed);
            // 掉落在僵尸周围范围
            Vector3 offset = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), newPlantSpeed.transform.position.z);
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
                        newPos.z = item.offsetSpeed.z;
                        item.item.transform.position = newPos;
                    }
                }
            }
        }
        if (curTime > 0.6f)
            targets.Clear();
    }
}
