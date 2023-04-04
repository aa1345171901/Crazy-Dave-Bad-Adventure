using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : BasePanel
{
    [Tooltip("血量条")]
    public HPBar HPBar;
    [Tooltip("进度条")]
    public Slider ProgressBar;
    [Tooltip("奔跑条")]
    public Slider RunBar;
    [Tooltip("钱")]
    public Text MoneyText;
    [Tooltip("阳光")]
    public Text SunText;
    [Tooltip("获取钱动画和音效")]
    public FinishGetGold getGold;

    private void Start()
    {
        ShopManager.Instance.MoneyChanged += () =>
        {
            MoneyText.text = ShopManager.Instance.Money.ToString();
        };
        GardenManager.Instance.SunChanged += () =>
        {
            SunText.text = GardenManager.Instance.Sun.ToString();
        };
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }

    public void SetHPBar(int hp, int maxHp)
    {
        HPBar.SetHPBar(hp, maxHp);
    }

    public void SetProgressSlider(float value)
    {
        ProgressBar.value = value;
    }

    public void SetRunSlider(float value)
    {
        RunBar.value = value;
    }

    public void GetGold()
    {
        getGold.GetGold();
    }
}
