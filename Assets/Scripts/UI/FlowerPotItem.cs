using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPotItem : ShopItem
{
    private readonly string info = "这是在进货时捡到的花盆\n可以在花园中<color=#00ff00>培养植物</color>";

    private void Awake()
    {
        this.Info = info;
    }

    protected override void OnClick()
    {
        base.OnClick();
        ShopManager.Instance.FlowerPotCount++;
        this.gameObject.SetActive(false);
    }

    public void SetActive()
    {
        this.gameObject.SetActive(true);
    }
}
