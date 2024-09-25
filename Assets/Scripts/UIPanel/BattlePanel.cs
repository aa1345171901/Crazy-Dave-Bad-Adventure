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
    [Tooltip("冲刺冷却条")]
    public Slider DashBar;

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

    public void SetDashSlider(int count, float value, int remainCount)
    {
        if (count == 1)
        {
            DashBar.value = value;
        }
        else
        {
            if (DashBar.transform.parent.childCount < count)
            {
                for (int i = count - DashBar.transform.parent.childCount; i < count; i++)
                {
                    var dashSlider = GameObject.Instantiate(DashBar, DashBar.transform.parent);
                }
                return;
            }
            for (int i = 0; i < DashBar.transform.parent.childCount; i++)
            {
                var dashSlider = DashBar.transform.parent.GetChild(i).GetComponent<Slider>();
                dashSlider.value = i < remainCount ? 1 : 0;
                dashSlider.handleRect.gameObject.SetActive(false);
            }
            if (remainCount != count)
            {
                var dashSlider = DashBar.transform.parent.GetChild(remainCount).GetComponent<Slider>();
                dashSlider.value = value;
                dashSlider.handleRect.gameObject.SetActive(true);
            }
            else
            {
                var dashSlider = DashBar.transform.parent.GetChild(DashBar.transform.parent.childCount - 1).GetComponent<Slider>();
                dashSlider.handleRect.gameObject.SetActive(true);
            }
        }
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
