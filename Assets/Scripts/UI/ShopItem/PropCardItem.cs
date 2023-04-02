using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum PropDamageType
{
    None,
    LawnMower,  // С�Ƴ�
    Fire,  // ����
    Hammer,
    VocalConcert,
}

[Serializable]
public class PropCard : ISerializationCallbackReceiver
{
    public string propName;
    public string propImagePath;
    public int defaultPrice;
    public int defaultSun;
    public string info;
    public int quality;
    public List<AttributeIncrement> attributes;

    // ����Ϊ��ѡ
    public string preconditions;  // ������ǰ�������ĳɾ�����
    public PropDamageType propDamageType;
    [HideInInspector]
    public string propDamageTypeString = "None";
    public int defalutDamage;
    public float coolingTime;

    public void OnAfterDeserialize()
    {
        PropDamageType type = (PropDamageType)System.Enum.Parse(typeof(PropDamageType), propDamageTypeString);
        propDamageType = type;
    }

    public void OnBeforeSerialize()
    {

    }
}

[Serializable]
public class AttributeIncrement : ISerializationCallbackReceiver
{
    public AttributeType attributeType;
    [HideInInspector]
    public string attributeTypeString;
    public int increment;

    public void OnAfterDeserialize()
    {
        AttributeType type = (AttributeType)System.Enum.Parse(typeof(AttributeType), attributeTypeString);
        attributeType = type;
    }

    public void OnBeforeSerialize()
    {

    }
}

public class PropCardItem : ShopItem
{
    [Header("��������")]
    public string propName;
    public Image Prop;

    public List<AttributeIncrement> AttributeDicts; // ���ӵ����Լ�

    public AttributePanel attributePanel;

    private PropCard propCard;

    public void SetProp(PropCard propCard)
    {
        this.propCard = propCard;
        this.propName = propCard.propName;

        Sprite image = Resources.Load<Sprite>(propCard.propImagePath);
        this.Prop.sprite = image;

        // ��Ʒ�۸�����   ÿ��������߼۸������ʣ� ��ɫΪ ���� / 3  ��ɫΪ ���� / 5  ��ɫΪ ���� / 6  ��ɫΪ ���� / 8
        float wave = LevelManager.Instance.IndexWave;
        switch (propCard.quality)
        {
            case 1:
                this.Price = (int)(propCard.defaultPrice * (wave / 3 < 1 ? 1 : wave / 3));
                break;
            case 2:
                this.Price = (int)(propCard.defaultPrice * (wave / 5 < 1 ? 1 : wave / 5));
                break;
            case 3:
                this.Price = (int)(propCard.defaultPrice * (wave / 6 < 1 ? 1 : wave / 6));
                break;
            case 4:
                this.Price = (int)(propCard.defaultPrice * (wave / 8 < 1 ? 1 : wave / 8));
                break;
            default:
                this.Price = propCard.defaultPrice;
                break;
        }
        this.PriceText.text = this.Price.ToString();
        this.Info = propCard.info;
        SetInfo();

        this.AttributeDicts = propCard.attributes;

        UpdateMoney();
    }

    public override void SetInfo()
    {
        if (propCard.propDamageType != PropDamageType.None)
        {
            var userData = GameManager.Instance.UserData;
            int finalDamage;
            float finalAttackCoolingTime;
            switch (propCard.propDamageType)
            {
                case PropDamageType.LawnMower:
                    finalDamage = Mathf.RoundToInt((userData.Power + propCard.defalutDamage) * (100f + userData.PercentageDamage) / 100);
                    finalAttackCoolingTime = propCard.coolingTime * (1 - userData.Speed / (100f + userData.Speed));
                    finalAttackCoolingTime.ToString("F2");
                    this.Info = string.Format(this.propCard.info,
                        "<color=#ff0000>" + finalDamage + "</color><color=#9932CD>��(100%����+" + propCard.defalutDamage + ") * �˺���</color>",
                        "<color=#ff0000>" + finalAttackCoolingTime + "</color><color=#9932CD>��" + propCard.coolingTime + "*��1-�ٶ�/���ٶ�+100����</color>");
                    break;
                case PropDamageType.Fire:
                    this.Info = string.Format(this.propCard.info, propCard.defalutDamage);
                    break;
                case PropDamageType.Hammer:
                    finalDamage = Mathf.RoundToInt((userData.Power * 1.5f + propCard.defalutDamage) * (100f + userData.PercentageDamage) / 100);
                    finalAttackCoolingTime = propCard.coolingTime - userData.AttackSpeed / 100f;
                    finalAttackCoolingTime.ToString("F2");
                    finalAttackCoolingTime = finalAttackCoolingTime < 0.5f ? 0.5f : finalAttackCoolingTime;
                    this.Info = string.Format(this.propCard.info,
                        "<color=#ff0000>" + finalDamage + "</color><color=#9932CD>��(150%����+" + propCard.defalutDamage + ") * �˺���</color>",
                        "<color=#ff0000>" + finalAttackCoolingTime + "</color><color=#9932CD>��" + propCard.coolingTime + "-�����ٶ�%��</color>");
                    break;
                case PropDamageType.VocalConcert:
                    break;
                default:
                    break;
            }
        }
    }

    protected override void OnClick()
    {
        base.OnClick();
        if (ShopManager.Instance.Money >= Price)
        {
            ShopManager.Instance.PurchaseProp(propCard, Price);
            var userData = GameManager.Instance.UserData;
            foreach (var item in AttributeDicts)
            {
                var fieldInfo = typeof(UserData).GetField(Enum.GetName(typeof(AttributeType), item.attributeType));
                fieldInfo.SetValue(userData, (int)fieldInfo.GetValue(userData) + item.increment);
                attributePanel.SetAttribute(item.attributeType, (int)fieldInfo.GetValue(userData));
            }
        }
    }
}
