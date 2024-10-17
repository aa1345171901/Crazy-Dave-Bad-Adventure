using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class StarBullet : PeaBullet
{
    public override BulletType BulletType => BulletType.Star;
    public override DamageType damageType => DamageType.StarBullet;

    public Vector3 StarfruitPos;
    public bool isRight;

    protected override void Init()
    {
        base.Init();

        if (isRight)
            direction = transform.position - StarfruitPos;
        else
        {
            direction = transform.position - StarfruitPos;
            direction.x = -direction.x;
        }
    }
}
