using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class GardenPanel : BasePanel
{
    public PlantConent plantConent;
    public PlantCardPage PlantCardPage;


    public Text SunText;

    public Text NowWave;

    private void Start()
    {
        GardenManager.Instance.SunChanged += () =>
        {
            SunText.text = GardenManager.Instance.Sun.ToString();
        };
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        // ��������һ֡�ŵ���start
        Invoke("CreateFlowerPat", Time.deltaTime);
        SunText.text = GardenManager.Instance.Sun.ToString();
        NowWave.text = "��ǰΪ��<color=#ffff00>" + (LevelManager.Instance.IndexWave + 1) + "</color>��";
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }

    public void GoShopping()
    {
        UIManager.Instance.PopPanel();
        UIManager.Instance.PushPanel(UIPanelType.ShopingPanel);
        AudioManager.Instance.PlayShoppingMusic(0);
    }

    public void NextWave()
    {
        UIManager.Instance.PopPanel();
        GameManager.Instance.NextWave();
    }

    public void CreateFlowerPat()
    {
        plantConent.CreateFlowerPot();
        PlantCardPage.CreateCard();
    }
}
