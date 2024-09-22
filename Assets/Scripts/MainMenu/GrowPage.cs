using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowPage : MonoBehaviour
{
    public MainMenu mainMenu;

    public Button btnMenu;
    public Button btnReduction;
    public Text GrowText;
    public Transform growContent;
    public GrowItem growItem;
    public Text dialog;
    public Text levelMax;
    public Button btnGrow;
    public Text levelText;

    ConfExternlGrowItem nowSelect;
    GrowItem nowSelectGrow;

    private void Start()
    {
        btnGrow.transform.parent.gameObject.SetActive(false);
        dialog.gameObject.SetActive(false);
        btnMenu.onClick.AddListener(OnMainMenu);
        GrowText.text = SaveManager.Instance.externalGrowthData.headNum.ToString();
        btnGrow.onClick.AddListener(OnGrow);
        btnReduction.onClick.AddListener(OnReduction);
        UpdateUI();
    }

    private void OnEnable()
    {
        btnGrow.transform.parent.gameObject.SetActive(false);
        dialog.gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        growContent.transform.DestroyChild();
        foreach (var item in ConfManager.Instance.confMgr.externlGrow.items)
        {
            var newGrowItem = GameObject.Instantiate(growItem, growContent);
            newGrowItem.gameObject.SetActive(true);
            newGrowItem.InitData(item, OnSelect);
        }
    }

    void OnSelect(string key, GrowItem nowSelectGrow)
    {
        this.nowSelectGrow = nowSelectGrow;
        btnGrow.transform.parent.gameObject.SetActive(true);
        dialog.gameObject.SetActive(true);
        var confItem = ConfManager.Instance.confMgr.externlGrow.GetItemByKey(key);
        nowSelect = confItem;
        string desc = GameTool.LocalText(confItem.desc);
        var level = SaveManager.Instance.externalGrowthData.GetLevelByKey(key);
        bool isFull = level >= confItem.levelAdd.Length;
        int nextAdd = isFull ? 0 : confItem.levelAdd[level];
        int sum = SaveManager.Instance.externalGrowthData.GetGrowSumValueByKey(key);
        dialog.text = string.Format(desc, nextAdd, level, sum);
        btnGrow.gameObject.SetActive(!isFull);
        var cost = isFull ? 0 : nowSelect.cost[level];
        var headNum = SaveManager.Instance.externalGrowthData.headNum;
        bool canLevel = headNum >= cost;
        btnGrow.interactable = canLevel;
        levelText.text = string.Format("{0}</color>/{1}", canLevel ? "<color=#00ff00>" + headNum : "<color=#ff0000>" + headNum, cost);
        levelMax.gameObject.SetActive(isFull);
    }

    void OnGrow()
    {
        if (nowSelect == null)
            return;
        var level = SaveManager.Instance.externalGrowthData.GetLevelByKey(nowSelect.key);
        bool isFull = level >= nowSelect.levelAdd.Length;
        if (isFull)
            return;
        var cost = nowSelect.cost[level];
        if (SaveManager.Instance.externalGrowthData.headNum < cost)
            return;
        SaveManager.Instance.externalGrowthData.headNum -= cost;
        SaveManager.Instance.externalGrowthData.SetGrowLevel(nowSelect.key, level + 1);
        nowSelectGrow.UpdateLevel(level + 1, true);
        OnSelect(nowSelect.key, nowSelectGrow);
        UpdateHeadNum();
        SaveManager.Instance.SaveExternalGrowData();
    }

    private void UpdateHeadNum()
    {
        GrowText.text = SaveManager.Instance.externalGrowthData.headNum.ToString();
        var animator = GrowText.transform.parent.GetComponent<Animator>();
        animator.Play("GrowMoney", 0, 0);
    }

    void OnReduction()
    {
        IEnumerator DelayPlay()
        {
            yield return new WaitForSeconds(0.5f);
            SaveManager.Instance.externalGrowthData.Reduction();
            UpdateUI();
            UpdateHeadNum();
            btnGrow.transform.parent.gameObject.SetActive(false);
            dialog.gameObject.SetActive(false);
            SaveManager.Instance.SaveExternalGrowData();
        }
        StartCoroutine(DelayPlay());
    }

    void OnMainMenu()
    {
        this.gameObject.SetActive(false);
        mainMenu.animator.Play("Enter", 0, 0);
        var animator = mainMenu.btnGrow.GetComponent<Animator>();
        mainMenu.btnGrow.GetComponent<UIEventListener>().enabled = true;
        animator.Play("idel", 0, 0);
        mainMenu.Dave.AnimationState.SetAnimation(0, "MainMenuIdel", true);
        mainMenu.playAudio = true;
    }
}
