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
                    SetItemInfo(flowerPotGardenItem, PlantType.Repeater, new string[] {CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage });
                    break;
                case PlantType.Repeater:
                    SetItemInfo(flowerPotGardenItem, PlantType.GatlingPea, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage });
                    break;
                case PlantType.GatlingPea:
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateBasicDamage, CultivatePercentageDamage, CultivateRange, CultivateCoolTime, CultivateBulletSpeed, CultivateSplashDamage });
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
                    SetItemInfo(flowerPotGardenItem, PlantType.None, new string[] { CultivateDigestiveSpeed, CultivateRange, CultivateSwallowCount, CultivateCoinConversionRate, CultivateSunConversionRate, CultivateBasicDamage, CultivatePercentageDamage });
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

    private void SetItemInfo(FlowerPotGardenItem flowerPotGardenItem, PlantType plantType, string[] infos)
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
    }

    public void UpdateSunPrice()
    {
        foreach (var item in plantCultivationItems)
        {
            item.UpdateSunPrice();
        }
    }
}
