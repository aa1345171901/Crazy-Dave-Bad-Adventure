using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class FireElf : BaseElf
{
    string animStr;

    public int level { get; set; }

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
        finalDamage = DefaultDamage;
    }

    public override IEnumerator Attack(Collider2D[] colliders)
    {
        animator.Play(animStr + "Attack", 0, 0);
        yield return new WaitForSeconds(0.33f);
        // todo 释放攻击
        yield return new WaitForSeconds(0.33f);
        animator.Play(animStr + "Idel", 0, 0);
        yield return base.Attack(colliders);
    }
}
