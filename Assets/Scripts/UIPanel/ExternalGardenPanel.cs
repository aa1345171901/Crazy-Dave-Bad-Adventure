using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class ExternalGardenPanel : BasePanel
{
    public Transform plantPosParent;
    public PlacePlantPosItem placePlantPosItem;
    public Text seedSum;
    public Transform seedContent;
    public ExternalGardenSeedItem externalGardenSeedItem;
    public Button btnShovel;
    public Image shovel;

    public Button btnMainmenu;

    public Text tips;

    public int selectSeed { get; private set; }
    public bool isShovel { get; private set; }
    public bool isItemDown { get; set; }

    ExternalGardenSeedItem selectSeedItem;

    public MainMenu mainMenu { get; set; }

    int seedSumCache;
    int placeNum;

    private void Start()
    {
        placeNum = 0;
        for (int i = 0; i < plantPosParent.childCount; i++)
        {
            var posT = plantPosParent.GetChild(i);
            var newPlaceItem = GameObject.Instantiate(placePlantPosItem, posT);
            newPlaceItem.gameObject.SetActive(true);
            newPlaceItem.transform.localPosition = new Vector3(0, 28f, 0);

            int pos = i + 1;
            var posList = SaveManager.Instance.externalGrowthData.plantPlace.Where((e) => e.key == pos);
            var data = posList.Count() == 0 ? null : posList.First();
            newPlaceItem.InitData(pos, data == null ? 0 : data.value, this);
            if (data != null)
                placeNum++;
        }
        UpdataUI();
        btnShovel.onClick.AddListener(OnShovelClick);
        btnMainmenu.onClick.AddListener(OnClose);
        UpdateCoolTips();
    }

    void UpdateCoolTips()
    {
        float loopTime = ConfManager.Instance.confMgr.gameIntParam.GetItemByKey("seedCardDefaultTime").value;
        loopTime -= placeNum * ConfManager.Instance.confMgr.gameIntParam.GetItemByKey("seedCardReduceTime").value;
        tips.text = string.Format(GameTool.LocalText("garden_cooltimeTips"), loopTime);
    }

    void OnClose()
    {
        UIManager.Instance.PopPanel();
        mainMenu.OnEnterMainMenu();
    }

    private void Update()
    {
        if ((Input.GetMouseButtonUp(0) && !isItemDown) || Input.GetMouseButtonDown(1))
        {
            if (isShovel)
            {
                isShovel = false;
                shovel.transform.localPosition = Vector3.zero;
                AudioManager.Instance.PlayEffectSoundByName("BtnGarden");
            }
        }
        if (Input.GetMouseButtonDown(1))
            selectSeed = 0;
        if (isShovel)
        {
            var mousePos = GameTool.GetMouseWorldPosi(UIManager.Instance.UICamera);
            shovel.transform.position = new Vector3(mousePos.x, mousePos.y, shovel.transform.position.z);
        }
    }

    void OnShovelClick()
    {
        IEnumerator DelaySet()
        {
            yield return new WaitForSeconds(0.1f);
            isShovel = true;
        }
        StartCoroutine(DelaySet());
        selectSeed = 0;
    }

    void UpdataUI()
    {
        seedSumCache = 0;
        seedContent.DestroyChild();
        foreach (var item in SaveManager.Instance.externalGrowthData.plantSeeds)
        {
            if (item.value != 0)
            {
                var seedItem = GameObject.Instantiate(externalGardenSeedItem, seedContent);
                seedItem.gameObject.SetActive(true);
                seedItem.InitData(item.key, SelectSeed);
                seedSumCache += item.value;
            }
        }
        GameObjectEx.UpdateLayout(btnShovel.transform.parent.gameObject);
        this.seedSum.text = seedSumCache.ToString();
    }

    void SelectSeed(int type, ExternalGardenSeedItem selectSeedItem)
    {
        int seedNum = SaveManager.Instance.externalGrowthData.GetPlantSeedCount(type);
        isShovel = false;
        if (seedNum <= 0)
        {
            selectSeed = 0;
            selectSeedItem = null;
            UpdataUI();
        }
        else
        {
            selectSeed = type;
            this.selectSeedItem = selectSeedItem;
            selectSeedItem.seedNum.text = "x" + seedNum;
        }
    }

    public void PlacePlantSeed(int pos)
    {
        SaveManager.Instance.externalGrowthData.PlacePlantSeed(pos, selectSeed);
        this.seedSum.text = (seedSumCache - 1).ToString();
        SelectSeed(selectSeed, selectSeedItem);
        placeNum++;
        UpdateCoolTips();
    }

    public void ShovelPlant(int pos)
    {
        SaveManager.Instance.externalGrowthData.ShovelPlantSeed(pos);
        isShovel = false;
        shovel.transform.localPosition = Vector3.zero;
        placeNum--;
        UpdateCoolTips();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        var animator = GetComponent<Animator>();
        animator.Play("show");
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
        SaveManager.Instance.SaveExternalGrowData();
    }
}
