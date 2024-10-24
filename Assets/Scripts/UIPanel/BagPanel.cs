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
    public GameObject sellPage;
    public Button sellOne;
    public Button sellAll;
    public Text sellOneText;
    public Text sellAllText;

    public bool AutoClose { get; set; }

    public ShoppingPanel ShoppingPanel { get; set; }

    public PropCard sellProp { get; set; }

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
                    ShoppingPanel?.SetSoundsItem(false);
                }
                else
                {
                    ShoppingPanel?.SetSoundsItem(true, nowShowPropCard);
                    InfoText.transform.parent.gameObject.SetActive(true);
                    InfoText.text = GameTool.LocalText(nowShowPropCard.info);
                    var textConf = ConfManager.Instance;
                    if (nowShowPropCard.propType != PropType.None)
                    {
                        var userData = GameManager.Instance.UserData;
                        int finalDamage;
                        int finalCount;
                        float finalAttackCoolingTime;
                        switch (nowShowPropCard.propType)
                        {
                            case PropType.LawnMower:
                                finalDamage = Mathf.RoundToInt((userData.Power + nowShowPropCard.value1) * (100f + userData.PercentageDamage) / 100);
                                finalAttackCoolingTime = nowShowPropCard.coolingTime * (1 - userData.Speed / (100f + userData.Speed));
                                finalAttackCoolingTime.ToString("F2");
                                InfoText.text = string.Format(GameTool.LocalText(this.nowShowPropCard.info),
                                    "<color=#ff0000>" + finalDamage + $"</color><color=#9932CD>（(100%{textConf.Power}+" + nowShowPropCard.value1 + $") * {textConf.PercentageDamage}）</color>",
                                    "<color=#ff0000>" + finalAttackCoolingTime + "</color><color=#9932CD>（" + nowShowPropCard.coolingTime + $"*（1-{textConf.Speed}/（{textConf.Speed}+100））</color>");
                                break;
                            case PropType.Fire:
                                InfoText.text = string.Format(GameTool.LocalText(this.nowShowPropCard.info), nowShowPropCard.value1);
                                break;
                            case PropType.Hammer:
                                finalDamage = Mathf.RoundToInt((userData.Power * 1.5f + nowShowPropCard.value1) * (100f + userData.PercentageDamage) / 100);
                                finalAttackCoolingTime = nowShowPropCard.coolingTime - userData.AttackSpeed / 100f;
                                finalAttackCoolingTime.ToString("F2");
                                finalAttackCoolingTime = finalAttackCoolingTime < 0.5f ? 0.5f : finalAttackCoolingTime;
                                InfoText.text = string.Format(GameTool.LocalText(this.nowShowPropCard.info),
                                    "<color=#ff0000>" + finalDamage + $"</color><color=#9932CD>（(150%{textConf.Power}+" + nowShowPropCard.value1 + $") * {textConf.PercentageDamage}）</color>",
                                    "<color=#ff0000>" + finalAttackCoolingTime + "</color><color=#9932CD>（" + nowShowPropCard.coolingTime + $"-{textConf.AttackSpeed}）</color>");
                                break;
                            case PropType.VocalConcert:
                                finalDamage = Mathf.RoundToInt(10 * 5 * (100f + userData.PercentageDamage) / 100);
                                float range = 2 * (100 + userData.Range) / 100f;
                                InfoText.text += "\n" + string.Format(GameTool.LocalText("battle_vocalconcert"), range, finalDamage);
                                break;
                            case PropType.SmellyFart:
                                finalDamage = Mathf.RoundToInt((userData.Botany / 5f + nowShowPropCard.value1) * (100f + userData.PercentageDamage) / 100);
                                InfoText.text = string.Format(GameTool.LocalText(nowShowPropCard.info), finalDamage);
                                break;
                            case PropType.FireElf:
                                if (nowShowPropCard.propName == "fireElf")
                                {
                                    finalDamage = Mathf.RoundToInt((userData.CriticalHitRate * 2 + nowShowPropCard.value1) * (100f + userData.CriticalDamage) / 100) * 2;
                                    finalCount = 1 + Mathf.RoundToInt(userData.CriticalDamage / 50);
                                    InfoText.text = string.Format(GameTool.LocalText(this.nowShowPropCard.info), finalCount, finalDamage);
                                }
                                else
                                {
                                    finalDamage = Mathf.RoundToInt((userData.CriticalHitRate + nowShowPropCard.value1) * (100f + userData.CriticalDamage) / 100);
                                    InfoText.text = string.Format(GameTool.LocalText(this.nowShowPropCard.info), finalDamage);
                                }
                                break;
                            case PropType.WaterElf:
                                if (nowShowPropCard.propName == "waterElf")
                                {
                                    finalDamage = Mathf.RoundToInt((userData.MaximumHP / 6 + userData.LifeRecovery / 5 + nowShowPropCard.value1));
                                    finalCount = 1 + Mathf.RoundToInt(userData.MaximumHP / 100);
                                    InfoText.text = string.Format(GameTool.LocalText(this.nowShowPropCard.info), finalCount, finalDamage);
                                }
                                else
                                {
                                    finalDamage = Mathf.RoundToInt((userData.MaximumHP / 3 + userData.LifeRecovery / 2.5f + nowShowPropCard.value1));
                                    InfoText.text = string.Format(GameTool.LocalText(this.nowShowPropCard.info), finalDamage);
                                }
                                break;
                            case PropType.DarkCloud:
                                finalDamage = Mathf.RoundToInt((userData.Adrenaline / 5f + nowShowPropCard.value1) * (100f + userData.CriticalDamage) / 100);
                                InfoText.text = string.Format(GameTool.LocalText(this.nowShowPropCard.info), finalDamage);
                                break;
                            case PropType.Gun:
                                if (nowShowPropCard.propName == "pistol")
                                {
                                    finalDamage = Mathf.RoundToInt((userData.Power / 2f + nowShowPropCard.value1) * (100f + userData.PercentageDamage) / 100);
                                    InfoText.text = string.Format(GameTool.LocalText(this.nowShowPropCard.info), finalDamage);
                                }
                                else
                                {
                                    finalDamage = Mathf.RoundToInt((userData.Power + nowShowPropCard.value1) * (100f + userData.PercentageDamage) / 100);
                                    InfoText.text = string.Format(GameTool.LocalText(this.nowShowPropCard.info), finalDamage);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        } 
    }

    private Dictionary<string, PropBagItem> propDicts = new Dictionary<string, PropBagItem>();
    private Camera UICamera;
    private RectTransform rectTransform;
    private RectTransform rectSellPage;

    private void Start()
    {
        UICamera = UIManager.Instance.UICamera;
        rectTransform = GetComponent<RectTransform>();
        rectSellPage = sellPage.GetComponent<RectTransform>();
        sellAll.onClick.AddListener(() =>{ ShopManager.Instance.SellProp(sellProp, true); UpdateUI(); });
        sellOne.onClick.AddListener(() => { ShopManager.Instance.SellProp(sellProp, false); UpdateUI(); });
    }

    private void Update()
    {
        // 判断鼠标是否在按钮范围内
        if (AutoClose && !BoundsUtils.GetSceneRect(UICamera, rectTransform, rectTransform.sizeDelta.x).Contains(Input.mousePosition))
        {
            UIManager.Instance.PopPanel();
        }
        if (Input.GetMouseButtonUp(0) && AutoClose && !BoundsUtils.GetSceneRect(UICamera, rectSellPage).Contains(Input.mousePosition))
        {
            sellPage.SetActive(false);
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        this.transform.SetSiblingIndex(this.transform.parent.childCount - 1);  // 设置最后一个渲染
        UpdateUI();
    }

    void UpdateUI()
    {
        sellPage.SetActive(false);
        // 先将已有的道具数目清零
        foreach (var item in propDicts)
        {
            item.Value.Count = 0;
            item.Value.gameObject.SetActive(false);
        }

        // 再增加数目或生成预制体
        var purchasedProps = ShopManager.Instance.PurchasedProps;
        foreach (var item in purchasedProps)
        {
            if (propDicts.ContainsKey(item.propName))
            {
                propDicts[item.propName].Count++;
                propDicts[item.propName].gameObject.SetActive(true);
            }
            else
            {
                PropBagItem bagItem = GameObject.Instantiate(propBagItem, content.transform);
                bagItem.SetPropCard(item);
                bagItem.Count = 1;
                bagItem.BagPanel = this;
                propDicts.Add(item.propName, bagItem);
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }
}
