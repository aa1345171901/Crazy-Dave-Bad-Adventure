using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class GardenPanel : BasePanel
{
    public PlantConent plantConent;

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        // 创建的下一帧才调用玩start
        Invoke("CreateFlowerPat", Time.deltaTime);
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        base.OnResume();
        this.gameObject.SetActive(true);
        CreateFlowerPat();
    }

    public override void OnPause()
    {
        base.OnPause();
        this.gameObject.SetActive(false);
    }

    public void GoShopping()
    {
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
    }
}
