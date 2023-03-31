using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProp : MonoBehaviour
{
    public int DefaultDamage { get; set; }
    public float DefaultAttackCoolingTime { get; set; }

    public virtual void Reuse()
    {

    }

    public virtual void ProcessAbility()
    {

    }

    public virtual void DayEnd()
    {

    }
}
