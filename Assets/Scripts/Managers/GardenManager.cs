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
    [Tooltip("������ֲ�����Ͷ�Ӧ��ֲ��Prefab")]
    public List<PlantUIPrefabInfo> PlantUIPrefabInfos;

    /// <summary>
    /// ���ι��ﹺ���û�з��õĻ�������
    /// </summary>
    public int NotPlacedFlowerPotCount { get; set; }

    public int FlowerPotCount { get; set; }

    /// <summary>
    /// �����ڷ�����
    /// </summary>
    public int MaxFlowerPotCount { get; set; } = 16;
}
