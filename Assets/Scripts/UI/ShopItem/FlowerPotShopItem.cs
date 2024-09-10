using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPotShopItem : ShopItem
{
    private readonly string info = GameTool.LocalText("flowerpot_info1");
    public readonly string infoNoPurchase = GameTool.LocalText("flowerpot_info2");

    private void Awake()
    {
        SetDefualtInfo();
    }

    protected override void OnClick()
    {
        if (GardenManager.Instance.AllFlowerPotCount < GardenManager.Instance.MaxFlowerPotCount)
        {
            this.Info = info;
            isDown = false;
            GardenManager.Instance.NotPlacedFlowerPotCount++;
            this.gameObject.SetActive(false);
        }
        else
        {
            this.Info = infoNoPurchase;
            CannotAfford?.Invoke();
        }
    }

    public void SetDefualtInfo()
    {
        this.Info = info;
    }

    public void SetActive()
    {
        this.gameObject.SetActive(true);
    }
}
