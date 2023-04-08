using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class AttributePanel : BasePanel
{
    public Dictionary<AttributeType, AttributeItem> AttributeDicts;

    public Transform AttributeContent;
    public Text InfoText;

    public ShoppingPanel ShoppingPanel { get; set; }

    private string info;
    public string Info
    {
        get
        {
            return info;
        }
        set
        {
            if (info != value)
            {
                info = value;
                if (string.IsNullOrEmpty(info))
                {
                    InfoText.transform.parent.gameObject.SetActive(false);
                    ShoppingPanel?.SetSoundsItem(false);
                }
                else
                {
                    ShoppingPanel?.SetSoundsItem(true, info);
                    InfoText.transform.parent.gameObject.SetActive(true);
                    InfoText.text = info;
                }
            }
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        this.transform.SetSiblingIndex(this.transform.parent.childCount - 1);  // 设置最后一个渲染
        SetAttributes();
    }

    public void SetAttribute(AttributeType attributeType, int value)
    {
        AttributeDicts[attributeType].SetValue(value);
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
        UIManager.Instance.PopPanel();
    }

    private void Start()
    {
        AttributeDicts = new Dictionary<AttributeType, AttributeItem>();
        var childs = AttributeContent.GetComponentsInChildren<AttributeItem>();
        for (int i = 0; i < childs.Length; i++)
        {
            AttributeDicts.Add(Enum.Parse<AttributeType>(i.ToString()), childs[i]);
        }
        SetAttributes();
    }

    private void SetAttributes()
    {
        if (AttributeDicts == null || AttributeDicts.Count <= 0)
            return;
        var userData = GameManager.Instance.UserData;

        foreach (var item in AttributeDicts)
        {
            SetAttribute(item.Key, (int)typeof(UserData).GetField(Enum.GetName(typeof(AttributeType), item.Key)).GetValue(userData));
            item.Value.SetInit(this, item.Key);
        }
    }
}
