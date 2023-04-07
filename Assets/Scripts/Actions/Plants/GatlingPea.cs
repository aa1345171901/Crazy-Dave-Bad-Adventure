using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingPea : PeaShooter
{
    public override PlantType PlantType => PlantType.GatlingPea;

    protected override void Attack(string trigger)
    {
        base.Attack(trigger);
        Invoke("CreatePeaBullet", 0.1f);
        Invoke("CreatePeaBullet", 0.15f);
        Invoke("CreatePeaBullet", 0.2f);
    }
}
