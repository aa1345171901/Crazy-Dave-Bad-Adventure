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
    private readonly string CultivateSunReduced = "阳光减少";
    private readonly string CultivateButterProbability = "黄油概率";
    private readonly string CultivateControlTime = "控制时间";
    private readonly string CultivateCriticalHitRate = "暴击率";
    private readonly string CultivateDamageDoubleRate = "伤害段数*2概率";
    private readonly string CultivateCoinGoldCount = "吸取金币个数";
    private readonly string CultivateDuration = "吸取持续时间";
    private readonly string CultivateZombieReducedRate = "吞噬墓碑概率";
    private readonly string CultivateAttackCount = "魅惑者可攻击次数";
    private readonly string CultivateOvereatingRate = "连续触发概率";
    private readonly string CultivateChangeCoin = "换取金币";
    private readonly string CultivateCutterCount = "吸取铁制品个数";
    private readonly string CultivateGoldCoinRate = "掉落金币概率";
    private readonly string CultivateDiamondRate = "掉落钻石概率";
    private readonly string CultivateTwinRate = "掉落双倍概率";
    private readonly string CultivateRangeAttackSpeed = "范围攻速提升";
    private readonly string CultivateRangeLifeResume = "范围生命恢复";
    private readonly string CultivateRangeDamage = "范围伤害增加";
    private readonly string CultivateAddBulletRate = "子弹概率变多";
    private readonly string CultivateBulletSize = "子弹大小";
    private readonly string CultivateReStartResumeLife = "触发后生命值恢复比例";
    private readonly string CultivateAttackPumpkinRate = "攻击掉落南瓜概率";
    private readonly string CultivateDecelerationPercentage = "减速百分比";
    private readonly string CultivateDecelerationTime = "减速时间";
    private readonly string CultivatDestroyingVehiclesCount = "破坏载具数量";
    private readonly string CultivatSunQuality = "生成阳光质量";
    private readonly string CultivatTallNutHp = "高坚果生命值（植物学）";
    private readonly string CultivatBoomRate = "最终爆炸概率";
    private readonly string CultivatCounterInjury = "反伤伤害";
    private readonly string CultivatCounterInjuryRate = "反伤概率";
    private readonly string CultivatPeaDamage = "豌豆增伤";
    private readonly string CultivatPeaSplash = "豌豆溅射伤害";
    private readonly string CultivatPeaSpeed = "豌豆速度";
    private readonly string CultivatRollSpeed = "滚动速度";
    private readonly string CultivatBoomNutRate = "爆炸坚果概率";
    private readonly string CultivatFrostTime = "冰冻时间";
    private readonly string CultivatFrostAttackSpeedAdd = "冰冻期间攻速增加";
    private readonly string CultivatExcavationTime = "出土时间";
    private readonly string CultivatSittingRate = "连坐概率";

    private readonly string CultivatePlayerIncreasedInjury = "玩家对僵尸增伤";
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
    private readonly string CultivatePlayerCoinGold = "金币";
    private readonly string CultivatePlayerArmor = "护甲";
    private readonly string CultivatePlayerSunshine = "阳光";

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
                    SetItemInfo(flowerPotGardenItem, PlantType.Repeater, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage });
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
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatePlayerLucky, CultivateAttackSpeed, CultivateWindSpeed, CultivateWindResume, CultivateWindage });
                    break;
                case PlantType.Cattail:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivatePenetrationCount, CultivateCriticalDamage, CultivateCoolTime, CultivateBulletSpeed, CultivatePlayerLifeResume });
                    break;
                case PlantType.CherryBomb:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateCoolTime, CultivateSunConversionRate, CultivatePlayerAdrenaline, CultivateImmediateMortalityRate, CultivateIncreasedInjury, CultivateSunReduced, CultivateExplosionRange });
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
                    SetItemInfo(flowerPotGardenItem, PlantType.GloomShroom, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateCriticalHitRate, CultivateCriticalDamage, CultivateDamageDoubleRate });
                    break;
                case PlantType.GloomShroom:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateCriticalHitRate, CultivateCriticalDamage, CultivateDamageDoubleRate });
                    break;
                case PlantType.GoldMagent:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatePlayerCoinGold, CultivatePlayerLucky, CultivatePlayerBotany, CultivateCoinGoldCount, CultivateCoolTime, CultivateDuration });
                    break;
                case PlantType.Gralic:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatePlayerLifeResume, CultivatePlayerAdrenaline, CultivatePlayerRange, CultivatePlayerBotany, CultivatePlayerMaxHp, CultivatePlayerAttackSpeed, CultivatePlayerSpeed }, true);
                    break;
                case PlantType.Gravebuster:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateZombieReducedRate, CultivatePlayerIncreasedInjury, CultivateCoinConversionRate, CultivatePlayerCoinGold, CultivatePlayerDamage, CultivatePlayerArmor, CultivateCoolTime });
                    break;
                case PlantType.HypnoShroom:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateAttackCount, CultivateOvereatingRate, CultivatePlayerLucky });
                    break;
                case PlantType.MagentShroom:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatePlayerCoinGold, CultivatePlayerLucky, CultivateCoolTime, CultivateChangeCoin, CultivateCutterCount, CultivateDuration });
                    break;
                case PlantType.Marigold:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatePlayerCoinGold, CultivatePlayerLucky, CultivateCoolTime, CultivateGoldCoinRate, CultivateTwinRate, CultivateDiamondRate });
                    break;
                case PlantType.Plantern:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateRange, CultivateRangeAttackSpeed, CultivateRangeLifeResume, CultivateRangeDamage, CultivatePlayerAttackSpeed, CultivatePlayerRange });
                    break;
                case PlantType.PuffShroom:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage, CultivateAddBulletRate ,CultivateBulletSize });
                    break;
                case PlantType.PumpkinHead:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateReStartResumeLife, CultivatePlayerArmor, CultivatePlayerMaxHp, CultivatePlayerLifeResume, CultivateAttackPumpkinRate});
                    break;
                case PlantType.ScaredyShroom:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage, CultivateAddBulletRate });
                    break;
                case PlantType.SnowPea:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage, CultivateDecelerationPercentage, CultivateDecelerationTime });
                    break;
                case PlantType.Spikerock:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivatDestroyingVehiclesCount, CultivateDecelerationPercentage, CultivateDecelerationTime, CultivatePlayerAdrenaline });
                    break;
                case PlantType.Spikeweed:
                    SetItemInfo(flowerPotGardenItem, PlantType.Spikerock, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivatDestroyingVehiclesCount, CultivateDecelerationPercentage, CultivateDecelerationTime, CultivatePlayerAdrenaline });
                    break;
                case PlantType.SplitPea:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage });
                    break;
                case PlantType.Starfruit:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage, CultivatePlayerLucky });
                    break;
                case PlantType.SunFlower:
                    SetItemInfo(flowerPotGardenItem, PlantType.TwinSunflower, new string[] { CultivatePlayerSunshine, CultivatePlayerLucky, CultivateCoolTime, CultivatSunQuality, CultivateTwinRate});
                    break;
                case PlantType.TallNut:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatTallNutHp, CultivatBoomRate, CultivatCounterInjury, CultivatCounterInjuryRate, CultivatePlayerMaxHp, CultivatePlayerArmor });
                    break;
                case PlantType.Threepeater:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage });
                    break;
                case PlantType.Torchwood:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatPeaDamage, CultivatPeaSplash, CultivatPeaSpeed, CultivatePlayerSunshine, CultivatePlayerAdrenaline });
                    break;
                case PlantType.TwinSunflower:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatePlayerSunshine, CultivatePlayerLucky, CultivateCoolTime, CultivatSunQuality, CultivateTwinRate });
                    break;
                case PlantType.WallNut:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage,CultivatRollSpeed, CultivateCoolTime, CultivatBoomNutRate, CultivatePlayerSpeed });
                    break;
                case PlantType.IceShroom:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivatFrostTime, CultivateCoolTime, CultivateSunReduced, CultivatFrostAttackSpeedAdd, CultivatePlayerLucky, CultivatePlayerLifeResume });
                    break;
                case PlantType.Jalapeno:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateCoolTime, CultivateSunConversionRate, CultivatePlayerAdrenaline, CultivateImmediateMortalityRate, CultivateIncreasedInjury, CultivateSunReduced, CultivateExplosionRange });
                    break;
                case PlantType.DoomShroom:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateCoolTime, CultivateSunConversionRate, CultivatePlayerAdrenaline, CultivateImmediateMortalityRate, CultivateIncreasedInjury, CultivateSunReduced });
                    break;
                case PlantType.Squash:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateCoolTime, CultivateSunConversionRate, CultivateImmediateMortalityRate, CultivateIncreasedInjury, CultivatSittingRate, CultivateSunReduced });
                    break;
                case PlantType.PotatoMine:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateCoolTime, CultivateSunConversionRate, CultivatePlayerAdrenaline, CultivateImmediateMortalityRate, CultivateIncreasedInjury, CultivateSunReduced, CultivateExplosionRange, CultivatExcavationTime });
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

    public void UpdateCard()
    {
        GoToWarItem.SetCard();
    }
}
