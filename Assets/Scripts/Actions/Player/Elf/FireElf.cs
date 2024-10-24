using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class FireElf : BaseElf
{
    string animStr;

    int level;

    private int finalDamage;
    private int finalCount;

    public override void Reuse()
    {
        base.Reuse();
        level = ShopManager.Instance.GetPurchaseTypeList(PropType.FireElf).Count;
        level = level > 4 ? 4 : level;
        switch (level)
        {
            case 1:
                animStr = "FireStar";
                break;
            case 2:
                animStr = "FireSeed";
                break;
            case 3:
                animStr = "FireBall";
                break;
            case 4:
                animStr = "FireElf";
                break;
        }
        animator.Play(animStr + "Idel", 0, 0);

        finalCount = 1;
        finalDamage = DefaultDamage;

        var confItem = ConfManager.Instance.confMgr.propCards.GetItemByTypeLevel((int)PropType.FireElf, level);
        if (confItem != null)
        {
            var userData = GameManager.Instance.UserData;
            if (confItem.propName == "fireElf")
            {
                finalDamage = Mathf.RoundToInt((userData.CriticalHitRate * 2 + confItem.value1) * (100f + userData.CriticalDamage) / 100) * 2;
                finalCount = 1 + Mathf.RoundToInt(userData.CriticalDamage / 50);
            }
            else
            {
                finalDamage = Mathf.RoundToInt((userData.CriticalHitRate + confItem.value1) * (100f + userData.CriticalDamage) / 100);
            }
            DefaultDamage = confItem.value1;
            DefaultAttackCoolingTime = confItem.coolingTime;
        }
    }

    public override IEnumerator Attack(Collider2D[] colliders)
    {
        animator.Play(animStr + "Attack", 0, 0);
        yield return new WaitForSeconds(0.33f);
        // 释放攻击
        for (int i = 0; i < finalCount; i++)
        {
            var prefab = Resources.Load<FireElfBullet>("Prefabs/Props/FireElf_Bullet");
            var fireElf = GameObject.Instantiate(prefab);
            fireElf.level = level;
            fireElf.damage = finalDamage;
            fireElf.range = level == 4 ? 1.5f : 1f;
            if (level == 4)
                fireElf.transform.localScale = Vector3.one * 1.5f;
            if (i < colliders.Length)
                fireElf.transform.position = colliders[i].transform.position;
            else
                fireElf.transform.position = colliders[0].transform.position + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2));
        }        

        yield return new WaitForSeconds(0.33f);
        animator.Play(animStr + "Idel", 0, 0);
        yield return base.Attack(colliders);
    }
}
