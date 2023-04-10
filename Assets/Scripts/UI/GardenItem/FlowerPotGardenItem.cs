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
    // ֲ���Ƿ����ֶ���
    public bool isManual;
    // �ֶ��ĸ�ֲ���Ƿ��ս
    public bool isGoToWar;

    public PlantAttribute(PlantCard plantCard, bool isManual = false)
    {
        this.plantCard = plantCard;
        this.isManual = isManual;
    }

    /// <summary>
    /// ����ʱ���ã������������
    /// </summary>
    /// <param name="index">Ŀ��ȼ�����</param>
    public void AddAttribute(int index)
    {
        switch (plantCard.plantType)
        {
            // ��Ҷ�� ��������0 Ϊ�����������ˣ� 1Ϊ�������ӹ����ٶȣ� ����ʱֱ�Ӽ�ȥlevel  todo
            case PlantType.Blover:
                if (attribute[index] == 0)
                {
                    GameManager.Instance.UserData.Lucky++;
                }

                if (attribute[index] == 1)
                {
                    GameManager.Instance.UserData.AttackSpeed++;
                }
                break;
            // ���� ����6 Ϊ�����ָ���Ѫֹʹ
            case PlantType.Cattail:
                if (attribute[index] == 6)
                {
                    GameManager.Instance.UserData.LifeRecovery++;
                }
                break;
            default:
                break;
        }
    }
}

public class FlowerPotGardenItem : MonoBehaviour
{
    public PlantAttribute PlantAttribute = null;

    private PlantCultivationPage plantCultivationPage;
    private GameObject seeding;
    private GameObject targetPlantPrefab;
    private GameObject targetPlant;

    private Animator animator;

    public void SetPlant(GameObject targetPlant, PlantCard plantCard, PlantCultivationPage plantCultivationPage)
    {
        // ���²��ţ���ֲ�ﶯ��һ��
        animator = GetComponent<Animator>();
        animator.Play("Idel");
        this.plantCultivationPage = plantCultivationPage;
        this.targetPlantPrefab = targetPlant;
        switch (plantCard.plantType)
        {
            case PlantType.CherryBomb:
                this.PlantAttribute = new PlantAttribute(plantCard, true);
                break;
            default:
                this.PlantAttribute = new PlantAttribute(plantCard);
                break;
        }
        seeding = GameObject.Instantiate(GardenManager.Instance.SeedingPrefab, this.transform);
        GardenManager.Instance.PlantAttributes.Add(this.PlantAttribute);
    }

    public void SetCattail(GameObject targetPlant, PlantCard plantCard)
    {
        // ���²��ţ���ֲ�ﶯ��һ��
        animator = GetComponent<Animator>();
        animator.Play("Idel");
        seeding = this.targetPlant;
        this.targetPlantPrefab = targetPlant;
        GardenManager.Instance.PlantAttributes.Remove(this.PlantAttribute);
        this.PlantAttribute = new PlantAttribute(plantCard);
        GardenManager.Instance.PlantAttributes.Add(this.PlantAttribute);
    }

    public void LoadPlant(PlantAttribute plantAttribute, GameObject targetPlant, PlantCultivationPage plantCultivationPage)
    {
        animator = GetComponent<Animator>();
        animator.Play("Idel");
        this.PlantAttribute = plantAttribute;
        if (plantAttribute.isCultivate)
            this.targetPlant = GameObject.Instantiate(targetPlant, this.transform);
        else
        {
            this.targetPlantPrefab = targetPlant;
            seeding = GameObject.Instantiate(GardenManager.Instance.SeedingPrefab, this.transform);
        }
        this.plantCultivationPage = plantCultivationPage;
    }

    private void OnMouseDown()
    {
        // �Ѿ������ĺ�Ҷ����Ҫ������ҳ�棬������ֲ���ѣ�ֱ�ӽ�PlantAttribute�滻��Ҷ��
        if (PlantAttribute.plantCard.plantType == PlantType.Lilypad)
        {
            if (PlantAttribute.isCultivate)
                return;
        }
        if (plantCultivationPage != null &&(plantCultivationPage.FlowerPotGardenItem != this || !plantCultivationPage.gameObject.activeSelf))
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
                    SetAttribute(6);
                    break;
                case PlantType.Cactus:
                    SetAttribute(6);
                    break;
                case PlantType.Blover:
                    SetAttribute(5);
                    break;
                case PlantType.Cattail:
                    SetAttribute(7);
                    break;
                case PlantType.CherryBomb:
                    SetAttribute(9);
                    break;
                case PlantType.Chomper:
                    SetAttribute(7);
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
            targetPlant = GameObject.Instantiate(targetPlantPrefab, this.transform);
            plantCultivationPage.SetPlantAttribute(this);
            // ��Ҷ����Ҫ����
            if (PlantAttribute.plantCard.plantType != PlantType.Lilypad)
                UpdateSunPrice();
            else
                plantCultivationPage.gameObject.SetActive(false);
        }
    }

    private void SetAttribute(int len)
    {
        var hash = RandomUtils.RandomCreateNumber(len, 3);
        int index = 0;
        foreach (var item in hash)
        {
            PlantAttribute.attribute[index] = item;
            index++;
        }
    }

    public void Evolution(PlantCard plantCard)
    {
        targetPlantPrefab = GardenManager.Instance.PlantUIPrefabInfos.GetPlantInfo(plantCard.plantType).plantPrefab;
        GameObject.Destroy(targetPlant);
        animator.Play("Idel", 0, 0);
        targetPlant = GameObject.Instantiate(targetPlantPrefab, this.transform);
        PlantAttribute.plantCard = plantCard;
        plantCultivationPage.SetPlantAttribute(this);
        UpdateSunPrice();
    }

    public void UpdateSunPrice()
    {
        plantCultivationPage.UpdateSunPrice();
    }
}
