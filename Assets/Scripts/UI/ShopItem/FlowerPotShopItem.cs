using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPotShopItem : ShopItem
{
    private readonly string info = "这是在进货时捡到的花盆\n可以在花园中<color=#00ff00>培养植物</color>";
    public readonly string infoNoPurchase = "花园中已经不能够放下更多的花盆了\n或许可以用<color=#ff0000>铲子</color>铲掉泥巴";

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
