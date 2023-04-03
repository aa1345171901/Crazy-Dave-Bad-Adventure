using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : PeaShooter
{
    protected override void Attack(string trigger)
    {
        base.Attack(trigger);
        Invoke("CreatePeaBullet", 0.15f);
    }
}
