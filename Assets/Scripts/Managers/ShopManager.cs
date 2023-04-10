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
    /// �̵���Ʒ����
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
    /// ��Ʒ��ϸ�ֵ���Ʒ
    /// </summary>
    public Dictionary<int, List<PropCard>> PropDicts { get; protected set; }

    /// <summary>
    /// �ų�����Ҫǰ��ֲ���ֲ�￨Ƭ������ˢ�µ��Ŀ�
    /// </summary>
    public List<PlantCard> PlantLists { get; protected set; }

    /// <summary>
    /// ��Ҫ�����Ŀ�Ƭ
    /// </summary>
    public Dictionary<PlantType, PlantCard> PlantEvolutionDict { get; set; }

    /// <summary>
    /// ����Ľ��������������˽������Ͳ�����ˢ������û�ط��ɹ�������������
    /// </summary>
    public Dictionary<PlantType, int> PurchasedPlantEvolutionDicts = new Dictionary<PlantType, int>();

    /// <summary>
    /// �Ѿ�����ĵ���
    /// </summary>
    public List<PropCard> PurchasedProps { get; set; } = new List<PropCard>();

    private List<PropCard> VocalConcert = new List<PropCard>();  // �ݳ��ῨƬ���

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
        
        // ����еĿ�����ȡ������ϳɹ�ǰ����ˢ��
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
                    // �����ǽ����Եģ����Խ�������������ˢ��
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
    /// ����ֲ�￨
    /// </summary>
    /// <param name="plantCard">����Ŀ�</param>
    /// <param name="price">�۸�</param>
    /// <returns>�����Ƿ������������ֲͨ�ﻨ�����˾����ˣ��ǽ���ֲ��������</returns>
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
                // Ĭ�ϼ���δ��ֲ���У�����false�жϻ���
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
        // ��ϳɹ��󣬽����и���Ͽ�����
        if (GameManager.Instance.IsOpenVocalConcert && VocalConcert.Count > 0)
        {
            PropDicts[VocalConcert[0].quality].AddRange(VocalConcert);
            VocalConcert.Clear();
        }

        // ��ȥ�����к��ÿ�
        foreach (var item in PlantEvolutionDict)
        {
            if (PlantLists.Contains(item.Value))
            {
                PlantLists.Remove(item.Value);
            }
        }
        // �������ֲ����Ӻ��ÿ�����
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
        // �ж������빺������������Ƿ���ӽ�����
        foreach (var item in plantCount)
        {
            switch (item.Key)
            {
                case PlantType.Peashooter:
                    // �жϹ����˫�����ָ������Լ��㶹���ֵ������ɹ�����
                    PlantListsAddEvolution(PlantType.Repeater, item.Value);
                    break;
                case PlantType.Repeater:
                    PlantListsAddEvolution(PlantType.GatlingPea, item.Value);
                    break;
                case PlantType.Cornpult:
                    // todo ��ũ��
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
        // ���ȡ4����ͬ�������
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
