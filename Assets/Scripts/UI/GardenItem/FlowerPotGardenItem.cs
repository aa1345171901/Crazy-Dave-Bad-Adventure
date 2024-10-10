using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    // 手动植物阳光消耗更改
    public Action SunChanged;

    public PlantAttribute(PlantCard plantCard)
    {
        this.plantCard = plantCard;
        switch (plantCard.plantType)
        {
            case PlantType.CherryBomb:
            case PlantType.IceShroom:
            case PlantType.Jalapeno:
            case PlantType.DoomShroom:
            case PlantType.Squash:
            case PlantType.PotatoMine:
                this.isManual = true;
                break;
            default:
                this.isManual = false;
                break;
        }
    }

    /// <summary>
    /// 培养时调用，增加玩家属性
    /// </summary>
    /// <param name="index">目标等级索引</param>
    public void AddAttribute(int index)
    {
        SunChanged?.Invoke();
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
            // 樱桃炸弹 属性4 为肾上腺素
            case PlantType.CherryBomb:
            case PlantType.Jalapeno:
            case PlantType.DoomShroom:
            case PlantType.PotatoMine:
                if (attribute[index] == 4)
                {
                    GameManager.Instance.UserData.Adrenaline++;
                }
                break;
            // 玉米投手 属性0 为最大生命值
            case PlantType.Cornpult:
                if (attribute[index] == 0)
                {
                    GameManager.Instance.UserData.MaximumHP++;
                }
                break;
            // 吸金菇 属性0 为金币,1为幸运，2为植物学
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
            // 墓碑 属性3 为金币,4为伤害，5为护甲
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
            // 魅惑菇 属性4为幸运
            case PlantType.HypnoShroom:
                if (attribute[index] == 4)
                {
                    GameManager.Instance.UserData.Lucky++;
                }
                break;
            // 磁力菇 属性0 为金币,1为幸运
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
            // 金盏花 属性0 为金币,1为幸运
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
            // 路灯 属性4 攻击速度,5为范围
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
            // 南瓜 属性1 护甲,2为最大生命值3生命恢复
            case PlantType.PumpkinHead:
                if (attribute[index] == 1)
                {
                    GameManager.Instance.UserData.Armor++;
                }
                if (attribute[index] == 2)
                {
                    GameManager.Instance.UserData.MaximumHP++;
                }
                if (attribute[index] == 3)
                {
                    GameManager.Instance.UserData.LifeRecovery++;
                }
                break;
            // 地刺 属性5为肾上腺素 
            case PlantType.Spikeweed:
            case PlantType.Spikerock:
                if (attribute[index] == 5)
                {
                    GameManager.Instance.UserData.Adrenaline++;
                }
                break;
            // 杨桃 属性5为幸运
            case PlantType.Starfruit:
                if (attribute[index] == 5)
                {
                    GameManager.Instance.UserData.Lucky++;
                }
                break;
            // 向日葵 属性0 为阳光,1为幸运
            case PlantType.SunFlower:
                if (attribute[index] == 0)
                {
                    GameManager.Instance.UserData.Sunshine += 25;
                }
                if (attribute[index] == 1)
                {
                    GameManager.Instance.UserData.Lucky++;
                }
                break;
            // 双子向日葵 属性0 为阳光,1为幸运
            case PlantType.TwinSunflower:
                if (attribute[index] == 0)
                {
                    GameManager.Instance.UserData.Sunshine += 25;
                }
                if (attribute[index] == 1)
                {
                    GameManager.Instance.UserData.Lucky++;
                }
                break;
            // 高坚果 属性4 为最大生命值,5为护甲
            case PlantType.TallNut:
                if (attribute[index] == 4)
                {
                    GameManager.Instance.UserData.MaximumHP++;
                }
                if (attribute[index] == 5)
                {
                    GameManager.Instance.UserData.Armor++;
                }
                break;
            // 火炬树桩 属性3 为阳光,4为肾上腺素
            case PlantType.Torchwood:
                if (attribute[index] == 0)
                {
                    GameManager.Instance.UserData.Sunshine += 25;
                }
                if (attribute[index] == 1)
                {
                    GameManager.Instance.UserData.Adrenaline++;
                }
                break;
            // 坚果墙 5速度
            case PlantType.WallNut:
                if (attribute[index] == 5)
                {
                    GameManager.Instance.UserData.Speed++;
                }
                break;
            // 寒冰菇 3幸运 4生命恢复
            case PlantType.IceShroom:
                if (attribute[index] == 3)
                {
                    GameManager.Instance.UserData.Lucky++;
                }
                if (attribute[index] == 4)
                {
                    GameManager.Instance.UserData.LifeRecovery++;
                }
                break;
            default:
                break;
        }
    }

    public void CultivatePlant(bool isLoad = false)
    {
        if (!isLoad)
            AchievementManager.Instance.SetAchievementType5((int)plantCard.plantType);
        if (!isCultivate)
        {
            // 培育成型设置培养的属性
            switch (plantCard.plantType)
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
                    maxLevel = int.MaxValue;
                    break;
                case PlantType.PumpkinHead:
                    SetAttribute(5);
                    break;
                case PlantType.ScaredyShroom:
                    SetAttribute(7);
                    break;
                case PlantType.SnowPea:
                    SetAttribute(8);
                    break;
                case PlantType.Spikeweed:
                    SetAttribute(6);
                    break;
                case PlantType.SplitPea:
                    SetAttribute(6);
                    break;
                case PlantType.Starfruit:
                    SetAttribute(6);
                    break;
                case PlantType.SunFlower:
                    SetAttribute(5);
                    break;
                case PlantType.TallNut:
                    SetAttribute(6);
                    break;
                case PlantType.Threepeater:
                    SetAttribute(6);
                    break;
                case PlantType.Torchwood:
                    SetAttribute(5);
                    break;
                case PlantType.WallNut:
                    SetAttribute(6);
                    break;
                case PlantType.IceShroom:
                    SetAttribute(5);
                    break;
                case PlantType.Jalapeno:
                    SetAttribute(9);
                    break;
                case PlantType.DoomShroom:
                    SetAttribute(8);
                    break;
                case PlantType.Squash:
                    SetAttribute(8);
                    break;
                case PlantType.PotatoMine:
                    SetAttribute(10);
                    break;
                case PlantType.CobCannon:
                    SetAttribute(7);
                    break;
                default:
                    break;
            }

            isCultivate = true;
        }

        void SetAttribute(int len)
        {
            var hash = RandomUtils.RandomCreateNumber(len, 3);
            int index = 0;
            foreach (var item in hash)
            {
                attribute[index] = item;
                index++;
            }
        }
    }
}

public class FlowerPotGardenItem : MonoBehaviour
{
    public PlantAttribute PlantAttribute = null;

    private PlantCultivationPage plantCultivationPage;
    private GameObject seeding;
    private GameObject targetPlantPrefab;
    public GameObject targetPlant;

    private Animator animator;

    public void SetPlant(GameObject targetPlant, PlantCard plantCard, PlantCultivationPage plantCultivationPage)
    {
        // 重新播放，与植物动画一致
        animator = GetComponent<Animator>();
        animator.Play("Idel");
        this.plantCultivationPage = plantCultivationPage;
        this.targetPlantPrefab = targetPlant;
        this.PlantAttribute = new PlantAttribute(plantCard);
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
        if (PlantAttribute == null)
            return;
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
            PlantAttribute.CultivatePlant();
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

    public void Sell()
    {
        if (plantCultivationPage == null)
        {
            AudioManager.Instance.PlayEffectSoundByName("NoSun");
            return;
        }
        int price = PlantAttribute.plantCard.defaultPrice + PlantAttribute.level1 + PlantAttribute.level2 + PlantAttribute.level3;
        ShopManager.Instance.Money += price;
        GardenManager.Instance.PlantAttributes.Remove(this.PlantAttribute);
        GardenManager.Instance.CardslotPlant.Remove(this.PlantAttribute);
        plantCultivationPage.gameObject.SetActive(false);
        GameObject.Destroy(this.gameObject);
    }

    public int GetPrice()
    {
        return PlantAttribute.plantCard.defaultPrice + PlantAttribute.level1 + PlantAttribute.level2 + PlantAttribute.level3;
    }
}
