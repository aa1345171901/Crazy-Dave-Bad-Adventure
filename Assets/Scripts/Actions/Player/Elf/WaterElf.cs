using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class WaterElf : BaseElf
{
    public ParticleSystem effect;
    public ParticleSystem effect2;

    string animStr;

    int level;

    private int finalDamage;
    private int finalCount;
    private float finalRepulsiveForce = 1;
    private float finalDecelerationPercentage = 0;

    public override void Reuse()
    {
        base.Reuse();
        level = ShopManager.Instance.GetPurchaseTypeList(PropType.WaterElf).Count;
        level = level > 4 ? 4 : level;
        switch (level)
        {
            case 1:
                animStr = "WaterDroplet";
                break;
            case 2:
                animStr = "WaterBead";
                break;
            case 3:
                animStr = "WaterSplash";
                break;
            case 4:
                animStr = "WaterElf";
                break;
        }
        animator.Play(animStr + "Idel", 0, 0);
        effect2.gameObject.SetActive(level == 4);

        finalCount = 1;
        finalDamage = DefaultDamage;

        var confItem = ConfManager.Instance.confMgr.propCards.GetItemByTypeLevel((int)PropType.WaterElf, level);
        if (confItem != null)
        {
            var userData = GameManager.Instance.UserData;
            if (confItem.propName == "waterElf")
            {
                finalDamage = Mathf.RoundToInt((userData.MaximumHP / 3 + userData.LifeRecovery / 2.5f + confItem.value1));
                finalCount = 1 + Mathf.RoundToInt(userData.MaximumHP / 100);
                finalDecelerationPercentage = userData.LifeRecovery / 150f;
                finalRepulsiveForce = userData.MaximumHP / 20;
            }
            else
            {
                finalDamage = Mathf.RoundToInt((userData.MaximumHP / 6 + userData.LifeRecovery / 5f + confItem.value1));
                finalDecelerationPercentage = userData.LifeRecovery / 200f;
                finalRepulsiveForce = userData.MaximumHP / 30;
            }

            if (finalDecelerationPercentage > 0.7f)
                finalDecelerationPercentage = 0.7f;
            DefaultDamage = confItem.value1;
            DefaultAttackCoolingTime = confItem.coolingTime;
        }
    }

    public override void Update2()
    {
        base.Update2();
        effect.GetComponent<ParticleSystemRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;
        effect2.GetComponent<ParticleSystemRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    public override IEnumerator Attack(Collider2D[] colliders)
    {
        animator.Play(animStr + "Attack", 0, 0);
        yield return new WaitForSeconds(0.33f);
        // 释放攻击
        for (int i = 0; i < finalCount; i++)
        {
            var prefab = Resources.Load<WaterElfBullet>("Prefabs/Props/WaterElf_Bullet");
            var waterElfBullet = GameObject.Instantiate(prefab);
            waterElfBullet.level = level;
            waterElfBullet.damage = finalDamage;
            waterElfBullet.finalDecelerationPercentage = finalDecelerationPercentage;
            waterElfBullet.finalRepulsiveForce = finalRepulsiveForce;
            if (level == 4)
                waterElfBullet.transform.localScale = Vector3.one * 1.5f;
            if (i < colliders.Length)
                waterElfBullet.targetPos = colliders[i].transform.position;
            else
                waterElfBullet.targetPos = colliders[0].transform.position + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2));
            waterElfBullet.transform.position = transform.position;
        }

        yield return new WaitForSeconds(0.33f);
        animator.Play(animStr + "Idel", 0, 0);
        yield return base.Attack(colliders);
    }
}
