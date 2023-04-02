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
            PlantUIPrefabInfo plantUIPrefabInfo = GardenManager.Instance.GetPlantUIPrefabInfo(noPlantingPlants[i].plantType);
            flowerPotPos.FlowerPot.SetPlant(plantUIPrefabInfo.plantPrefab, noPlantingPlants[i], plantCultivationPage);
            haveFlowerPotPos.Remove(flowerPotPos);
        }
        noPlantingPlants.Clear();
    }
}
