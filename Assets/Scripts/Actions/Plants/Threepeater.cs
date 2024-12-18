using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threepeater : PeaShooter
{
    public override PlantType PlantType => PlantType.Threepeater;

    [Tooltip("子弹发射位置")]
    public List<Transform> BulletPoses;

    public override void Reuse(bool randomPos = true)
    {
        base.Reuse(randomPos);

        pos = this.transform.position;
        size = new Vector2(finalRage * 2, 3);
    }

    protected override PeaBullet CreatePeaBullet()
    {
        foreach (var item in BulletPoses)
        {
            var peaBullet = GameObject.Instantiate(PeaBullet, item);
            peaBullet.Damage = finalDamage;
            peaBullet.SplashPercentage = splashPercentage;
            peaBullet.Speed *= bulletSpeedMul;
        }
        return null;
    }
}
