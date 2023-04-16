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

public class BloverEffect
{
    public void Init()
    {
        Windage = Windspeed = BloverResume = 0;
    }

    /// <summary>
    /// 三叶草的提供的逆风时对僵尸的风阻，可叠加，超过1的部分需要/10
    /// </summary>
    public float Windage { get; set; }
    /// <summary>
    /// 三叶草的提供的顺风时对玩家的风速，可叠加，超过1的部分需要/5
    /// </summary>
    public float Windspeed { get; set; }
    /// <summary>
    /// 三叶草的提供的逆风时对玩家的生命恢复
    /// </summary>
    public int BloverResume { get; set; }
}

/// <summary>
/// 火炬对豌豆的增益
/// </summary>
public class TorchwoodEffect
{
    public void Init()
    {
        DamageAdd = SplashDamage = PeaSpeed = 1;
    }

    public float DamageAdd { get; set; }

    public float SplashDamage { get; set; }

    public float PeaSpeed { get; set; }
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

    public int NotPlacedWaterFlowerPotCount { get; set; }

    public int WaterFlowerPotCount { get; set; }

    /// <summary>
    /// 现在已经有的花盆数
    /// </summary>
    public int AllFlowerPotCount => NotPlacedFlowerPotCount + FlowerPotCount + NotPlacedWaterFlowerPotCount + WaterFlowerPotCount;

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

    /// <summary>
    /// 墓碑吞噬者
    /// </summary>
    public List<Gravebuster> Gravebusters { get; set; } = new List<Gravebuster>();

    /// <summary>
    /// 高坚果
    /// </summary>
    public List<TallNut> TallNuts { get; set; } = new List<TallNut>();

    /// <summary>
    /// 卡槽的植物
    /// </summary>
    public List<PlantAttribute> CardslotPlant { get; set; } = new List<PlantAttribute>();

    /// <summary>
    ///  最大插槽
    /// </summary>
    public int MaxSlot { get; set; } = 2;

    /// <summary>
    /// 是否读取了存档，在PlantContent时进行花盆和植物的载入
    /// </summary>
    public bool IsLoadPlantData { get; set; }

    /// <summary>
    ///  三叶草提供的增益
    /// </summary>
    public BloverEffect BloverEffect { get; private set; } = new BloverEffect();

    /// <summary>
    /// 火炬树桩
    /// </summary>
    public TorchwoodEffect TorchwoodEffect { get; private set; } = new TorchwoodEffect();

    /// <summary>
    /// 墓碑提供的增伤
    /// </summary>
    public float GravebusterDamage { get; set; } = 1;

    public void AddPlant(PlantCard plantCard)
    {
        NoPlantingPlants.Add(plantCard);
    }

    public void PlantsGoToWar()
    {
        BloverEffect.Init();
        TorchwoodEffect.Init();
        GravebusterDamage = 1;
        TallNuts.Clear();
        foreach (var item in PlantAttributes)
        {
            if (!item.isManual && !PlantDict.ContainsKey(item) && item.isCultivate)
            {
                var plantPrefab = PlantPrefabInfos.GetPlantInfo(item.plantCard.plantType)?.plant;
                if (plantPrefab != null)
                {
                    var plant = GameObject.Instantiate(plantPrefab);
                    plant.plantAttribute = item;
                    if (item.plantCard.plantType == PlantType.Gravebuster)
                    {
                        Gravebusters.Add(plant as Gravebuster);
                    }
                    PlantDict.Add(item, plant);
                }
            }
        }

        // 不能在遍历时修改值，所以新建一个字典存储
        Dictionary<PlantAttribute, Plant> destroyPlants = new Dictionary<PlantAttribute, Plant>();
        foreach (var item in PlantDict)
        {
            if (item.Key.plantCard.plantType != item.Value.PlantType)
            {
                var plant = GameObject.Instantiate(PlantPrefabInfos.GetPlantInfo(item.Key.plantCard.plantType).plant);
                plant.plantAttribute = item.Key;
                destroyPlants.Add(item.Key, plant);
                GameObject.Destroy(item.Value.gameObject);
            }
            item.Value.Reuse();
        }

        foreach (var item in destroyPlants)
        {
            PlantDict[item.Key] = item.Value;
            item.Value.Reuse();
        }
        destroyPlants.Clear();
    }

    public int GetNoPlantingPlantsLilypadCount()
    {
        int count = 0;
        foreach (var item in NoPlantingPlants)
        {
            if (item.plantType == PlantType.Lilypad)
                count++;
        }
        return count;
    }

    public int GetPlantsCount(PlantType plantType)
    {
        int count = 0;
        foreach (var item in PlantAttributes)
        {
            if (item.plantCard.plantType == plantType)
                count++;
        }
        return count;
    }
}
