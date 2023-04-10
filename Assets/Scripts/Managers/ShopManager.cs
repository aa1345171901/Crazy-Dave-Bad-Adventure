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
    /// 排除掉需要前置植物的植物卡片，可以刷新到的卡
    /// </summary>
    public List<PlantCard> PlantLists { get; protected set; }

    /// <summary>
    /// 需要进化的卡片
    /// </summary>
    public Dictionary<PlantType, PlantCard> PlantEvolutionDict { get; set; }

    /// <summary>
    /// 购买的进化卡数量，买够了进化卡就不能再刷，否则没地方可供进化，不符合
    /// </summary>
    public Dictionary<PlantType, int> PurchasedPlantEvolutionDicts = new Dictionary<PlantType, int>();

    /// <summary>
    /// 已经购买的道具
    /// </summary>
    public List<PropCard> PurchasedProps { get; set; } = new List<PropCard>();

    private List<PropCard> VocalConcert = new List<PropCard>();  // 演唱会卡片组合

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
        
        // 组合中的卡，获取到在组合成功前不再刷新
        if (propCard.propDamageType == PropDamageType.VocalConcert && !GameManager.Instance.IsOpenVocalConcert)
        {
            PropDicts[propCard.quality].Remove(propCard);
            VocalConcert.Add(propCard);
        }
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
                case "magnetic":
                    GameManager.Instance.HaveMagnetic = true;
                    // 由于是降属性的，所以解锁完能力后不再刷新
                    PropDicts[propCard.quality].Remove(propCard);
                    break;
                case "blackhole":
                    GameManager.Instance.HaveBlackHole = true;
                    PropDicts[propCard.quality].Remove(propCard);
                    break;
                case "Pot_Water":
                    GardenManager.Instance.NotPlacedWaterFlowerPotCount++;
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 购买植物卡
    /// </summary>
    /// <param name="plantCard">购买的卡</param>
    /// <param name="price">价格</param>
    /// <returns>返回是否能买，如果是普通植物花盆满了就买不了，是进化植物则能买</returns>
    public bool PurchasePlant(PlantCard plantCard, int price, bool isNormal = false)
    {
        bool result = false;
        switch (plantCard.plantType)
        {
            case PlantType.Repeater:
            case PlantType.Cattail:
            case PlantType.GatlingPea:
            case PlantType.GloomShroom:
            case PlantType.GoldMagent:
                DicExistJudge(PurchasedPlantEvolutionDicts, plantCard.plantType);
                Money -= price;
                result = true;
                break;
            default:
                // 默认加入未种植序列，返回false判断花盆
                if (isNormal)
                {
                    Money -= price;
                    GardenManager.Instance.AddPlant(plantCard);
                }
                break;
        }
        return result;
    }

    private void DicExistJudge(Dictionary<PlantType, int> dicts, PlantType plantType)
    {
        if (dicts.ContainsKey(plantType))
        {
            dicts[plantType]++;
        }
        else
        {
            dicts.Add(plantType, 1);
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

        PlantLists = new List<PlantCard>();
        PlantEvolutionDict = new Dictionary<PlantType, PlantCard>();
        var plantCards = shopLists.PlantCards;
        foreach (var item in plantCards)
        {
            PlantLists.Add(item);
            switch (item.plantType)
            {
                case PlantType.Repeater:
                case PlantType.Cattail:
                case PlantType.GatlingPea:
                case PlantType.GloomShroom:
                case PlantType.GoldMagent:
                    PlantEvolutionDict.Add(item.plantType, item);
                    break;
                default:
                    break;
            }
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

    public void UpdateCardPool()
    {
        // 组合成功后，将所有该组合卡加入
        if (GameManager.Instance.IsOpenVocalConcert && VocalConcert.Count > 0)
        {
            PropDicts[VocalConcert[0].quality].AddRange(VocalConcert);
            VocalConcert.Clear();
        }

        // 先去掉所有后置卡
        foreach (var item in PlantEvolutionDict)
        {
            if (PlantLists.Contains(item.Value))
            {
                PlantLists.Remove(item.Value);
            }
        }
        // 检测现有植物添加后置卡数量
        var plants = GardenManager.Instance.PlantAttributes;
        Dictionary<PlantType, int> plantCount = new Dictionary<PlantType, int>();
        foreach (var item in plants)
        {
            if (item.isCultivate)
            {
                switch (item.plantCard.plantType)
                {
                    case PlantType.Peashooter:
                    case PlantType.Repeater:
                    case PlantType.Cornpult:
                    case PlantType.FumeShroom:
                    case PlantType.Lilypad:
                        DicExistJudge(plantCount, item.plantCard.plantType);
                        break;
                    default:
                        break;
                }
            }
        }
        // 判断数量与购买的数量觉定是否添加进卡池
        foreach (var item in plantCount)
        {
            switch (item.Key)
            {
                case PlantType.Peashooter:
                    // 判断购买的双发射手个数，以及豌豆射手的培育成功个数
                    PlantListsAddEvolution(PlantType.Repeater, item.Value);
                    break;
                case PlantType.Repeater:
                    PlantListsAddEvolution(PlantType.GatlingPea, item.Value);
                    break;
                case PlantType.Cornpult:
                    // todo 加农炮
                    // PlantListsAddEvolution(PlantType.GloomShroom, item.Value);
                    break;
                case PlantType.FumeShroom:
                    PlantListsAddEvolution(PlantType.GloomShroom, item.Value);
                    break;
                case PlantType.Lilypad:
                    PlantListsAddEvolution(PlantType.Cattail, item.Value);
                    break;
                default:
                    break;
            }
        }
    }

    private void PlantListsAddEvolution(PlantType plantType, int value)
    {
        int count = 0;
        if (PurchasedPlantEvolutionDicts.ContainsKey(plantType))
            count = PurchasedPlantEvolutionDicts[plantType];
        if (value > count)
            PlantLists.Add(PlantEvolutionDict[plantType]);
    }
}

public static class RandomUtils
{
    public static HashSet<int> RandomCreateNumber(int len, int count)
    {
        // 随机取4个不同的随机数
        HashSet<int> hashSet = new HashSet<int>();
        int RmNum = count;
        for (; hashSet.Count < RmNum;)
        {
            int randomIndex = UnityEngine.Random.Range(0, len);
            if (!hashSet.Contains(randomIndex))
            {
                hashSet.Add(randomIndex);
            }
        }
        return hashSet;
    }
}
