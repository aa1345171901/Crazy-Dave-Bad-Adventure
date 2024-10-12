using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class ExternalGardenPanel : BasePanel
{
    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        var animator = GetComponent<Animator>();
        animator.Play("show");
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
        SaveManager.Instance.SaveExternalGrowData();
    }
}
