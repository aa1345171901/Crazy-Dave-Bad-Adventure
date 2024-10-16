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

public static class PropsExpand
{
    public static bool Contains<T>(this List<BaseProp> list) where T : BaseProp
    {
        bool contains = false;
        foreach (var item in list)
        {
            if (item is T)
            {
                contains = true;
                break;
            }
        }
        return contains;
    }

    public static T GetValue<T>(this List<BaseProp> list) where T : BaseProp
    {
        T baseProp = null;
        foreach (var item in list)
        {
            if (item is T target)
            {
                baseProp = target;
                break;
            }
        }
        return baseProp;
    }
}
