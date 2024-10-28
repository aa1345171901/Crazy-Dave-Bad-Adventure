using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopDownPlate;
using UnityEngine;

//[Serializable]
//public class ShopLists
//{
//    public List<PropCard> PropCards;
//    public List<PlantCard> PlantCards;
//}

public struct PropPurchaseEvent
{
    public string propName;

    public PropPurchaseEvent(string propName)
    {
        this.propName = propName;
    }

    public static void Trigger(string propName)
    {
        EventManager.TriggerEvent<PropPurchaseEvent>(new PropPurchaseEvent(propName));
    }
}

public class ShopManager : BaseManager<ShopManager>
{
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

    public Dictionary<int, List<PlantCard>> PlantDicts { get; protected set; }

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

    protected override void Initialize()
    {
        base.Initialize();
        LoadShopItemsConf();
    }

    public void PurchaseProp(PropCard propCard, int price, Action<AttributeType, int> call = null, bool isLoad = false)
    {
        if (!isLoad)
            AchievementManager.Instance.SetAchievementType6(propCard.propName);
        PurchasedProps.Add(propCard);
        Money -= price;
        SetPropEffect(propCard);

        var userData = GameManager.Instance.UserData;
        foreach (var item in propCard.attributes)
        {
            if (item.attributeType == AttributeType.Lucky)
            {
                // 死神锁定幸运为6
                if (GetPurchaseTypeList(PropType.DeathGod).Count > 0)
                {
                    userData.Lucky = 6;
                    call?.Invoke(item.attributeType, 6);
                    continue;
                }
            }
            var fieldInfo = typeof(UserData).GetField(Enum.GetName(typeof(AttributeType), item.attributeType));
            int value = (int)fieldInfo.GetValue(userData) + item.increment;
            if (item.attributeType == AttributeType.MaximumHP && value < 0)
            {
                value = 1;
            }
            fieldInfo.SetValue(userData, value);
            call?.Invoke(item.attributeType, (int)fieldInfo.GetValue(userData));
        }

        if (propCard.propName == "vampireScepter" && userData.MaximumHP > 1)
        {
            userData.MaximumHP -= userData.MaximumHP / 4;
            call?.Invoke(AttributeType.MaximumHP, userData.MaximumHP);
        }
    }

    private void SetPropEffect(PropCard propCard)
    {
        if (propCard.propType != PropType.None)
            GameManager.Instance.SetPropDamage(propCard.propType, propCard.value1, propCard.coolingTime);
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
                    break;
                case "blackhole":
                    GameManager.Instance.HaveBlackHole = true;
                    break;
                case "Pot_Water":
                    GardenManager.Instance.NotPlacedWaterFlowerPotCount++;
                    break;
                case "key":
                    PropPurchaseEvent.Trigger(propCard.propName);
                    break;
                case "cardSlot":
                    int slotNum = GardenManager.Instance.SlotNum;
                    slotNum++;
                    if (slotNum <= ConfManager.Instance.confMgr.gameIntParam.GetItemByKey("maxSlot").value)
                    {
                        GardenManager.Instance.SlotNum = slotNum;
                    }
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
            case PlantType.TwinSunflower:
            case PlantType.Spikerock:
            case PlantType.CobCannon:
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

    private void LoadShopItemsConf()
    {
        //TextAsset ta = Resources.Load<TextAsset>("Shopping");
        //shopLists = JsonUtility.FromJson<ShopLists>(ta.text);

        PropDicts = new Dictionary<int, List<PropCard>>();
        var propCards = ConfManager.Instance.confMgr.data.propCards.PropCards;
        foreach (var item in propCards)
        {
            if (!PropDicts.ContainsKey(item.quality))
            {
                PropDicts[item.quality] = new List<PropCard>();
            }

            // 成就判断是否解锁
            var achievementItem = ConfManager.Instance.confMgr.data.achievement.GetPropItemByName(item.propName);
            if (achievementItem == null || AchievementManager.Instance.GetReach(achievementItem.achievementId))
            {
#if UNITY_ANDROID
            if (item.propName != "magnetic" && item.propName != "blackhole")
#endif
                PropDicts[item.quality].Add(item);
            }                                                                                 
        }

        PlantLists = new List<PlantCard>();
        PlantDicts = new Dictionary<int, List<PlantCard>>();
        PlantEvolutionDict = new Dictionary<PlantType, PlantCard>();
        var plantCards = ConfManager.Instance.confMgr.data.plantCards.PlantCards;
        foreach (var item in plantCards)
        {
            switch (item.plantType)
            {
                case PlantType.Repeater:
                case PlantType.Cattail:
                case PlantType.GatlingPea:
                case PlantType.GloomShroom:
                case PlantType.TwinSunflower:
                case PlantType.Spikerock:
                case PlantType.CobCannon:
                    // 成就判断是否解锁
                    var achievementItem = ConfManager.Instance.confMgr.data.achievement.GetPlantItemByPlantType((int)item.plantType);
                    if (achievementItem == null || AchievementManager.Instance.GetReach(achievementItem.achievementId))
                    {
                        PlantEvolutionDict.Add(item.plantType, item);
                    }
                    break;
                default:
                    if (item.plantType == PlantType.DoomShroom)
                    {
                        // 成就判断是否解锁
                        var achievementItem2 = ConfManager.Instance.confMgr.data.achievement.GetPlantItemByPlantType((int)item.plantType);
                        if (achievementItem2 == null || AchievementManager.Instance.GetReach(achievementItem2.achievementId))
                        {
                            PlantLists.Add(item);
                            if (!PlantDicts.ContainsKey(item.quality))
                                PlantDicts[item.quality] = new List<PlantCard>();
                            PlantDicts[item.quality].Add(item);
                        }
                    }
                    else
                    {
                        PlantLists.Add(item);
                        if (!PlantDicts.ContainsKey(item.quality))
                            PlantDicts[item.quality] = new List<PlantCard>();
                        PlantDicts[item.quality].Add(item);
                    }
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

    public List<PropCard> GetPurchaseTypeList(PropType propType)
    {
        var list = new List<PropCard>();
        foreach (var item in PurchasedProps)
        {
            if (item.propType == propType)
                list.Add(item);
        }
        return list;
    }

    public void RemovePurchasePropByName(string name)
    {
        PropCard propCard = null;
        foreach (var item in PurchasedProps)
        {
            if (item.propName == name)
            {
                propCard = item;
                break;
            }
        }
        if (propCard != null)
        {
            PurchasedProps.Remove(propCard);
        }
    }

    public void SellProp(PropCard propCard, bool isSellAll)
    {
        if (propCard == null)
            return;
        var list = new List<PropCard>();
        foreach (var item in PurchasedProps)
        {
            if (item.propName == propCard.propName)
            {
                list.Add(item);
            }
        }
        int nowPrice = Mathf.RoundToInt(propCard.GetNowPrice() * ConfManager.Instance.confMgr.gameIntParam.GetItemByKey("sellRate").value / 100f);
        var userData = GameManager.Instance.UserData;
        void SellItem(PropCard propCard)
        {
            PurchasedProps.Remove(propCard);
            Money += nowPrice;
            foreach (var item in propCard.attributes)
            {
                var fieldInfo = typeof(UserData).GetField(Enum.GetName(typeof(AttributeType), item.attributeType));
                int value = (int)fieldInfo.GetValue(userData) - item.increment;
                if (item.attributeType == AttributeType.MaximumHP && value < 0)
                {
                    value = 1;
                }
                fieldInfo.SetValue(userData, value);
                GameManager.Instance.attributePanel.SetAttribute(item.attributeType, (int)fieldInfo.GetValue(userData));
            }
        }
        if (isSellAll)
        {
            foreach (var item in list)
            {
                SellItem(item);
            }
        }
        else
        {
            SellItem(list.First());
        }
        GameManager.Instance.RemoveProp(propCard, isSellAll, isSellAll ? list.Count : 1);
    }

    public void UpdateCardPool()
    {
        // 判断要加的卡
        List<PropCard> tempAddPropDicts = new List<PropCard>();
        // 判断要去掉的卡
        HashSet<PropCard> tempRemovePropDicts = new HashSet<PropCard>();

        // 判断前置卡片，有前置卡片才加入可刷出字典
        foreach (var item in ConfManager.Instance.confMgr.propCards.frontLimit)
        {
            var frontCount = PurchasePropCount(item.Key.frontProp);
            if (frontCount > 0)
            {
                tempAddPropDicts.Add(item.Value);
            }
            else
            {
                tempRemovePropDicts.Add(item.Value);
            }
        }

        // 判断有最大限制的道具，达到最大数量时去掉
        foreach (var item in ConfManager.Instance.confMgr.propCards.maxNumLimit)
        {
            var count = PurchasePropCount(item.Value.propName);
            if (count < item.Key.maxNum)
            {
                tempAddPropDicts.Add(item.Value);
            }
            else
            {
                tempRemovePropDicts.Add(item.Value);
            }
        }
        // 先加需要加的有条件限制的卡
        foreach (var item in tempAddPropDicts)
        {
            if (!tempRemovePropDicts.Contains(item) && PropDicts.ContainsKey(item.quality) && !PropDicts[item.quality].Contains(item))
            {
                PropDicts[item.quality].Add(item);
            }
        }
        // 去掉条件不满足的
        foreach (var item in tempRemovePropDicts)
        {
            if (PropDicts.ContainsKey(item.quality) && PropDicts[item.quality].Contains(item))
            {
                PropDicts[item.quality].Remove(item);
            }
        }
        tempAddPropDicts.Clear();
        tempRemovePropDicts.Clear();

        // 先去掉所有后置植物卡
        foreach (var item in PlantEvolutionDict)
        {
            if (PlantLists.Contains(item.Value))
            {
                PlantLists.Remove(item.Value);
                PlantDicts[item.Value.quality].Remove(item.Value);
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
                    case PlantType.SunFlower:
                    case PlantType.Spikeweed:
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
                    PlantListsAddEvolution(PlantType.CobCannon, item.Value);
                    break;
                case PlantType.FumeShroom:
                    PlantListsAddEvolution(PlantType.GloomShroom, item.Value);
                    break;
                case PlantType.Lilypad:
                    PlantListsAddEvolution(PlantType.Cattail, item.Value);
                    break;
                case PlantType.SunFlower:
                    PlantListsAddEvolution(PlantType.TwinSunflower, item.Value);
                    break;
                case PlantType.Spikeweed:
                    PlantListsAddEvolution(PlantType.Spikerock, item.Value);
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
        if (value > count && PlantEvolutionDict.ContainsKey(plantType))
        {
            PlantLists.Add(PlantEvolutionDict[plantType]);
            PlantDicts[PlantEvolutionDict[plantType].quality].Remove(PlantEvolutionDict[plantType]);
        }
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
