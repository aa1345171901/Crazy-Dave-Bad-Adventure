using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[Serializable]
public class ShopLists
{
    public List<PropCard> PropCards;
    public List<PlantCard> PlantCards;
}

public class ShopManager : BaseManager<ShopManager>
{
    /// <summary>
    /// 商店物品集合
    /// </summary>
    public ShopLists shopLists;

    private int money;
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            MoneyChanged?.Invoke();
        }
    }
    public Action MoneyChanged;

    /// <summary>
    /// 按品质细分的商品
    /// </summary>
    public Dictionary<int, List<PropCard>> PropDicts { get; protected set; }

    /// <summary>
    /// 已经购买的道具
    /// </summary>
    public List<PropCard> PurchasedProps { get; set; } = new List<PropCard>();

    protected override void Initialize()
    {
        base.Initialize();
        ParseShopItemsJson();
    }

    public void PurchaseProp(PropCard propCard, int price)
    {
        PurchasedProps.Add(propCard);
        Money -= price;
        SetPropEffect(propCard);
    }

    private void SetPropEffect(PropCard propCard)
    {
        if (propCard.propDamageType != PropDamageType.None)
            GameManager.Instance.SetPropDamage(propCard.propDamageType, propCard.defalutDamage, propCard.coolingTime);
        else
        {
            switch (propCard.propName)
            {
                case "PortalCard":
                    GameManager.Instance.SetTransferGate();
                    break;
                case "Spinacia":
                    GameManager.Instance.IsZombieShock = true;
                    break;
                default:
                    break;
            }
        }
    }

    private void ParseShopItemsJson()
    {
        TextAsset ta = Resources.Load<TextAsset>("Shopping");
        shopLists = JsonUtility.FromJson<ShopLists>(ta.text);

        PropDicts = new Dictionary<int, List<PropCard>>();
        var propCards = shopLists.PropCards;
        foreach (var item in propCards)
        {
            if (!PropDicts.ContainsKey(item.quality))
            {
                PropDicts[item.quality] = new List<PropCard>();
            }
            PropDicts[item.quality].Add(item);                                                                                  
        }
    }

    public int PurchasePropCount(string name)
    {
        int count = 0;
        foreach (var item in PurchasedProps)
        {
            if (item.propName == name)
                count++;
        }
        return count;
    }
}
