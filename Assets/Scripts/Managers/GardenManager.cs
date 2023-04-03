using System;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[Serializable]
public class PlantInfo
{
    public PlantType plantType;
}

[Serializable]
public class PlantUIPrefabInfo : PlantInfo
{
    public GameObject plantPrefab;
}

[Serializable]
public class PlantPrefabInfo : PlantInfo
{
    public Plant plant;
}

public static class PlantInfoExpand
{
    public static T GetPlantInfo<T>(this List<T> list, PlantType plantType) where T : PlantInfo
    {
        T plantPrefabInfo = null;
        foreach (var item in list)
        {
            if (item.plantType == plantType)
            {
                plantPrefabInfo = item;
                break;
            }
        }
        return plantPrefabInfo;
    }
}

public class GardenManager : BaseManager<GardenManager>
{
    [Tooltip("植物苗预制体")]
    public GameObject SeedingPrefab;
    [Tooltip("花盆中植物类型对应的植物Prefab")]
    public List<PlantUIPrefabInfo> PlantUIPrefabInfos;
    [Tooltip("场景中的植物合集")]
    public List<PlantPrefabInfo> PlantPrefabInfos;

    private int sun;
    public int Sun
    {
        get
        {
            return sun;
        }
        set
        {
            sun = value;
            SunChanged?.Invoke();
        }
    }
    public Action SunChanged;

    /// <summary>
    /// 本次购物购买的没有放置的花盆数量
    /// </summary>
    public int NotPlacedFlowerPotCount { get; set; }

    public int FlowerPotCount { get; set; }

    /// <summary>
    /// 最大花盆摆放数量
    /// </summary>
    public int MaxFlowerPotCount { get; set; } = 16;

    /// <summary>
    /// 本次购物购买的植物
    /// </summary>
    public List<PlantCard> NoPlantingPlants { get; set; } = new List<PlantCard>();

    /// <summary>
    /// 种植的植物的属性
    /// </summary>
    public List<PlantAttribute> PlantAttributes { get; set; } = new List<PlantAttribute>();

    /// <summary>
    /// 战斗的植物集合
    /// </summary>
    public Dictionary<PlantAttribute, Plant> PlantDict { get; set; } = new Dictionary<PlantAttribute, Plant>();

    public void AddPlant(PlantCard plantCard)
    {
        NoPlantingPlants.Add(plantCard);
    }

    public void PlantsGoToWar()
    {
        if (PlantDict.Count != PlantAttributes.Count)
        {
            foreach (var item in PlantAttributes)
            {
                if (!PlantDict.ContainsKey(item) && item.isCultivate)
                {
                    var plant = GameObject.Instantiate(PlantPrefabInfos.GetPlantInfo(item.plantCard.plantType).plant);
                    plant.plantAttribute = item;
                    PlantDict.Add(item, plant);
                }
            }
        }

        foreach (var item in PlantDict)
        {
            item.Value.Reuse();
        }
    }
}
