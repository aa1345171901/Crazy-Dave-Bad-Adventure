using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MoneyClick
{
    public float RotationAngle;

    private float angle;

    protected override void PlayAnimation()
    {
        base.PlayAnimation();
        angle += RotationAngle;
        this.transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    protected override void OnClick()
    {
        base.OnClick();
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
