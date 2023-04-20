using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PolevaulterMove : AIMove
{
    [Tooltip("在近战范围外的速度")]
    public float walkSpeed = 1f;
    public PolevaulterAttack polevaulterAttack;

    public override void ProcessAbility()
    {
        base.ProcessAbility();
        if (!polevaulterAttack.isAttacking)
        {
            if (AIParameter.Distance < polevaulterAttack.AttackRange)
            {
                if (polevaulterAttack.trackEntry == null)
                    SetRealSpeed();
            }
            else
            {
                MoveSpeed = walkSpeed;
            }
        }
        if (target != null)
        {
            polevaulterAttack.direction = target.transform.position - polevaulterAttack.PolePos.transform.position;
        }
        else
        {
            polevaulterAttack.direction = Target.transform.position - polevaulterAttack.PolePos.transform.position;
        }
    }
}
