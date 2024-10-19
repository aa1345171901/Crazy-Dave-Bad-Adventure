using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElf : BaseElf
{
    string animStr;

    public int level { get; set; }

    public override void Reuse()
    {
        base.Reuse();
        level = 4;
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
