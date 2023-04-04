using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCardPage : MonoBehaviour
{
    public Animator animator;
    private bool isExpand;

    private void OnMouseEnter()
    {
        if (!isExpand)
        {
            animator.SetTrigger("Enter");
        }
    }

    private void OnMouseExit()
    {
        if (isExpand)
        {
            animator.SetTrigger("Exit");
        }
    }

    private void SetExpandTrue()
    {
        this.isExpand = true;
    }

    private void SetExpandFalse()
    {
        this.isExpand = false;
    }
}
