using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElf : BaseElf
{
    public ParticleSystem effect;
    public ParticleSystem effect2;

    string animStr;

    public int level { get; set; }

    public override void Reuse()
    {
        base.Reuse();
        level = 4;
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
        // todo 释放攻击
        yield return new WaitForSeconds(0.33f);
        animator.Play(animStr + "Idel", 0, 0);
        yield return base.Attack(colliders);
    }
}
