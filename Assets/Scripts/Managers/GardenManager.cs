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
                    var plantPrefab = PlantPrefabInfos.GetPlantInfo(item.plantCard.plantType).plant;
                    if (plantPrefab != null)
                    {
                        var plant = GameObject.Instantiate(plantPrefab);
                        plant.plantAttribute = item;
                        PlantDict.Add(item, plant);
                    }
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
}
