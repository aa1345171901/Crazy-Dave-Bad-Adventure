using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AchievementItem : MonoBehaviour
{
    public Image img;
    public Image board;
    public Text title;
    public Text info;
    public Text lockText;

    ConfAchievementItem confItem;

    public void InitData(ConfAchievementItem confItem)
    {
        this.confItem = confItem;
        ConfManager.Instance.languageChange += ChangeText;
        Sprite sprite = Resources.Load<Sprite>(confItem.imgPath);
        img.sprite = sprite;
        if (confItem.process != 0 && confItem.process != 1)
        {
            title.text += $"(0/{confItem.process})";
        }
        ChangeText();

        bool isLock = confItem.id % 2 == 1;
        float alpha = isLock ? 1.0f : 0.5f;
        img.color = new Color(1, 1, 1, alpha);
        board.color = new Color(1, 1, 1, alpha);
    }

    void ChangeText()
    {
        title.text = GameTool.LocalText(confItem.title);
        info.text = GameTool.LocalText(confItem.desc);
        lockText.text = GameTool.LocalText(confItem.lockText);
    }
}
