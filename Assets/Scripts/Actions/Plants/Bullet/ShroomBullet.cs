using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomBullet : PeaBullet
{
    public override BulletType BulletType => BulletType.Shroom;

    public float BulletSize;

    protected override void Init()
    {
        base.Init();
        this.transform.localScale = new Vector3(BulletSize, BulletSize, BulletSize);
        SplashSizeX *= BulletSize;
        SplashSizeY *= BulletSize;
        bulletParticleSystem.transform.localScale = new Vector3(BulletSize, BulletSize, BulletSize);
    }
}
