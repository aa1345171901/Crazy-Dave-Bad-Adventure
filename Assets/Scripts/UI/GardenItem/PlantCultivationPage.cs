using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class PlantCultivationPage : MonoBehaviour
{
    public List<PlantCultivationItem> plantCultivationItems;
    public Text InfoText;
    [Tooltip("进化按钮")]
    public EvolutionItem EvolutionItem;
    [Tooltip("出战按钮")]
    public GoToWarItem GoToWarItem;
    [Tooltip("吃按钮")]
    public EatItem EatItem;

    public PlantConent PlantConent { get; set; }

    /// <summary>
    /// 主要用于页面显示的正确
    /// </summary>
    public FlowerPotGardenItem FlowerPotGardenItem { get; private set; }

    private Camera UICamera;
    private RectTransform rectTransform;

    private readonly string CultivateInfo = "培育";
    private readonly string CultivateBasicDamage = "基础伤害";
    private readonly string CultivatePercentageDamage = "百分比伤害";
    private readonly string CultivateRange = "攻击检测范围";
    private readonly string CultivateCoolTime = "攻击冷却";
    private readonly string CultivateBulletSpeed = "子弹速度";
    private readonly string CultivateSplashDamage = "溅射伤害";
    private readonly string CultivateDigestiveSpeed = "消化速度";
    private readonly string CultivateSwallowCount = "一次性吞噬个数";
    private readonly string CultivateCoinConversionRate = "金币率转换";
    private readonly string CultivateSunConversionRate = "阳光率转换";
    private readonly string CultivatePenetrationCount = "穿透数量";
    private readonly string CultivateCriticalDamage = "暴击伤害";
    private readonly string CultivateAttackSpeed = "玩家攻击速度";
    private readonly string CultivateWindSpeed = "风速";
    private readonly string CultivateWindResume = "逆风恢复";
    private readonly string CultivateWindage = "僵尸风阻";
    private readonly string CultivateExplosionRange = "爆炸范围";
    private readonly string CultivateImmediateMortalityRate = "普通僵尸即死率";
    private readonly string CultivateIncreasedInjury = "大型僵尸增伤";
    private readonly string CultivateSunReduced = "阳光";
    private readonly string CultivateButterProbability = "黄油概率";
    private readonly string CultivateControlTime = "控制时间";

    private readonly string CultivatePlayerAdrenaline = "肾上腺素";
    private readonly string CultivatePlayerLifeResume = "生命恢复";
    private readonly string CultivatePlayerLucky = "幸运";
    private readonly string CultivatePlayerBotany = "植物学";
    private readonly string CultivatePlayerRange = "范围";
    private readonly string CultivatePlayerDamage = "伤害";
    private readonly string CultivatePlayerAttackSpeed = "攻击速度";
    private readonly string CultivatePlayerSpeed = "速度";
    private readonly string CultivatePlayerPower = "力量";
    private readonly string CultivatePlayerMaxHp = "最大生命值";

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && FlowerPotGardenItem != null && gameObject.activeSelf
            && !BoundsUtils.GetSceneRect(UICamera, FlowerPotGardenItem.GetComponent<RectTransform>()).Contains(Input.mousePosition)
            && !BoundsUtils.GetAnchorLeftRect(UICamera, rectTransform).Contains(Input.mousePosition))
        {
            gameObject.SetActive(false);
            FlowerPotGardenItem = null;
            AudioManager.Instance.PlayEffectSoundByName("pageExpansion", Random.Range(0.8f, 1f));
        }
    }

    public void SetPlantAttribute(FlowerPotGardenItem flowerPotGardenItem)
    {
        UpdatePos(flowerPotGardenItem);
        EvolutionItem.gameObject.SetActive(false);
        GoToWarItem.gameObject.SetActive(false);
        EatItem.gameObject.SetActive(false);
        this.FlowerPotGardenItem = flowerPotGardenItem;
        this.InfoText.text = flowerPotGardenItem.PlantAttribute.plantCard.plantName;
        // 还未培育成型
        if (!flowerPotGardenItem.PlantAttribute.isCultivate)
        {
            var plantCultivateItem = plantCultivationItems[0];
            foreach (var item in plantCultivationItems)
            {
                item.gameObject.SetActive(false);
            }
            plantCultivateItem.gameObject.SetActive(true);
            plantCultivateItem.SetInfo(CultivateAttributeType.Cultivate, flowerPotGardenItem, CultivateInfo);
        }
        else
        {
            // 根据植物进行属性培养提示以及进化按钮展示
            switch (flowerPotGardenItem.PlantAttribute.plantCard.plantType)
            {
                case PlantType.Peashooter:
                    SetItemInfo(flowerPotGardenItem, PlantType.Repeater, new string[] {CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage });
                    break;
                case PlantType.Repeater:
                    SetItemInfo(flowerPotGardenItem, PlantType.GatlingPea, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage });
                    break;
                case PlantType.GatlingPea:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage });
                    break;
                case PlantType.Cactus:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatePenetrationCount, CultivateCoolTime, CultivateBasicDamage, CultivatePercentageDamage, CultivateBulletSpeed, CultivateCriticalDamage });
                    break;
                case PlantType.Blover:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatePlayerLucky, CultivateAttackSpeed, CultivateWindSpeed, CultivateWindResume, CultivateWindage});
                    break;
                case PlantType.Cattail:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivatePenetrationCount, CultivateCriticalDamage, CultivateCoolTime, CultivateBulletSpeed, CultivatePlayerLifeResume });
                    break;
                case PlantType.CherryBomb:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateExplosionRange, CultivateBasicDamage, CultivatePercentageDamage, CultivateCoolTime, CultivateSunConversionRate, CultivatePlayerAdrenaline, CultivateImmediateMortalityRate, CultivateIncreasedInjury, CultivateSunReduced });
                    break;
                case PlantType.Chomper:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateDigestiveSpeed, CultivateRange, CultivateSwallowCount, CultivateCoinConversionRate, CultivateSunConversionRate, CultivateBasicDamage, CultivatePercentageDamage });
                    break;
                case PlantType.CoffeeBean:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatePlayerAdrenaline, CultivatePlayerLucky, CultivatePlayerBotany, CultivatePlayerRange, CultivatePlayerDamage, CultivatePlayerAttackSpeed, CultivatePlayerSpeed, CultivatePlayerPower }, true);
                    break;
                case PlantType.Cornpult:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatePlayerMaxHp, CultivateBasicDamage, CultivatePercentageDamage, CultivateButterProbability, CultivateCoolTime, CultivateBulletSpeed, CultivateControlTime });
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
        }
    }

    private void UpdatePos(FlowerPotGardenItem flowerPotGardenItem)
    {
        if (UICamera == null)
        {
            UICamera = UIManager.Instance.UICamera;
            rectTransform = this.GetComponent<RectTransform>();
        }
        rectTransform.position = flowerPotGardenItem.transform.position;
        Rect bounds = BoundsUtils.GetAnchorLeftRect(UICamera, rectTransform);
        if (bounds.xMax > Screen.width)
        {
            var pos = transform.position;
            // 100 为canvas每单位像素
            pos.x = transform.position.x - bounds.width / 100;
            transform.position = pos;
        }
    }

    private void SetItemInfo(FlowerPotGardenItem flowerPotGardenItem, PlantType plantType, string[] infos, bool canEat = false)
    {
        for (int i = 0; i < plantCultivationItems.Count; i++)
        {
            plantCultivationItems[i].gameObject.SetActive(true);
            plantCultivationItems[i].SetInfo((CultivateAttributeType)(i + 1), flowerPotGardenItem, infos[flowerPotGardenItem.PlantAttribute.attribute[i]]);
        }

        var purchasedPlantEvolutionDicts = ShopManager.Instance.PurchasedPlantEvolutionDicts;
        var plantEvolutionDict = ShopManager.Instance.PlantEvolutionDict;
        if (purchasedPlantEvolutionDicts.ContainsKey(plantType))
        {
            if (purchasedPlantEvolutionDicts[plantType] > 0)
            {
                EvolutionItem.gameObject.SetActive(true);
                EvolutionItem.SetTarget(flowerPotGardenItem, plantEvolutionDict[plantType]);
            }
        }

        GoToWarItem.SetTarget(flowerPotGardenItem);

        if (canEat)
            EatItem.SetTarget(flowerPotGardenItem);
    }

    public void UpdateSunPrice()
    {
        foreach (var item in plantCultivationItems)
        {
            item.UpdateSunPrice();
        }
    }

    public void EmptyFlowerPot(FlowerPotPosition flowerPotPosition)
    {
        PlantConent.EmptyFlowerPot(flowerPotPosition);
    }
}
