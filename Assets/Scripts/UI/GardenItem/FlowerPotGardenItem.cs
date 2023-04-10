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

    // 为了泛用性，在生成的时候按顺序取值，可能每个植物增加的属性都不相同，一般取三个属性增长，如有特殊则后续再设计,其中value为等级，等级增长后属性如何增加由具体植物中设计
    public int level1;
    public int level2;
    public int level3;
    public int maxLevel = 10;

    // attribute为随机属性顺序，对应目标植物的对应属性，在培育完成后即设置，只能一次
    public int[] attribute = new int[3];
    // 植物是否是手动的
    public bool isManual;
    // 手动的该植物是否出战
    public bool isGoToWar;

    public PlantAttribute(PlantCard plantCard, bool isManual = false)
    {
        this.plantCard = plantCard;
        this.isManual = isManual;
    }

    /// <summary>
    /// 培养时调用，增加玩家属性
    /// </summary>
    /// <param name="index">目标等级索引</param>
    public void AddAttribute(int index)
    {
        switch (plantCard.plantType)
        {
            // 三叶草 属性增益0 为主角增加幸运， 1为主角增加攻击速度， 出售时直接减去level  todo
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
            // 香蒲 属性6 为生命恢复活血止痛
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
        // 重新播放，与植物动画一致
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
        // 重新播放，与植物动画一致
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
        // 已经培养的荷叶不需要打开培养页面，后续种植香蒲，直接将PlantAttribute替换荷叶的
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
            // 培育成型设置培养的属性
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
            // 荷叶不需要更新
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
