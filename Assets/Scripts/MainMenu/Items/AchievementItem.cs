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

        ChangeText();

        bool isLock = AchievementManager.Instance.GetReach(confItem.achievementId);
        float alpha = isLock ? 1.0f : 0.5f;
        img.color = new Color(1, 1, 1, alpha);
        board.color = new Color(1, 1, 1, alpha);
    }

    void ChangeText()
    {
        title.text = GameTool.LocalText(confItem.title);
        int nowProcess = AchievementManager.Instance.GetProcess(confItem.achievementId);
        bool isLock = AchievementManager.Instance.GetReach(confItem.achievementId);
        if (!isLock && confItem.process != 0 && confItem.process != 1)
        {
            title.text += $"  ({nowProcess}/{confItem.process})";
        }
        info.text = GameTool.LocalText(confItem.desc);
        lockText.text = GameTool.LocalText(confItem.lockText);
    }
}
