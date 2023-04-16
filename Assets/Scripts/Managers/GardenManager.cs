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
    /// ��Ҷ�ݵ��ṩ�����ʱ�Խ�ʬ�ķ��裬�ɵ��ӣ�����1�Ĳ�����Ҫ/10
    /// </summary>
    public float Windage { get; set; }
    /// <summary>
    /// ��Ҷ�ݵ��ṩ��˳��ʱ����ҵķ��٣��ɵ��ӣ�����1�Ĳ�����Ҫ/5
    /// </summary>
    public float Windspeed { get; set; }
    /// <summary>
    /// ��Ҷ�ݵ��ṩ�����ʱ����ҵ������ָ�
    /// </summary>
    public int BloverResume { get; set; }
}

/// <summary>
/// �����㶹������
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
    [Tooltip("ֲ����Ԥ����")]
    public GameObject SeedingPrefab;
    [Tooltip("������ֲ�����Ͷ�Ӧ��ֲ��Prefab")]
    public List<PlantUIPrefabInfo> PlantUIPrefabInfos;
    [Tooltip("�����е�ֲ��ϼ�")]
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
    /// ���ι��ﹺ���û�з��õĻ�������
    /// </summary>
    public int NotPlacedFlowerPotCount { get; set; }

    public int FlowerPotCount { get; set; }

    public int NotPlacedWaterFlowerPotCount { get; set; }

    public int WaterFlowerPotCount { get; set; }

    /// <summary>
    /// �����Ѿ��еĻ�����
    /// </summary>
    public int AllFlowerPotCount => NotPlacedFlowerPotCount + FlowerPotCount + NotPlacedWaterFlowerPotCount + WaterFlowerPotCount;

    /// <summary>
    /// �����ڷ�����
    /// </summary>
    public int MaxFlowerPotCount { get; set; } = 16;

    /// <summary>
    /// ���ι��ﹺ���ֲ��
    /// </summary>
    public List<PlantCard> NoPlantingPlants { get; set; } = new List<PlantCard>();

    /// <summary>
    /// ��ֲ��ֲ�������
    /// </summary>
    public List<PlantAttribute> PlantAttributes { get; set; } = new List<PlantAttribute>();

    /// <summary>
    /// ս����ֲ�Ｏ��
    /// </summary>
    public Dictionary<PlantAttribute, Plant> PlantDict { get; set; } = new Dictionary<PlantAttribute, Plant>();

    /// <summary>
    /// Ĺ��������
    /// </summary>
    public List<Gravebuster> Gravebusters { get; set; } = new List<Gravebuster>();

    /// <summary>
    /// �߼��
    /// </summary>
    public List<TallNut> TallNuts { get; set; } = new List<TallNut>();

    /// <summary>
    /// ���۵�ֲ��
    /// </summary>
    public List<PlantAttribute> CardslotPlant { get; set; } = new List<PlantAttribute>();

    /// <summary>
    ///  �����
    /// </summary>
    public int MaxSlot { get; set; } = 2;

    /// <summary>
    /// �Ƿ��ȡ�˴浵����PlantContentʱ���л����ֲ�������
    /// </summary>
    public bool IsLoadPlantData { get; set; }

    /// <summary>
    ///  ��Ҷ���ṩ������
    /// </summary>
    public BloverEffect BloverEffect { get; private set; } = new BloverEffect();

    /// <summary>
    /// �����׮
    /// </summary>
    public TorchwoodEffect TorchwoodEffect { get; private set; } = new TorchwoodEffect();

    /// <summary>
    /// Ĺ���ṩ������
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

        // �����ڱ���ʱ�޸�ֵ�������½�һ���ֵ�洢
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
