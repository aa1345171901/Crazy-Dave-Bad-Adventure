using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantConent : MonoBehaviour
{
    [Tooltip("����ҳ��,������Ҫ��ֵ�����ɵĻ���")]
    public PlantCultivationPage plantCultivationPage;

    /// <summary>
    /// ���������λ����Ϣ
    /// </summary>
    private List<FlowerPotPosition> flowerPotPositions;

    /// <summary>
    /// �ܹ����û����λ��
    /// </summary>
    private List<FlowerPotPosition> canLayUpFlowerPotPos = new List<FlowerPotPosition>();

    /// <summary>
    /// �л����λ��
    /// </summary>
    private List<FlowerPotPosition> haveFlowerPotPos = new List<FlowerPotPosition>();

    private void Start()
    {
        flowerPotPositions = new List<FlowerPotPosition>(this.GetComponentsInChildren<FlowerPotPosition>());

        foreach (var item in flowerPotPositions)
        {
            if (!item.HaveEarth && item.FlowerPot == null)
            {
                canLayUpFlowerPotPos.Add(item);
            }
        }
    }

    public void CreateFlowerPot()
    {
        // ���μ����Ƿ��Ƕ�ȡ�浵
        if (GardenManager.Instance.IsLoadPlantData)
        {
            int flowerpotCount = GardenManager.Instance.FlowerPotCount;
            // �Ȱѻ����������
            for (int i = 0; i < flowerpotCount; i++)
            {
                int index = Random.Range(0, canLayUpFlowerPotPos.Count);
                var flowerPotPos = canLayUpFlowerPotPos[index];
                flowerPotPos.CreateFlowerPot();
                canLayUpFlowerPotPos.Remove(flowerPotPos);
                haveFlowerPotPos.Add(flowerPotPos);
            }
            // ֲ��λ��������ɣ��Լ����������Լ��Ƿ�����
            var plants = GardenManager.Instance.PlantAttributes;
            foreach (var item in plants)
            {
                int index = Random.Range(0, haveFlowerPotPos.Count);
                var flowerPotPos = haveFlowerPotPos[index];
                PlantUIPrefabInfo plantUIPrefabInfo = GardenManager.Instance.PlantUIPrefabInfos.GetPlantInfo(item.plantCard.plantType);
                if (plantUIPrefabInfo == null)
                    plantUIPrefabInfo = GardenManager.Instance.PlantUIPrefabInfos.GetPlantInfo(PlantType.None);
                flowerPotPos.FlowerPot.LoadPlant(item, plantUIPrefabInfo.plantPrefab, plantCultivationPage);
                haveFlowerPotPos.Remove(flowerPotPos);
            }
            GardenManager.Instance.IsLoadPlantData = false;
        }
        for (int i = 0; i < GardenManager.Instance.NotPlacedFlowerPotCount; i++)
        {
            // ���λ������ ����
            int index = Random.Range(0, canLayUpFlowerPotPos.Count);
            var flowerPotPos = canLayUpFlowerPotPos[index];
            flowerPotPos.CreateFlowerPot();
            canLayUpFlowerPotPos.Remove(flowerPotPos);
            GardenManager.Instance.FlowerPotCount++;
            haveFlowerPotPos.Add(flowerPotPos);
        }
        GardenManager.Instance.NotPlacedFlowerPotCount = 0;
        CreatePlant();
    }

    public void CreatePlant()
    {
        var noPlantingPlants = GardenManager.Instance.NoPlantingPlants;
        for (int i = 0; i < noPlantingPlants.Count; i++)
        {
            int index = Random.Range(0, haveFlowerPotPos.Count);
            var flowerPotPos = haveFlowerPotPos[index];
            PlantUIPrefabInfo plantUIPrefabInfo = GardenManager.Instance.PlantUIPrefabInfos.GetPlantInfo(noPlantingPlants[i].plantType);
            if (plantUIPrefabInfo == null)
                plantUIPrefabInfo = GardenManager.Instance.PlantUIPrefabInfos.GetPlantInfo(PlantType.None);
            flowerPotPos.FlowerPot.SetPlant(plantUIPrefabInfo.plantPrefab, noPlantingPlants[i], plantCultivationPage);
            haveFlowerPotPos.Remove(flowerPotPos);
        }
        noPlantingPlants.Clear();
    }
}
