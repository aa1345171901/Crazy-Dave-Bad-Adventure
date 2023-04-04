using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : BasePanel
{
    [Tooltip("Ѫ����")]
    public HPBar HPBar;
    [Tooltip("������")]
    public Slider ProgressBar;
    [Tooltip("������")]
    public Slider RunBar;
    [Tooltip("Ǯ")]
    public Text MoneyText;
    [Tooltip("����")]
    public Text SunText;
    [Tooltip("��ȡǮ��������Ч")]
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
