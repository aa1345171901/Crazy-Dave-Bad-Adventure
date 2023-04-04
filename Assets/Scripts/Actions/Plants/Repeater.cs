using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : PeaShooter
{
    public override PlantType PlantType => PlantType.Repeater;

    protected override void Attack(string trigger)
    {
        base.Attack(trigger);
        Invoke("CreatePeaBullet", 0.15f);
    }
}
