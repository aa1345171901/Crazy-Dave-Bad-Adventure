using System;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[Serializable]
public class PlantUIPrefabInfo
{
    public PlantType plantType;
    public GameObject plantPrefab;
}

public class GardenManager : BaseManager<GardenManager>
{
    [Tooltip("植物苗预制体")]
    public GameObject SeedingPrefab;
    [Tooltip("花盆中植物类型对应的植物Prefab")]
    public List<PlantUIPrefabInfo> PlantUIPrefabInfos;

    private int sun = 10000;
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

    public void AddPlant(PlantCard plantCard)
    {
        NoPlantingPlants.Add(plantCard);
    }

    public PlantUIPrefabInfo GetPlantUIPrefabInfo(PlantType plantType)
    {
        PlantUIPrefabInfo plantUIPrefabInfo = null;
        foreach (var item in PlantUIPrefabInfos)
        {
            if (item.plantType == plantType)
            {
                plantUIPrefabInfo = item;
                break;
            }
        }
        return plantUIPrefabInfo;
    }
}
