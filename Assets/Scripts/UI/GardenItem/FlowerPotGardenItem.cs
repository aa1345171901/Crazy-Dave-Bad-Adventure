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
    // �ֶ�ֲ���������ĸ���
    public Action SunChanged;

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
        SunChanged?.Invoke();
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
            // ӣ��ը�� ����5 Ϊ��������
            case PlantType.CherryBomb:
                if (attribute[index] == 5)
                {
                    GameManager.Instance.UserData.Adrenaline++;
                }
                break;
            // ����Ͷ�� ����0 Ϊ�������ֵ
            case PlantType.Cornpult:
                if (attribute[index] == 0)
                {
                    GameManager.Instance.UserData.MaximumHP++;
                }
                break;
            // ���� ����0 Ϊ���,1Ϊ���ˣ�2Ϊֲ��ѧ
            case PlantType.GoldMagent:
                if (attribute[index] == 0)
                {
                    GameManager.Instance.UserData.GoldCoins++;
                }
                if (attribute[index] == 1)
                {
                    GameManager.Instance.UserData.Lucky++;
                }
                if (attribute[index] == 2)
                {
                    GameManager.Instance.UserData.Botany++;
                }
                break;
            // Ĺ�� ����3 Ϊ���,4Ϊ�˺���5Ϊ����
            case PlantType.Gravebuster:
                if (attribute[index] == 3)
                {
                    GameManager.Instance.UserData.GoldCoins++;
                }
                if (attribute[index] == 4)
                {
                    GameManager.Instance.UserData.PercentageDamage++;
                }
                if (attribute[index] == 5)
                {
                    GameManager.Instance.UserData.Armor++;
                }
                break;
            // �Ȼ� ����4Ϊ����
            case PlantType.HypnoShroom:
                if (attribute[index] == 4)
                {
                    GameManager.Instance.UserData.Lucky++;
                }
                break;
            // ������ ����0 Ϊ���,1Ϊ����
            case PlantType.MagentShroom:
                if (attribute[index] == 0)
                {
                    GameManager.Instance.UserData.GoldCoins++;
                }
                if (attribute[index] == 1)
                {
                    GameManager.Instance.UserData.Lucky++;
                }
                break;
            // ��յ�� ����0 Ϊ���,1Ϊ����
            case PlantType.Marigold:
                if (attribute[index] == 0)
                {
                    GameManager.Instance.UserData.GoldCoins++;
                }
                if (attribute[index] == 1)
                {
                    GameManager.Instance.UserData.Lucky++;
                }
                break;
            // ·�� ����4 �����ٶ�,5Ϊ��Χ
            case PlantType.Plantern:
                if (attribute[index] == 4)
                {
                    GameManager.Instance.UserData.AttackSpeed++;
                }
                if (attribute[index] == 5)
                {
                    GameManager.Instance.UserData.Range++;
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
        if (PlantAttribute == null)
            return;
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
                    SetAttribute(8);
                    break;
                case PlantType.Cornpult:
                    SetAttribute(7);
                    break;
                case PlantType.FumeShroom:
                    SetAttribute(7);
                    break;
                case PlantType.GoldMagent:
                    SetAttribute(6);
                    break;
                case PlantType.Gralic:
                    SetAttribute(7);
                    break;
                case PlantType.Gravebuster:
                    SetAttribute(7);
                    break;
                case PlantType.HypnoShroom:
                    SetAttribute(5);
                    break;
                case PlantType.MagentShroom:
                    SetAttribute(6);
                    break;
                case PlantType.Marigold:
                    SetAttribute(6);
                    break;
                case PlantType.Plantern:
                    SetAttribute(6);
                    break;
                case PlantType.PuffShroom:
                    SetAttribute(8);
                    PlantAttribute.maxLevel = int.MaxValue;
                    break;
                case PlantType.PumpkinHead:
                    break;
                case PlantType.ScaredyShroom:
                    break;
                case PlantType.SnowPea:
                    break;
                case PlantType.Spikerock:
                    break;
                case PlantType.Spikeweed:
                    break;
                case PlantType.SplitPea:
                    break;
                case PlantType.Starfruit:
                    break;
                case PlantType.SunFlower:
                    break;
                case PlantType.TallNut:
                    break;
                case PlantType.Threepeater:
                    break;
                case PlantType.Torchwood:
                    break;
                case PlantType.TwinSunflower:
                    break;
                case PlantType.WallNut:
                    break;
                case PlantType.IceShroom:
                    break;
                case PlantType.Jalapeno:
                    break;
                case PlantType.DoomShroom:
                    break;
                case PlantType.Squash:
                    break;
                case PlantType.PotatoMine:
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

    public void Eat()
    {
        GameObject.Destroy(targetPlant);
        GardenManager.Instance.PlantAttributes.Remove(this.PlantAttribute);
        this.PlantAttribute = null;
        plantCultivationPage.EmptyFlowerPot(this.GetComponentInParent<FlowerPotPosition>());
        plantCultivationPage.gameObject.SetActive(false);
    }

    public void UpdateSunPrice()
    {
        plantCultivationPage.UpdateSunPrice();
    }
}
