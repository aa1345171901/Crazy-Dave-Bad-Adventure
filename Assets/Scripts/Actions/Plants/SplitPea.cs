using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitPea : PeaShooter
{
    public override PlantType PlantType => PlantType.SplitPea;

    [Tooltip("反向子弹发射位置")]
    public Transform BulletPos2;

    public override void Reuse()
    {
        base.Reuse();

        pos = this.transform.position;
        size = new Vector2(finalRage * 2, 1);
    }

    protected override PeaBullet CreatePeaBullet()
    {
        CreatePeaBullet2();
        Invoke("CreatePeaBullet2", 0.15f);
        return base.CreatePeaBullet();
    }

    private void CreatePeaBullet2()
    {
        var peaBullet = GameObject.Instantiate(PeaBullet, BulletPos2);
        peaBullet.Damage = finalDamage;
        peaBullet.SplashPercentage = splashPercentage;
        peaBullet.Speed *= -bulletSpeedMul;
    }
}
