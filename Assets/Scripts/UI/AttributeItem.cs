using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class AttributeItem : MonoBehaviour
{
    public Text AttributeText;
    public Text Value;

    public AttributeType AttributeType { get; set; }
    public AttributePanel AttributePanel { get; set; }

    private int value;
    private string info;

    private void Start()
    {
        if (AttributeText == null)
            AttributeText = this.transform.Find("Text").GetComponent<Text>();
        if (Value == null)
            Value = this.transform.Find("ValueText").GetComponent<Text>();
        SetValue(value);
    }

    public void SetValue(int value)
    {
        this.value = value;
        if (Value == null)
            return;
        Value.text = value.ToString();
        if (value < 0)
        {
            Value.color = AttributeText.color = Color.red;
        }
        else if (value > 0)
        {
            Value.color = AttributeText.color = Color.green;
        }
        else
        {
            Value.color = AttributeText.color = Color.white;
        }
        string colorStr;
        if (value >= 0)
            colorStr = "<color=#00ff00>";
        else
            colorStr = "<color=#ff0000>";
        switch (AttributeType)
        {
            case AttributeType.MaximumHP:
                info = string.Format(GameTool.LocalText("property_MaximumHP_info"), colorStr + value);
                break;
            case AttributeType.LifeRecovery:
                info = string.Format(GameTool.LocalText("property_LifeRecovery_info"), colorStr + value);
                break;
            case AttributeType.Adrenaline:
                int recoveryHp = value / 10;
                recoveryHp = recoveryHp == 0 ? 1 : recoveryHp;
                info = string.Format(GameTool.LocalText("property_Adrenaline_info"), colorStr + value, recoveryHp);
                break;
            case AttributeType.Power:
                info = string.Format(GameTool.LocalText("property_Power_info"), colorStr + value, value);
                break;
            case AttributeType.PercentageDamage:
                info = string.Format(GameTool.LocalText("property_PercentageDamage_info"), colorStr + value);
                break;
            case AttributeType.AttackSpeed:
                info = string.Format(GameTool.LocalText("property_AttackSpeed_info"), colorStr + value);
                break;
            case AttributeType.Range:
                info = string.Format(GameTool.LocalText("property_Range_info"), colorStr + value);
                break;
            case AttributeType.CriticalHitRate:
                info = string.Format(GameTool.LocalText("property_CriticalHitRate_info"), colorStr + value);
                break;
            case AttributeType.CriticalDamage:
                info = string.Format(GameTool.LocalText("property_CriticalDamage_info"), colorStr + (150 + value));
                break;
            case AttributeType.Speed:
                info = string.Format(GameTool.LocalText("property_MoveSpeed_info"), colorStr + value);
                break;
            case AttributeType.Armor:
                float finalArmor = GameManager.Instance.UserData.Armor / (50f + GameManager.Instance.UserData.Armor);
                finalArmor = finalArmor > 0.9f ? 0.9f : finalArmor;
                info = string.Format(GameTool.LocalText("property_Armor_info"), colorStr + (int)(finalArmor * 100));
                break;
            case AttributeType.Lucky:
                info = GameTool.LocalText("property_Lucky_info");
                break;
            case AttributeType.Sunshine:
                info = string.Format(GameTool.LocalText("property_Sunshine_info"), colorStr + value);
                break;
            case AttributeType.GoldCoins:
                info = string.Format(GameTool.LocalText("property_GoldCoins_info"), colorStr + value);
                break;
            case AttributeType.Botany:
                info = string.Format(GameTool.LocalText("property_Botany_info"), colorStr + value * 2);
                break;
            default:
                break;
        }
    }

    public void SetInit(AttributePanel attributePanel, AttributeType attributeType)
    {
        this.AttributePanel = attributePanel;
        this.AttributeType = attributeType;
    }

    private void OnMouseEnter()
    {
        AudioManager.Instance.PlayEffectSoundByName("shopItem");
        AttributePanel.Info = info;
    }

    private void OnMouseExit()
    {
        AttributePanel.Info = null;
    }
}
