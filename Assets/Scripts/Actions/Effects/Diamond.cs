using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MoneyClick
{
    public float BlinkingIntervalTime; // …¡À∏º‰∏Ù ±º‰

    private Animator animator;

    private float timer;

    private void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    protected override void PlayAnimation()
    {
        base.PlayAnimation();
        if (Time.time - timer > BlinkingIntervalTime)
        {
            animator.SetTrigger("Flash");
            timer = Time.time;
        }
    }
}
