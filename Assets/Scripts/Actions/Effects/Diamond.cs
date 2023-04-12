using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Diamond : Coin
{
    public float BlinkingIntervalTime; // …¡À∏º‰∏Ù ±º‰

    private Animator animator;

    private float timer;

    private void Start()
    {
        this.animator = GetComponent<Animator>();
        GameManager.Instance.Coins.Add(this);
    }

    protected override void PlayAnimation()
    {
        if (Time.time - timer > BlinkingIntervalTime)
        {
            animator.SetTrigger("Flash");
            timer = Time.time;
        }
    }
}
