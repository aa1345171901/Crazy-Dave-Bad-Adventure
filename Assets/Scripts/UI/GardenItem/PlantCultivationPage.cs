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

    private readonly string CultivateInfo = GameTool.LocalText("cultivation_plant1");
    private readonly string CultivateBasicDamage = GameTool.LocalText("cultivation_plant2");
    private readonly string CultivatePercentageDamage = GameTool.LocalText("cultivation_plant3");
    private readonly string CultivateRange = GameTool.LocalText("cultivation_plant4");
    private readonly string CultivateCoolTime = GameTool.LocalText("cultivation_plant5");
    private readonly string CultivateBulletSpeed = GameTool.LocalText("cultivation_plant6");
    private readonly string CultivateSplashDamage = GameTool.LocalText("cultivation_plant7");
    private readonly string CultivateDigestiveSpeed = GameTool.LocalText("cultivation_plant8");
    private readonly string CultivateSwallowCount = GameTool.LocalText("cultivation_plant9");
    private readonly string CultivateCoinConversionRate = GameTool.LocalText("cultivation_plant10");
    private readonly string CultivateSunConversionRate = GameTool.LocalText("cultivation_plant11");
    private readonly string CultivatePenetrationCount = GameTool.LocalText("cultivation_plant12");
    private readonly string CultivateCriticalDamage = GameTool.LocalText("cultivation_plant13");
    private readonly string CultivateAttackSpeed = GameTool.LocalText("cultivation_plant14");
    private readonly string CultivateWindSpeed = GameTool.LocalText("cultivation_plant15");
    private readonly string CultivateWindResume = GameTool.LocalText("cultivation_plant16");
    private readonly string CultivateWindage = GameTool.LocalText("cultivation_plant17");
    private readonly string CultivateExplosionRange = GameTool.LocalText("cultivation_plant18");
    private readonly string CultivateImmediateMortalityRate = GameTool.LocalText("cultivation_plant19");
    private readonly string CultivateIncreasedInjury = GameTool.LocalText("cultivation_plant20");
    private readonly string CultivateSunReduced = GameTool.LocalText("cultivation_plant21");
    private readonly string CultivateButterProbability = GameTool.LocalText("cultivation_plant22");
    private readonly string CultivateControlTime = GameTool.LocalText("cultivation_plant23");
    private readonly string CultivateCriticalHitRate = GameTool.LocalText("cultivation_plant24");
    private readonly string CultivateDamageDoubleRate = GameTool.LocalText("cultivation_plant25");
    private readonly string CultivateCoinGoldCount = GameTool.LocalText("cultivation_plant26");
    private readonly string CultivateDuration = GameTool.LocalText("cultivation_plant27");
    private readonly string CultivateZombieReducedRate = GameTool.LocalText("cultivation_plant28");
    private readonly string CultivateAttackCount = GameTool.LocalText("cultivation_plant29");
    private readonly string CultivateOvereatingRate = GameTool.LocalText("cultivation_plant30");
    private readonly string CultivateChangeCoin = GameTool.LocalText("cultivation_plant31");
    private readonly string CultivateCutterCount = GameTool.LocalText("cultivation_plant32");
    private readonly string CultivateGoldCoinRate = GameTool.LocalText("cultivation_plant33");
    private readonly string CultivateDiamondRate = GameTool.LocalText("cultivation_plant34");
    private readonly string CultivateTwinRate = GameTool.LocalText("cultivation_plant35");
    private readonly string CultivateRangeAttackSpeed = GameTool.LocalText("cultivation_plant36");
    private readonly string CultivateRangeLifeResume = GameTool.LocalText("cultivation_plant37");
    private readonly string CultivateRangeDamage = GameTool.LocalText("cultivation_plant38");
    private readonly string CultivateAddBulletRate = GameTool.LocalText("cultivation_plant39");
    private readonly string CultivateBulletSize = GameTool.LocalText("cultivation_plant40");
    private readonly string CultivateReStartResumeLife = GameTool.LocalText("cultivation_plant41");
    private readonly string CultivateAttackPumpkinRate = GameTool.LocalText("cultivation_plant42");
    private readonly string CultivateDecelerationPercentage = GameTool.LocalText("cultivation_plant43");
    private readonly string CultivateDecelerationTime = GameTool.LocalText("cultivation_plant44");
    private readonly string CultivatDestroyingVehiclesCount = GameTool.LocalText("cultivation_plant45");
    private readonly string CultivatSunQuality = GameTool.LocalText("cultivation_plant46");
    private readonly string CultivatTallNutHp = GameTool.LocalText("cultivation_plant47");
    private readonly string CultivatBoomRate = GameTool.LocalText("cultivation_plant48");
    private readonly string CultivatCounterInjury = GameTool.LocalText("cultivation_plant49");
    private readonly string CultivatCounterInjuryRate = GameTool.LocalText("cultivation_plant50");
    private readonly string CultivatPeaDamage = GameTool.LocalText("cultivation_plant51");
    private readonly string CultivatPeaSplash = GameTool.LocalText("cultivation_plant52");
    private readonly string CultivatPeaSpeed = GameTool.LocalText("cultivation_plant53");
    private readonly string CultivatRollSpeed = GameTool.LocalText("cultivation_plant54");
    private readonly string CultivatBoomNutRate = GameTool.LocalText("cultivation_plant55");
    private readonly string CultivatFrostTime = GameTool.LocalText("cultivation_plant56");
    private readonly string CultivatFrostAttackSpeedAdd = GameTool.LocalText("cultivation_plant57");
    private readonly string CultivatExcavationTime = GameTool.LocalText("cultivation_plant58");
    private readonly string CultivatSittingRate = GameTool.LocalText("cultivation_plant59");

    private readonly string CultivatePlayerIncreasedInjury = GameTool.LocalText("cultivation_plant60");
    private readonly string CultivatePlayerAdrenaline = GameTool.LocalText("cultivation_plant61");
    private readonly string CultivatePlayerLifeResume = GameTool.LocalText("cultivation_plant62");
    private readonly string CultivatePlayerLucky = GameTool.LocalText("cultivation_plant63");
    private readonly string CultivatePlayerBotany = GameTool.LocalText("cultivation_plant64");
    private readonly string CultivatePlayerRange = GameTool.LocalText("cultivation_plant65");
    private readonly string CultivatePlayerDamage = GameTool.LocalText("cultivation_plant66");
    private readonly string CultivatePlayerAttackSpeed = GameTool.LocalText("cultivation_plant67");
    private readonly string CultivatePlayerSpeed = GameTool.LocalText("cultivation_plant68");
    private readonly string CultivatePlayerPower = GameTool.LocalText("cultivation_plant69");
    private readonly string CultivatePlayerMaxHp = GameTool.LocalText("cultivation_plant70");
    private readonly string CultivatePlayerCoinGold = GameTool.LocalText("cultivation_plant71");
    private readonly string CultivatePlayerArmor = GameTool.LocalText("cultivation_plant72");
    private readonly string CultivatePlayerSunshine = GameTool.LocalText("cultivation_plant73");

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
        this.InfoText.text = GameTool.LocalText(flowerPotGardenItem.PlantAttribute.plantCard.plantName);
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
