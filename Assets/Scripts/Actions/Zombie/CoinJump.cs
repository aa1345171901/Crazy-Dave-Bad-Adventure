using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class CoinJump : MonoBehaviour
{
    [Header("预制体")]
    public GameObject SliverCoin;
    public GameObject GoldCoin;
    public GameObject Diamond;

    // 钱币掉落动画参数
    private float height;
    private float time;
    private Vector3 oriPos;
    private Vector3 offsetSpeed;
    private float curTime;

    private GameObject ExplosionGO;  // 可能会爆的
    private MoneyClick targetCoin;

    public void DeadExplosionRate()
    {
        /*
        * 是否掉落银币，金币，钻石，与幸运挂钩，  
        * 掉落概率为  银币 （15 + 幸运）%  , 
        * 金币（幸运 / 6）%， 
        * 钻石（幸运 / 30）%
        */
        // 幸运*30增加精度
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
            targetCoin = GameObject.Instantiate(ExplosionGO).GetComponent<MoneyClick>();
            targetCoin.transform.position = this.transform.position;

            // 掉落在僵尸周围范围
            Vector3 offset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            height = Random.Range(0.3f, 0.6f);
            time = Random.Range(0.4f, 0.6f);
            offsetSpeed = offset / time;
            oriPos = transform.position;
            curTime = 0;
        }
    }

    void Update()
    {
        if (targetCoin != null && !targetCoin.IsExit)
        {
            if (curTime < time)
            {
                curTime += Time.deltaTime;
                Vector3 newPos = oriPos + offsetSpeed * curTime;
                newPos.y += Mathf.Cos(curTime / time * Mathf.PI - Mathf.PI / 2) * height;
                targetCoin.transform.position = newPos;
            }
        }
    }
}
