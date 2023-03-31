using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class AttributePanel : BasePanel
{
    public Dictionary<AttributeType, AttributeItem> AttributeDicts;

    public Transform AttributeContent;

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
        UIManager.Instance.PopPanel();  // 不是单个出现的Panel，出栈时使前一个也出
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
        }
    }
}
