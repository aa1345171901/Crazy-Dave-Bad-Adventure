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
                info = "最大生命值\n每波开始时生命值将为" + colorStr + value + "</color>\n生命值变为0脑子会被僵尸吃掉";
                break;
            case AttributeType.LifeRecovery:
                info = "生命恢复\n每<color=#ff0000>10s</color>会恢复" + colorStr + value + "</color>点生命值\n恢复能力高于受到的伤害时我将无敌";
                break;
            case AttributeType.Adrenaline:
                int recoveryHp = value / 10;
                recoveryHp = recoveryHp == 0 ? 1 : recoveryHp;
                info = "肾上腺素\n每次攻击都让我热血澎湃\n每次攻击有" + colorStr + value + "%概率恢复" + recoveryHp + "点生命值</color>";
                break;
            case AttributeType.Power:
                info = "力量\n平底锅造成的基础伤害增加" + colorStr + value + "</color>\n击退的力量增加<color=#00ff00>" + value + "%</color>";
                break;
            case AttributeType.PercentageDamage:
                info = "伤害\n平底锅造成的基础伤害增加" + colorStr + value + "%</color>\n某些道具的伤害也会随百分比伤害增加";
                break;
            case AttributeType.AttackSpeed:
                info = "攻击速度\n平底锅移动的速度增加" + colorStr + value + "%</color>\n某些道具的也会受到攻击速度的影响，如木槌";
                break;
            case AttributeType.Range:
                info = "攻击范围\n眼睛更明亮，攻击范围增加" + colorStr + value + "%</color>\n某些道具的也会受到攻击范围的影响，如演唱会套装";
                break;
            case AttributeType.CriticalHitRate:
                info = "暴击率\n攻击有" + colorStr + value + "%</color>造成1.5倍伤害";
                break;
            case AttributeType.Speed:
                info = "移动速度\n基础移速增加" + colorStr + value + "%</color>\n某些道具的也会受到攻击范围的影响，如小推车\n<color=#00ff00>跑步的速度为基础速度的1.5倍</color>";
                break;
            case AttributeType.Armor:
                float finalArmor = GameManager.Instance.UserData.Armor / (50f + GameManager.Instance.UserData.Armor);
                finalArmor = finalArmor > 0.9f ? 0.9f : finalArmor;
                info = "护甲|<color=#ff0000>上限90%</color>\n受到的伤害减少" + colorStr + (int)(finalArmor * 100) + "%</color>\n能有效阻挡僵尸吃掉脑子";
                break;
            case AttributeType.Lucky:
                info = "幸运\n能够增加钱币掉落的概率及质量\n能够增加阳光掉落最大个数\n能够增大高级物品刷新概率\n这非常重要";
                break;
            case AttributeType.Sunshine:
                info = "阳光\n每波结束可以免费获得" + colorStr + value + "</color>阳光";
                break;
            case AttributeType.GoldCoins:
                info = "金币\n每波结束可以免费获得" + colorStr + value + "</color>金币";
                break;
            case AttributeType.Botany:
                info = "植物学\n可以使植物造成的伤害增加" + colorStr + value * 2 + "%</color>";
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
