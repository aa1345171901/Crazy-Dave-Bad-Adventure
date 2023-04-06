using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[Serializable]
public class PlantAttribute
{
    public PlantCard plantCard;
    public bool isCultivate;

    // Ϊ�˷����ԣ������ɵ�ʱ��˳��ȡֵ������ÿ��ֲ�����ӵ����Զ�����ͬ��һ��ȡ��������������������������������,����valueΪ�ȼ����ȼ�������������������ɾ���ֲ�������
    public int level1;
    public int level2;
    public int level3;
    public int maxLevel = 10;

    // attributeΪ�������˳�򣬶�ӦĿ��ֲ��Ķ�Ӧ���ԣ���������ɺ����ã�ֻ��һ��
    public int[] attribute = new int[3];

    public PlantAttribute(PlantCard plantCard)
    {
        this.plantCard = plantCard;
    }
}

public class FlowerPotGardenItem : MonoBehaviour
{
    public PlantAttribute PlantAttribute = null;

    private PlantCultivationPage plantCultivationPage;
    private GameObject seeding;
    private GameObject taegetPlantPrefab;
    private GameObject targetPlant;

    private Animator animator;

    public void SetPlant(GameObject taegetPlant, PlantCard plantCard, PlantCultivationPage plantCultivationPage)
    {
        // ���²��ţ���ֲ�ﶯ��һ��
        animator = GetComponent<Animator>();
        animator.Play("Idel");
        this.plantCultivationPage = plantCultivationPage;
        this.taegetPlantPrefab = taegetPlant;
        this.PlantAttribute = new PlantAttribute(plantCard);
        seeding = GameObject.Instantiate(GardenManager.Instance.SeedingPrefab, this.transform);
        GardenManager.Instance.PlantAttributes.Add(this.PlantAttribute);
    }

    private void OnMouseDown()
    {
        if (plantCultivationPage != null && plantCultivationPage.FlowerPotGardenItem != this)
        {
            AudioManager.Instance.PlayEffectSoundByName("pageExpansion", UnityEngine.Random.Range(1f, 1.2f));
            plantCultivationPage.gameObject.SetActive(true);
            plantCultivationPage.SetPlantAttribute(this);
        }
    }

    public void CultivatePlant()
    {
        if (!PlantAttribute.isCultivate)
        {
            // ����������������������
            switch (PlantAttribute.plantCard.plantType)
            {
                case PlantType.Peashooter:
                    var hash = RandomUtils.RandomCreateNumber(6, 3);
                    int index = 0;
                    foreach (var item in hash)
                    {
                        PlantAttribute.attribute[index] = item;
                        index++;
                    }
                    break;
                case PlantType.Cactus:
                    break;
                case PlantType.Blover:
                    break;
                case PlantType.Cattail:
                    break;
                case PlantType.CherryBomb:
                    break;
                case PlantType.Chomper:
                    break;
                case PlantType.CoffeeBean:
                    break;
                case PlantType.Cornpult:
                    break;
                case PlantType.FumeShroom:
                    break;
                case PlantType.GloomShroom:
                    break;
                case PlantType.GoldMagent:
                    break;
                default:
                    break;
            }

            PlantAttribute.isCultivate = true;
            GameObject.Destroy(seeding);
            animator.Play("Idel", 0, 0);
            targetPlant = GameObject.Instantiate(taegetPlantPrefab, this.transform);
            plantCultivationPage.SetPlantAttribute(this);
            UpdateSunPrice();
        }
    }

    public void Evolution(PlantCard plantCard)
    {
        taegetPlantPrefab = GardenManager.Instance.PlantUIPrefabInfos.GetPlantInfo(plantCard.plantType).plantPrefab;
        GameObject.Destroy(targetPlant);
        animator.Play("Idel", 0, 0);
        targetPlant = GameObject.Instantiate(taegetPlantPrefab, this.transform);
        PlantAttribute.plantCard = plantCard;
        plantCultivationPage.SetPlantAttribute(this);
        UpdateSunPrice();
    }

    public void UpdateSunPrice()
    {
        plantCultivationPage.UpdateSunPrice();
    }
}
