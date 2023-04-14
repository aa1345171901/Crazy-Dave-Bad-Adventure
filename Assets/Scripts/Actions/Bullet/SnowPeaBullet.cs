using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class SnowPeaBullet : PeaBullet
{
    public float DecelerationPercentage = 0.2f;
    public float DecelerationTime = 3f;

    public GameObject particleSystem1;

    protected override void DoDamage(Health health, int damage)
    {
        base.DoDamage(health, damage);
        particleSystem1.gameObject.SetActive(false);
        var aiMove = health.GetComponent<AIMove>();
        if (aiMove)
        {
            aiMove.BeDecelerated(DecelerationPercentage, DecelerationTime);
        }
    }
}
