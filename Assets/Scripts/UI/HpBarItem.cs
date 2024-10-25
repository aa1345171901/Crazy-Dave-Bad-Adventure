using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarItem : MonoBehaviour
{
    public Slider sliderHp;
    public Slider sliderAnimFade;
    public Text textName;

    float timer;
    float nowValue;

    readonly float animTime = 0.5f;

    public void InitData(string name, int maxHp)
    {
        textName.text = name;
        sliderHp.maxValue = maxHp;
        sliderAnimFade.maxValue = maxHp;
        sliderHp.value = maxHp;
        sliderAnimFade.value = maxHp;
    }

    public void SetHp(int value, int maxHp)
    {
        if (sliderHp.maxValue != maxHp)
        {
            sliderHp.maxValue = maxHp;
            sliderAnimFade.maxValue = maxHp;
        }
        sliderHp.value = value;
        nowValue = sliderAnimFade.value;
        timer = Time.time;
    }

    private void Update()
    {
        if (Time.time - timer < animTime)
        {
            float process = (Time.time - timer) / animTime;
            sliderAnimFade.value = Mathf.Lerp(nowValue, sliderHp.value, process);
        }
    }
}
