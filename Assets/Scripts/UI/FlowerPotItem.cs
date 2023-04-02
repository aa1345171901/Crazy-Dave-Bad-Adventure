using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPotItem : ShopItem
{
    private readonly string info = "�����ڽ���ʱ�񵽵Ļ���\n�����ڻ�԰��<color=#00ff00>����ֲ��</color>";

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
