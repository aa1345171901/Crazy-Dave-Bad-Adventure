using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OtherGameModesItem : MonoBehaviour
{
    public Image img;
    public GameObject have;
    public Text nameText;

    ConfOtherGameModesItem confItem;

    public void InitData(ConfOtherGameModesItem confItem)
    {
        this.confItem = confItem;

        Sprite bg = Resources.Load<Sprite>(confItem.imgPath);
        img.sprite = bg;

        nameText.text = GameTool.LocalText(confItem.name);
    }
}
