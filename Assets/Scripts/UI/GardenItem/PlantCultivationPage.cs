using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class PlantCultivationPage : MonoBehaviour
{
    public List<PlantCultivationItem> plantCultivationItems;
    public Text InfoText;
    [Tooltip("������ť")]
    public EvolutionItem EvolutionItem;
    [Tooltip("��ս��ť")]
    public GoToWarItem GoToWarItem;
    [Tooltip("�԰�ť")]
    public EatItem EatItem;

    public PlantConent PlantConent { get; set; }

    /// <summary>
    /// ��Ҫ����ҳ����ʾ����ȷ
    /// </summary>
    public FlowerPotGardenItem FlowerPotGardenItem { get; private set; }

    private Camera UICamera;
    private RectTransform rectTransform;

    private readonly string CultivateInfo = "����";
    private readonly string CultivateBasicDamage = "�����˺�";
    private readonly string CultivatePercentageDamage = "�ٷֱ��˺�";
    private readonly string CultivateRange = "������ⷶΧ";
    private readonly string CultivateCoolTime = "������ȴ";
    private readonly string CultivateBulletSpeed = "�ӵ��ٶ�";
    private readonly string CultivateSplashDamage = "�����˺�";
    private readonly string CultivateDigestiveSpeed = "�����ٶ�";
    private readonly string CultivateSwallowCount = "һ�������ɸ���";
    private readonly string CultivateCoinConversionRate = "�����ת��";
    private readonly string CultivateSunConversionRate = "������ת��";
    private readonly string CultivatePenetrationCount = "��͸����";
    private readonly string CultivateCriticalDamage = "�����˺�";
    private readonly string CultivateAttackSpeed = "��ҹ����ٶ�";
    private readonly string CultivateWindSpeed = "����";
    private readonly string CultivateWindResume = "���ָ�";
    private readonly string CultivateWindage = "��ʬ����";
    private readonly string CultivateExplosionRange = "��ը��Χ";
    private readonly string CultivateImmediateMortalityRate = "��ͨ��ʬ������";
    private readonly string CultivateIncreasedInjury = "���ͽ�ʬ����";
    private readonly string CultivateSunReduced = "�������";
    private readonly string CultivateButterProbability = "���͸���";
    private readonly string CultivateControlTime = "����ʱ��";
    private readonly string CultivateCriticalHitRate = "������";
    private readonly string CultivateDamageDoubleRate = "�˺�����*2����";
    private readonly string CultivateCoinGoldCount = "��ȡ��Ҹ���";
    private readonly string CultivateDuration = "��ȡ����ʱ��";
    private readonly string CultivateZombieReducedRate = "����Ĺ������";
    private readonly string CultivateAttackCount = "�Ȼ��߿ɹ�������";
    private readonly string CultivateOvereatingRate = "������������";
    private readonly string CultivateChangeCoin = "��ȡ���";
    private readonly string CultivateCutterCount = "��ȡ����Ʒ����";
    private readonly string CultivateGoldCoinRate = "�����Ҹ���";
    private readonly string CultivateDiamondRate = "������ʯ����";
    private readonly string CultivateTwinRate = "����˫������";
    private readonly string CultivateRangeAttackSpeed = "��Χ��������";
    private readonly string CultivateRangeLifeResume = "��Χ�����ָ�";
    private readonly string CultivateRangeDamage = "��Χ�˺�����";
    private readonly string CultivateAddBulletRate = "�ӵ����ʱ��";
    private readonly string CultivateBulletSize = "�ӵ���С";
    private readonly string CultivateReStartResumeLife = "����������ֵ�ָ�����";
    private readonly string CultivateAttackPumpkinRate = "���������Ϲϸ���";
    private readonly string CultivateDecelerationPercentage = "���ٰٷֱ�";
    private readonly string CultivateDecelerationTime = "����ʱ��";
    private readonly string CultivatDestroyingVehiclesCount = "�ƻ��ؾ�����";
    private readonly string CultivatSunQuality = "������������";
    private readonly string CultivatTallNutHp = "�߼������ֵ��ֲ��ѧ��";
    private readonly string CultivatBoomRate = "���ձ�ը����";
    private readonly string CultivatCounterInjury = "�����˺�";
    private readonly string CultivatCounterInjuryRate = "���˸���";
    private readonly string CultivatPeaDamage = "�㶹����";
    private readonly string CultivatPeaSplash = "�㶹�����˺�";
    private readonly string CultivatPeaSpeed = "�㶹�ٶ�";
    private readonly string CultivatRollSpeed = "�����ٶ�";
    private readonly string CultivatBoomNutRate = "��ը�������";
    private readonly string CultivatFrostTime = "����ʱ��";
    private readonly string CultivatFrostAttackSpeedAdd = "�����ڼ乥������";
    private readonly string CultivatExcavationTime = "����ʱ��";
    private readonly string CultivatSittingRate = "��������";

    private readonly string CultivatePlayerIncreasedInjury = "��ҶԽ�ʬ����";
    private readonly string CultivatePlayerAdrenaline = "��������";
    private readonly string CultivatePlayerLifeResume = "�����ָ�";
    private readonly string CultivatePlayerLucky = "����";
    private readonly string CultivatePlayerBotany = "ֲ��ѧ";
    private readonly string CultivatePlayerRange = "��Χ";
    private readonly string CultivatePlayerDamage = "�˺�";
    private readonly string CultivatePlayerAttackSpeed = "�����ٶ�";
    private readonly string CultivatePlayerSpeed = "�ٶ�";
    private readonly string CultivatePlayerPower = "����";
    private readonly string CultivatePlayerMaxHp = "�������ֵ";
    private readonly string CultivatePlayerCoinGold = "���";
    private readonly string CultivatePlayerArmor = "����";
    private readonly string CultivatePlayerSunshine = "����";

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
        // ��δ��������
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
            // ����ֲ���������������ʾ�Լ�������ťչʾ
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
            // 100 Ϊcanvasÿ��λ����
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
