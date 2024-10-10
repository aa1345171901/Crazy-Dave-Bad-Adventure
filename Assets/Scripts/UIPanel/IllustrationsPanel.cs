using System.Collections;
using System.Collections.Generic;
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

    private Animator animator;

    public MainMenu mainMenu { get; set; }

    ConfPlantIllustrationsItem selectPlant;

    private void Start()
    {
        btnClose.onClick.AddListener(OnClose);
        btnReturn.onClick.AddListener(OnReturn);
        btnLookPlant.onClick.AddListener(OnLookPlant);
    }

    void OnClose()
    {
        UIManager.Instance.PopPanel();
        mainMenu.OnEnterMainMenu();
    }

    void OnReturn()
    {
        plantPage.SetActive(false);
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
                if (selectPlant == null)
                {
                    selectPlant = item;
                    OnPlantSelect(selectPlant);
                }
                var plantGo = GameObject.Instantiate(plantIllustrationsItem, plantContent);
                plantGo.gameObject.SetActive(true);
                plantGo.InitData(item, OnPlantSelect);
            }
        }
    }

    void OnPlantSelect(ConfPlantIllustrationsItem confItem)
    {
        selectPlant = confItem;
        var confCardItem = ConfManager.Instance.confMgr.plantCards.GetPlantCardByType(confItem.plantType);
        plantCostMoney.text = GameTool.LocalText("tujian_huafei") + confCardItem.defaultPrice;
        plantCostSun.text = GameTool.LocalText("tujian_xiaohao") + confCardItem.defaultSun;
        plantInfo.text = GameTool.LocalText(confItem.info);
        plantName.text = GameTool.LocalText(confCardItem.plantName);
        plantPage_plantRoot.DestroyChild();
        var plantGo = Resources.Load(confItem.prefabPath);
        GameObject.Instantiate(plantGo, plantPage_plantRoot);
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
        plantInfo.resizeTextForBestFit = SaveManager.Instance.systemData.language != "cn";
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
