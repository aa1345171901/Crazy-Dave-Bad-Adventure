using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageShowItem : MonoBehaviour
{
    public Image bg;
    public Text title;
    public Text value;
    public Slider slider;

    public void InitData(bool isTaking, int type, int value, int maxValue)
    {
        string path = isTaking ? "TakingDamageStatistics" : "DamageStatistics";
        path += type;
        Sprite sprite = Resources.Load<Sprite>("UI/DamageStatistics/" + path);
        bg.sprite = sprite;

        title.text = GameTool.LocalText(path);

        this.value.text = value + "/" + maxValue;

        slider.maxValue = maxValue;
        slider.value = value;
    }

    public void UpdateUI(int value, int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = value;
    }
}
