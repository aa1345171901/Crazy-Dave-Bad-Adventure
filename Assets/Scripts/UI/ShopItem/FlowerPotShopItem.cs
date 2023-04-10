using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPotShopItem : ShopItem
{
    private readonly string info = "�����ڽ���ʱ�񵽵Ļ���\n�����ڻ�԰��<color=#00ff00>����ֲ��</color>";
    public readonly string infoNoPurchase = "��԰���Ѿ����ܹ����¸���Ļ�����\n���������<color=#ff0000>����</color>�������";

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
