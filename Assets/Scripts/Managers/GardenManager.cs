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
    [Tooltip("ֲ����Ԥ����")]
    public GameObject SeedingPrefab;
    [Tooltip("������ֲ�����Ͷ�Ӧ��ֲ��Prefab")]
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
