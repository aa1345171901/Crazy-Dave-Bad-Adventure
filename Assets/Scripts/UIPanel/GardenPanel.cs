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

    public Transform nextZombieRoot;
    public ZombieIllustrationsItem zombieItem;
    public GameObject bossItem;

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
        // 创建的下一帧才调用start
        Invoke("CreateFlowerPat", Time.deltaTime);
        SunText.text = GardenManager.Instance.Sun.ToString();
        NowWave.text = string.Format(GameTool.LocalText("garden_wave"), (LevelManager.Instance.IndexWave + 1));
        CreateZombieType();
    }

    public void CreateZombieType()
    {
        nextZombieRoot.DestroyChild();
        if (ConfManager.Instance.confMgr.wave.waves.ContainsKey(LevelManager.Instance.IndexWave + 2))
        {
            foreach (var item in ConfManager.Instance.confMgr.wave.waves[LevelManager.Instance.IndexWave + 2])
            {
                if (item.zombieType == (int)ZombieType.Boss)
                {
                    var bossItemGo = GameObject.Instantiate(bossItem, nextZombieRoot);
                    bossItemGo.SetActive(true);
                }
                else
                {
                    var zombieItemGo = GameObject.Instantiate(zombieItem, nextZombieRoot);
                    zombieItemGo.gameObject.SetActive(true);
                    var confItem = ConfManager.Instance.confMgr.zombieIllustrations.GetItemByType(item.zombieType);
                    zombieItemGo.InitData(confItem, null);
                }
            }
        }
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
