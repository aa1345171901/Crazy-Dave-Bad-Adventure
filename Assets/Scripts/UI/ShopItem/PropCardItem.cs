using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum PropType
{
    None,
    /// <summary>
    /// 小推车
    /// </summary>
    LawnMower,
    /// <summary>
    /// 火焰
    /// </summary>
    Fire,  // 火焰
    /// <summary>
    /// 木槌
    /// </summary>
    Hammer,
    /// <summary>
    /// 演唱会
    /// </summary>
    VocalConcert,
    /// <summary>
    /// 僵尸数量改变
    /// </summary>
    ZombieChange,
    /// <summary>
    /// 大蒜、臭屁
    /// </summary>
    SmellyFart,
    /// <summary>
    /// 火精灵
    /// </summary>
    FireElf,
    /// <summary>
    /// 水精灵
    /// </summary>
    WaterElf,
    /// <summary>
    /// 乌云
    /// </summary>
    DarkCloud,
}

[Serializable]
public class PropCard
{
    public string propName;
    public string propImagePath;
    public int defaultPrice;
    public string info;
    public int quality;
    public List<AttributeIncrement> attributes;

    // 以下为可选
    public string preconditions;  // 解锁的前置条件的成就名称
    public PropType propType;
    /// <summary>
    /// 伤害以及增减幅数量
    /// </summary>
    public int value1;
    public float coolingTime;

    public int GetNowPrice()
    {
        int price = defaultPrice;
        int wave = LevelManager.Instance.IndexWave;
        // 商品价格设置   每波过后道具价格膨胀率， 白色为 波数 / 3  蓝色为 波数 / 5  紫色为 波数 / 6  红色为 波数 / 8
        switch (quality)
        {
            case 1:
                price = (int)(defaultPrice * (wave / 3 < 1 ? 1 : wave / 3));
                break;
            case 2:
                price = (int)(defaultPrice * (wave / 5 < 1 ? 1 : wave / 5));
                break;
            case 3:
                price = (int)(defaultPrice * (wave / 6 < 1 ? 1 : wave / 6));
                break;
            case 4:
                price = (int)(defaultPrice * (wave / 8 < 1 ? 1 : wave / 8));
                break;
            default:
                price = defaultPrice;
                break;
        }
        return price;
    }
}

[Serializable]
public class AttributeIncrement
{
    public AttributeType attributeType;
    public int increment;
}

public class PropCardItem : ShopItem
{
    [Header("道具特有")]
    public string propName;
    public Image Prop;

    public List<AttributeIncrement> AttributeDicts; // 增加的属性集

    public AttributePanel attributePanel;

    public Action CanNotPurchaseWaterPot;

    public Transform effectRoot;

    private PropCard propCard;

    public void SetProp(PropCard propCard)
    {
        this.propCard = propCard;
        this.propName = propCard.propName;

        Sprite image = Resources.Load<Sprite>(propCard.propImagePath);
        this.Prop.sprite = image;

        this.Price = propCard.GetNowPrice();
        this.PriceText.text = this.Price.ToString();
        this.Info = GameTool.LocalText(propCard.info);
        SetInfo();

        this.AttributeDicts = propCard.attributes;

        SetQualityEffect();

        UpdateMoney();
    }

    void SetQualityEffect()
    {
        effectRoot.DestroyChild();
        var effect = Resources.Load("Prefabs/Effects/UI/guangzhu_" + propCard.quality);
        if (effect != null)
            GameObject.Instantiate(effect, effectRoot);
    }

    public override void SetInfo()
    {
        if (propCard.propType != PropType.None)
        {
            var userData = GameManager.Instance.UserData;
            int finalDamage;
            int finalCount;
            float finalAttackCoolingTime;
            var textConf = ConfManager.Instance;
            switch (propCard.propType)
            {
                case PropType.LawnMower:
                    finalDamage = Mathf.RoundToInt((userData.Power + propCard.value1) * (100f + userData.PercentageDamage) / 100);
                    finalAttackCoolingTime = propCard.coolingTime * (1 - userData.Speed / (100f + userData.Speed));
                    this.Info = string.Format(GameTool.LocalText(this.propCard.info),
                        "<color=#ff0000>" + finalDamage + $"</color><color=#9932CD>（(100%{textConf.Power}+" + propCard.value1 + $") * {textConf.PercentageDamage}）</color>",
                        "<color=#ff0000>" + (int)finalAttackCoolingTime + "</color><color=#9932CD>（" + propCard.coolingTime + "*（1-速度/（速度+100））</color>");
                    break;
                case PropType.Fire:
                    this.Info = string.Format(GameTool.LocalText(this.propCard.info), propCard.value1);
                    break;
                case PropType.Hammer:
                    finalDamage = Mathf.RoundToInt((userData.Power * 1.5f + propCard.value1) * (100f + userData.PercentageDamage) / 100);
                    finalAttackCoolingTime = propCard.coolingTime - userData.AttackSpeed / 100f;
                    finalAttackCoolingTime = finalAttackCoolingTime < 0.5f ? 0.5f : finalAttackCoolingTime;
                    this.Info = string.Format(GameTool.LocalText(this.propCard.info),
                        "<color=#ff0000>" + finalDamage + $"</color><color=#9932CD>（(150%{textConf.Power}+" + propCard.value1 + $") * {textConf.PercentageDamage}）</color>",
                        "<color=#ff0000>" + (int)finalAttackCoolingTime + "</color><color=#9932CD>（" + propCard.coolingTime + $"-{textConf.AttackSpeed}）</color>");
                    break;
                case PropType.VocalConcert:
                    break;
                case PropType.SmellyFart:
                    finalDamage = Mathf.RoundToInt((userData.Botany / 5f + propCard.value1) * (100f + userData.PercentageDamage) / 100);
                    this.Info = string.Format(GameTool.LocalText(this.propCard.info), finalDamage);
                    break;
                case PropType.FireElf:
                    if (propCard.propName == "fireElf")
                    {
                        finalDamage = Mathf.RoundToInt((userData.CriticalHitRate * 2 + propCard.value1) * (100f + userData.CriticalDamage) / 100) * 2;
                        finalCount = 1 + Mathf.RoundToInt(userData.CriticalDamage / 50);
                        this.Info = string.Format(GameTool.LocalText(this.propCard.info), finalCount, finalDamage);
                    }
                    else
                    {
                        finalDamage = Mathf.RoundToInt((userData.CriticalHitRate + propCard.value1) * (100f + userData.CriticalDamage) / 100);
                        this.Info = string.Format(GameTool.LocalText(this.propCard.info), finalDamage);
                    }
                    break;
                case PropType.WaterElf:
                    if (propCard.propName == "waterElf")
                    {
                        finalDamage = Mathf.RoundToInt((userData.MaximumHP / 6 + userData.LifeRecovery / 5 + propCard.value1));
                        finalCount = 1 + Mathf.RoundToInt(userData.MaximumHP / 100);
                        this.Info = string.Format(GameTool.LocalText(this.propCard.info), finalCount, finalDamage);
                    }
                    else
                    {
                        finalDamage = Mathf.RoundToInt((userData.MaximumHP / 3 + userData.LifeRecovery / 2.5f + propCard.value1));
                        this.Info = string.Format(GameTool.LocalText(this.propCard.info), finalDamage);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    protected override void OnClick()
    {
        if (ShopManager.Instance.Money >= Price)
        {
            if (propCard.propName == "Pot_Water")
            {
                // 如果购买的道具是水花盆，则需要判断花园中是否能够摆放
                if (GardenManager.Instance.AllFlowerPotCount < GardenManager.Instance.MaxFlowerPotCount)
                {
                    Shop();
                }
                else
                    CanNotPurchaseWaterPot?.Invoke();
            }
            else
                Shop();
        }
        else
        {
            CannotAfford?.Invoke();
        }
    }

    private void Shop()
    {
        ShopManager.Instance.PurchaseProp(propCard, Price, attributePanel.SetAttribute);
        this.isDown = false;
        this.gameObject.SetActive(false);
    }
}
