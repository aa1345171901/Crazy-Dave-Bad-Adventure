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
                info = "�������ֵ\nÿ����ʼʱ����ֵ��Ϊ" + colorStr + value + "</color>\n����ֵ��Ϊ0���ӻᱻ��ʬ�Ե�";
                break;
            case AttributeType.LifeRecovery:
                info = "�����ָ�\nÿ<color=#ff0000>10s</color>��ָ�" + colorStr + value + "</color>������ֵ\n�ָ����������ܵ����˺�ʱ�ҽ��޵�";
                break;
            case AttributeType.Adrenaline:
                int recoveryHp = value / 10;
                recoveryHp = recoveryHp == 0 ? 1 : recoveryHp;
                info = "��������\nÿ�ι�����������Ѫ����\nÿ�ι�����" + colorStr + value + "%���ʻָ�" + recoveryHp + "������ֵ</color>";
                break;
            case AttributeType.Power:
                info = "����\nƽ�׹���ɵĻ����˺�����" + colorStr + value + "</color>\n���˵���������<color=#00ff00>" + value + "%</color>";
                break;
            case AttributeType.PercentageDamage:
                info = "�˺�\nƽ�׹���ɵĻ����˺�����" + colorStr + value + "%</color>\nĳЩ���ߵ��˺�Ҳ����ٷֱ��˺�����";
                break;
            case AttributeType.AttackSpeed:
                info = "�����ٶ�\nƽ�׹��ƶ����ٶ�����" + colorStr + value + "%</color>\nĳЩ���ߵ�Ҳ���ܵ������ٶȵ�Ӱ�죬��ľ�";
                break;
            case AttributeType.Range:
                info = "������Χ\n�۾���������������Χ����" + colorStr + value + "%</color>\nĳЩ���ߵ�Ҳ���ܵ�������Χ��Ӱ�죬���ݳ�����װ";
                break;
            case AttributeType.CriticalHitRate:
                info = "������\n������" + colorStr + value + "%</color>���1.5���˺�";
                break;
            case AttributeType.Speed:
                info = "�ƶ��ٶ�\n������������" + colorStr + value + "%</color>\nĳЩ���ߵ�Ҳ���ܵ�������Χ��Ӱ�죬��С�Ƴ�\n<color=#00ff00>�ܲ����ٶ�Ϊ�����ٶȵ�1.5��</color>";
                break;
            case AttributeType.Armor:
                float finalArmor = GameManager.Instance.UserData.Armor / (50f + GameManager.Instance.UserData.Armor);
                finalArmor = finalArmor > 0.9f ? 0.9f : finalArmor;
                info = "����|<color=#ff0000>����90%</color>\n�ܵ����˺�����" + colorStr + (int)(finalArmor * 100) + "%</color>\n����Ч�赲��ʬ�Ե�����";
                break;
            case AttributeType.Lucky:
                info = "����\n�ܹ�����Ǯ�ҵ���ĸ��ʼ�����\n�ܹ������������������\n�ܹ�����߼���Ʒˢ�¸���\n��ǳ���Ҫ";
                break;
            case AttributeType.Sunshine:
                info = "����\nÿ������������ѻ��" + colorStr + value + "</color>����";
                break;
            case AttributeType.GoldCoins:
                info = "���\nÿ������������ѻ��" + colorStr + value + "</color>���";
                break;
            case AttributeType.Botany:
                info = "ֲ��ѧ\n����ʹֲ����ɵ��˺�����" + colorStr + value * 2 + "%</color>";
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
