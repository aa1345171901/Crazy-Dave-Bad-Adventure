using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Coin : MoneyClick
{
    public float RotationAngle;

    private float angle;

    private void Start()
    {
        if (!isDigest)
            GameManager.Instance.Coins.Add(this);
        Destroy(gameObject, AvailableTime);
    }

    protected override void PlayAnimation()
    {
        base.PlayAnimation();
        angle += RotationAngle;
        this.transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public override void OnClick()
    {
        if (GameManager.Instance.Coins.Contains(this) || isDigest)
        {
            base.OnClick();
            GameManager.Instance.Coins.Remove(this);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.Coins.Remove(this);
    }
}
