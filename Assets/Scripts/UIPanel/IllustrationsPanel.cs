using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class IllustrationsPanel : BasePanel
{
    public Transform plantRoot;
    public Button btnLookPlant;
    public Transform zombieRoot;
    public Button btnLookZombie;
    public Button btnClose;
    public Button btnReturn;

    public GameObject plantPage;
    public Transform plantPage_plantRoot;
    public Text plantInfo;
    public Text plantName;
    public Text plantCostMoney;
    public Text plantCostSun;
    public Transform plantContent;
    public PlantIllustrationsItem plantIllustrationsItem;

    public GameObject zombiePage;
    public Transform zombieNormalRoot;
    public Transform zombieBossRoot;
    public Text zombieInfo;
    public Text zombieName;
    public Transform zombieContent;
    public ZombieIllustrationsItem zombieIllustrationsItem;
    public ZombieIllustrationsItem zombieBoss;

    private Animator animator;

    public MainMenu mainMenu { get; set; }

    ConfPlantIllustrationsItem selectPlant;
    ConfZombieIllustrationsItem selectZombie;

    private void Start()
    {
        btnClose.onClick.AddListener(OnClose);
        btnReturn.onClick.AddListener(OnReturn);
        btnLookPlant.onClick.AddListener(OnLookPlant);
        btnLookZombie.onClick.AddListener(OnLookZombie);
    }

    void OnClose()
    {
        UIManager.Instance.PopPanel();
        mainMenu.OnEnterMainMenu();
        OnReturn();
    }

    void OnReturn()
    {
        plantPage.SetActive(false);
        zombiePage.SetActive(false);
        btnReturn.gameObject.SetActive(false);
    }

    void OnLookPlant()
    {
        plantPage.SetActive(true);
        btnReturn.gameObject.SetActive(true);
        if (plantContent.transform.childCount < ConfManager.Instance.confMgr.plantIllustrations.items.Count)
        {
            foreach (var item in ConfManager.Instance.confMgr.plantIllustrations.items)
            {
                if (selectPlant == null && SaveManager.Instance.externalGrowthData.GetPlantCount(item.plantType) > 0)
                {
                    selectPlant = item;
                    OnPlantSelect(selectPlant);
                }
                var plantGo = GameObject.Instantiate(plantIllustrationsItem, plantContent);
                plantGo.gameObject.SetActive(true);
                plantGo.InitData(item, OnPlantSelect);
            }
        }
        if (selectPlant == null)
            OnPlantSelect(ConfManager.Instance.confMgr.plantIllustrations.items.First());
    }

    void OnPlantSelect(ConfPlantIllustrationsItem confItem)
    {
        selectPlant = confItem;
        var confCardItem = ConfManager.Instance.confMgr.plantCards.GetPlantCardByType(confItem.plantType);
        if (SaveManager.Instance.externalGrowthData.GetPlantCount(confItem.plantType) > 0)
        {
            plantName.text = GameTool.LocalText(confCardItem.plantName);
            plantInfo.text = GameTool.LocalText(confItem.info);
            plantCostMoney.text = GameTool.LocalText("tujian_huafei") + confCardItem.defaultPrice;
            plantCostSun.text = GameTool.LocalText("tujian_xiaohao") + confCardItem.defaultSun;
        }
        else
        {
            plantName.text = "? ? ?";
            plantInfo.text = "? ? ?\r\n" + GameTool.LocalText("tujian_plant");
            plantCostMoney.text = GameTool.LocalText("tujian_huafei") + "? ?";
            plantCostSun.text = GameTool.LocalText("tujian_xiaohao") + "? ?";
        }
        plantPage_plantRoot.DestroyChild();
        var plantGo = Resources.Load(confItem.prefabPath);
        GameObject.Instantiate(plantGo, plantPage_plantRoot);
    }

    void OnLookZombie()
    {
        zombiePage.SetActive(true);
        btnReturn.gameObject.SetActive(true);
        if (zombieContent.transform.childCount < ConfManager.Instance.confMgr.zombieIllustrations.items.Count)
        {
            foreach (var item in ConfManager.Instance.confMgr.zombieIllustrations.items)
            {
                if (selectZombie == null && SaveManager.Instance.externalGrowthData.GetZombieCount(item.zombieType) > 0)
                {
                    selectZombie = item;
                    OnZombieSelect(selectZombie);
                }
                if (item.zombieType == (int)ZombieType.Boss)
                {
                    zombieBoss.InitData(item, OnZombieSelect);
                }
                else
                {
                    var zombieGo = GameObject.Instantiate(zombieIllustrationsItem, zombieContent);
                    zombieGo.gameObject.SetActive(true);
                    zombieGo.InitData(item, OnZombieSelect);
                }
            }
        }
        if (selectZombie == null)
            OnZombieSelect(ConfManager.Instance.confMgr.zombieIllustrations.items.First());
    }

    void OnZombieSelect(ConfZombieIllustrationsItem confItem)
    {
        selectZombie = confItem;
        zombieBossRoot.DestroyChild();
        zombieNormalRoot.DestroyChild();

        if (SaveManager.Instance.externalGrowthData.GetZombieCount(confItem.zombieType) > 0)
        {
            zombieInfo.text = GameTool.LocalText(confItem.info);
            zombieName.text = GameTool.LocalText(confItem.zombieName);
            if (SaveManager.Instance.systemData.language == "cn")
                zombieInfo.resizeTextForBestFit = (confItem.zombieType == (int)ZombieType.Boss || confItem.zombieType == (int)ZombieType.Gargantuan);

            var zombieGo = Resources.Load(confItem.prefabPath);
            if (confItem.zombieType == (int)ZombieType.Boss)
                GameObject.Instantiate(zombieGo, zombieBossRoot);
            else
                GameObject.Instantiate(zombieGo, zombieNormalRoot);
        }
        else
        {
            zombieInfo.text = "? ? ?\r\n" + GameTool.LocalText("tujian_zombie");
            zombieName.text = "? ? ?";
        }    
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        animator = GetComponent<Animator>();
        animator.Play("show");

        UpdateUI();
    }

    void UpdateUI()
    {
        CreatePlant();
        CreateZombie();
        plantPage.SetActive(false);
        zombiePage.SetActive(false);
        plantInfo.resizeTextForBestFit = SaveManager.Instance.systemData.language != "cn";
        zombieInfo.resizeTextForBestFit = SaveManager.Instance.systemData.language != "cn";
        zombieInfo.resizeTextMinSize = SaveManager.Instance.systemData.language != "cn" ? 15 : 20;
    }

    void CreatePlant()
    {
        plantRoot.DestroyChild();
        var index = Random.Range(0, ConfManager.Instance.confMgr.plantIllustrations.items.Count);
        var confPlant = ConfManager.Instance.confMgr.plantIllustrations.items[index];
        var plantGo = Resources.Load(confPlant.prefabPath);
        GameObject.Instantiate(plantGo, plantRoot);
    }

    void CreateZombie()
    {
        zombieRoot.DestroyChild();
        var index = Random.Range(0, ConfManager.Instance.confMgr.zombieIllustrations.items.Count);
        var confZombie = ConfManager.Instance.confMgr.zombieIllustrations.items[index];
        var zombieGo = Resources.Load(confZombie.prefabPath);
        GameObject.Instantiate(zombieGo, zombieRoot);
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }
}
