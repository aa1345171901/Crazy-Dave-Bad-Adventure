using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
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
    [Tooltip("植物卡片页面")]
    public PlantCardPage plantCardPage;
    [Tooltip("击杀头")]
    public Text GrowText;

    float defualtRunWidth;

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
        MoneyText.text = ShopManager.Instance.Money.ToString();
        SunText.text = GardenManager.Instance.Sun.ToString();
        GrowText.text = GameManager.Instance.HeadNum.ToString();
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        base.OnResume();
        plantCardPage.CreateCard();
        MoneyText.text = ShopManager.Instance.Money.ToString();
        SunText.text = GardenManager.Instance.Sun.ToString();
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

    public void SetRunSliderWidth(float value)
    {
        if (defualtRunWidth == 0)
            defualtRunWidth = RunBar.transform.GetComponent<RectTransform>().sizeDelta.x;
        RunBar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(defualtRunWidth * value, RunBar.transform.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void GetGold()
    {
        getGold.GetGold();
    }

    public void SetGrowText()
    {
        GrowText.text = GameManager.Instance.HeadNum.ToString();
        var animator = GrowText.transform.parent.GetComponent<Animator>();
        animator.Play("GrowMoney", 0, 0);
    }

    public void UpdatePlantPage()
    {
        plantCardPage.SetCard();
    }

    public void Pause()
    {
        GameManager.Instance.Pause();
    }
}
