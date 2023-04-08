using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    public Text InfoText;
    public GameObject content;
    public PropBagItem propBagItem;

    private PropCard nowShowPropCard;
    public PropCard NowShowPropCard 
    { 
        get 
        {
            return NowShowPropCard;
        } 
        set 
        {
            if (nowShowPropCard != value)
            {
                nowShowPropCard = value;
                if (nowShowPropCard == null)
                {
                    InfoText.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    InfoText.transform.parent.gameObject.SetActive(true);
                    InfoText.text = nowShowPropCard.info;
                    if (nowShowPropCard.propDamageType != PropDamageType.None)
                    {
                        var userData = GameManager.Instance.UserData;
                        int finalDamage;
                        float finalAttackCoolingTime;
                        switch (nowShowPropCard.propDamageType)
                        {
                            case PropDamageType.LawnMower:
                                finalDamage = Mathf.RoundToInt((userData.Power + nowShowPropCard.defalutDamage) * (100f + userData.PercentageDamage) / 100);
                                finalAttackCoolingTime = nowShowPropCard.coolingTime * (1 - userData.Speed / (100f + userData.Speed));
                                finalAttackCoolingTime.ToString("F2");
                                InfoText.text = string.Format(this.nowShowPropCard.info,
                                    "<color=#ff0000>" + finalDamage + "</color><color=#9932CD>（(100%力量+" + nowShowPropCard.defalutDamage + ") * 伤害）</color>",
                                    "<color=#ff0000>" + finalAttackCoolingTime + "</color><color=#9932CD>（" + nowShowPropCard.coolingTime + "*（1-速度/（速度+100））</color>");
                                break;
                            case PropDamageType.Fire:
                                InfoText.text = string.Format(this.nowShowPropCard.info, nowShowPropCard.defalutDamage);
                                break;
                            case PropDamageType.Hammer:
                                finalDamage = Mathf.RoundToInt((userData.Power * 1.5f + nowShowPropCard.defalutDamage) * (100f + userData.PercentageDamage) / 100);
                                finalAttackCoolingTime = nowShowPropCard.coolingTime - userData.AttackSpeed / 100f;
                                finalAttackCoolingTime.ToString("F2");
                                finalAttackCoolingTime = finalAttackCoolingTime < 0.5f ? 0.5f : finalAttackCoolingTime;
                                InfoText.text = string.Format(this.nowShowPropCard.info,
                                    "<color=#ff0000>" + finalDamage + "</color><color=#9932CD>（(150%力量+" + nowShowPropCard.defalutDamage + ") * 伤害）</color>",
                                    "<color=#ff0000>" + finalAttackCoolingTime + "</color><color=#9932CD>（" + nowShowPropCard.coolingTime + "-攻击速度%）</color>");
                                break;
                            case PropDamageType.VocalConcert:
                                finalDamage = Mathf.RoundToInt(10 * 5 * (100f + userData.PercentageDamage) / 100);
                                float range = 2 * (100 + userData.Range) / 100f;
                                InfoText.text += "\n音乐会套装每秒对范围<color=#ff0000>" + range + "</color>造成<color=#ff0000>" + finalDamage + "</color>伤害";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        } 
    }

    private Dictionary<PropCard, PropBagItem> propDicts = new Dictionary<PropCard, PropBagItem>();
    private Camera UICamera;
    private RectTransform rectTransform;

    private void Start()
    {
        UICamera = UIManager.Instance.UICamera;
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 判断鼠标是否在按钮范围内
        if (!BoundsUtils.GetSceneRect(UICamera, rectTransform).Contains(Input.mousePosition))
        {
            UIManager.Instance.PopPanel();
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        // 先将已有的道具数目清零
        foreach (var item in propDicts)
        {
            item.Value.Count = 0;
        }
        
        // 再增加数目或生成预制体
        var purchasedProps = ShopManager.Instance.PurchasedProps;
        foreach (var item in purchasedProps)
        {
            if (propDicts.ContainsKey(item))
            {
                propDicts[item].Count++;
            }
            else
            {
                PropBagItem bagItem = GameObject.Instantiate(propBagItem, content.transform);
                bagItem.SetPropCard(item);
                bagItem.Count = 1;
                bagItem.BagPanel = this;
                propDicts.Add(item, bagItem) ;
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }
}
