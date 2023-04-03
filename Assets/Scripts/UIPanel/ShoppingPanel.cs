using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingPanel : BasePanel
{
    public Transform PropContent;
    public Transform PlantContent;

    public GameObject PropItemPrefab;
    public GameObject PlantItemPrefab;

    public UIButton BtnRenovate;
    public Text RenovateMoneyText;

    public Text DialogText;
    public Text Money;

    [Tooltip("花盆")]
    public FlowerPotShopItem FlowerPotItem;

    public AudioSource audioSource;

    private Animator animator;

    private List<ShopItem> shopItems = new List<ShopItem>();
    private List<PropCardItem> propCardItems = new List<PropCardItem>();
    private List<PlantCardItem> plantCardItems = new List<PlantCardItem>();
    private AttributePanel attributePanel;
    private int renovateMoney;
    private int RenovateMoney
    {
        get
        {
            return renovateMoney;
        }
        set
        {
            renovateMoney = value;
            if (ShopManager.Instance.Money > RenovateMoney)
                RenovateMoneyText.text = "刷新\n-" + renovateMoney.ToString();
            else
                RenovateMoneyText.text = "刷新\n" + "<color=#ff0000>-" + renovateMoney.ToString() + "</color>";
        }
    }
    private int renovateCount = 0;
    private int autoRefreshWave = -1;

    private readonly int ItemsCount = 4;

    private readonly string[] CannotAffordStrings = new string[] 
    {
        "她很好，我不配，应为我没有钱钱",
        "越没钱的时候越想花钱，穷到要吃土l",
        "购物车里的东西只能看着下架"
    };
    private readonly string[] CannotPlantingStrings = new string[]
    {
        "没有花盆植物不能种植",
        "植物只能在<color=#ff0000>花盆</color>上培养,花盆可以通过<color=#ff0000>刷新</color>进行获取",
        "花盆肥肠重要！！"
    };
    private bool isTriggerCannotAfford;

    private void Start()
    {
        this.DialogText.transform.parent.gameObject.SetActive(false);
        ShopManager.Instance.MoneyChanged += MoneyChanged;
        BtnRenovate.gameObject.SetActive(false);
        InitItems();
        AudioManager.Instance.AudioLists.Add(audioSource);
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        FlowerPotItem.CannotAfford += ()=> 
        {
            this.DialogText.text = FlowerPotItem.infoNoPurchase;
        };
    }

    private void MoneyChanged()
    {
        foreach (var item in shopItems)
        {
            item.UpdateMoney();
        }
        Money.text = ShopManager.Instance.Money.ToString();
        RenovateMoney = RenovateMoney;
    }

    private void Update()
    {
        if (shopItems == null || shopItems.Count <= 0)
            return;
        if (FlowerPotItem.isDown)
        {
            this.DialogText.transform.parent.gameObject.SetActive(true);
            this.DialogText.text = FlowerPotItem.Info;
            return;
        }
        else
        {
            FlowerPotItem.SetDefualtInfo();
        }
        this.DialogText.transform.parent.gameObject.SetActive(false);
        bool hasDown = false;
        foreach (var item in shopItems)
        {
            if (item.isDown)
            {
                this.DialogText.transform.parent.gameObject.SetActive(true);
                item.SetInfo();
                if (!isTriggerCannotAfford)
                    this.DialogText.text = item.Info;
                hasDown = true;
                break;
            }
        }
        if (!hasDown)
            isTriggerCannotAfford = false;
    }

    private void InitItems()
    {
        for (int i = 0; i < ItemsCount; i++)
        {
            PropCardItem propItem = GameObject.Instantiate(PropItemPrefab, PropContent).GetComponent<PropCardItem>();
            propItem.CannotAfford += CannotAfford;
            propItem.gameObject.SetActive(false);
            propCardItems.Add(propItem);

            PlantCardItem plant = GameObject.Instantiate(PlantItemPrefab, PlantContent).GetComponent<PlantCardItem>();
            plant.CannotAfford += CannotAfford;
            plant.CanNotPlanting += CanNotPlanting;
            plant.gameObject.SetActive(false);
            plantCardItems.Add(plant);
        }
        shopItems.AddRange(propCardItems);
        shopItems.AddRange(plantCardItems);
    }

    private void CannotAfford()
    {
        int index = UnityEngine.Random.Range(0, CannotAffordStrings.Length);
        this.DialogText.text = CannotAffordStrings[index];
        isTriggerCannotAfford = true;
    }

    private void CanNotPlanting()
    {
        int index = UnityEngine.Random.Range(0, CannotPlantingStrings.Length);
        this.DialogText.text = CannotPlantingStrings[index];
        isTriggerCannotAfford = true;
    }

    public void Refresh()
    {
        if (ShopManager.Instance.Money < RenovateMoney)
            return;

        audioSource.Play();
        animator.SetTrigger("refresh");

        float lucky = GameManager.Instance.UserData.Lucky;
        /*
         * 出现白色品质的概率为100-蓝紫红  
         * 蓝色概率为 幸运40前 （20+幸运) 40-50(固定60) 50以上为（100-白色（10） - 紫色-红色）   
         * 紫色概率为幸运/2%  
         * 红色概率为幸运/10%   
         * 白色最低10 蓝色封顶60最低30  紫色概率封顶40， 红色概率封顶20
         */
        // 都乘10换算成从1000中取，增加精度,后面的区间需加前面的区间
        float redProbability = 10 * lucky / 10;
        if (redProbability > 200)
            redProbability = 200;
        float purpleProbability = 10 * lucky / 2 + redProbability;
        if (purpleProbability > 400)
            purpleProbability = 400;
        float blueProbability = 10 * (20 + lucky);
        if (lucky > 40 && lucky < 50)
            blueProbability = 10 * 60;
        if (lucky >= 50)
            blueProbability = 10 * (100 - 10 - purpleProbability - redProbability);
        blueProbability += purpleProbability;

        // 从1000中选4个数，决定4个道具的品质
        HashSet<int> hashSet = RandomUtils.RandomCreateNumber(1000, 4);
        // 按品质分类
        Dictionary<int, int> propDicts = new Dictionary<int, int>();
        foreach (var item in hashSet)
        {
            int quality = 1;
            if (item <= redProbability)
                quality = 4;
            else if (item <= purpleProbability)
                quality = 3;
            else if (item <= blueProbability)
                quality = 2;
            if (!propDicts.ContainsKey(quality))
                propDicts[quality] = 0;
            propDicts[quality]++;
        }
        // 获取不同的道具，使同品质道具不相同
        int index = 0;
        foreach (var item in propDicts)
        {
            var dicts = ShopManager.Instance.PropDicts;
            HashSet<int> propHashSet = RandomUtils.RandomCreateNumber(dicts[item.Key].Count, item.Value);
            foreach (var hash in propHashSet)
            {
                propCardItems[index].gameObject.SetActive(true);
                propCardItems[index].SetProp(dicts[item.Key][hash]);
                propCardItems[index].attributePanel = this.attributePanel;
                index++;
            }
        }

        // 植物 todo
        var plantCards = ShopManager.Instance.shopLists.PlantCards;
        hashSet = RandomUtils.RandomCreateNumber(plantCards.Count, 4);
        index = 0;
        foreach (var item in hashSet)
        {
            plantCardItems[index].gameObject.SetActive(true);
            plantCardItems[index].SetPlant(plantCards[item]);
            index++;
        }
        if (renovateCount != 0)
        {
            // 发钱刷新可点击获取花盆
            ShopManager.Instance.Money -= RenovateMoney;
            FlowerPotItem.SetActive();
        }
        RenovateMoney += (autoRefreshWave + renovateCount) * 2 + 1;
        renovateCount++;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        if (autoRefreshWave != LevelManager.Instance.IndexWave)
        {
            animator = GetComponent<Animator>();
            animator.SetTrigger("enter");
            autoRefreshWave = LevelManager.Instance.IndexWave;
            Invoke("OpenAttributePanel", 1);
        }
        else
        {
            // 从花园进入
            attributePanel = UIManager.Instance.PushPanel(UIPanelType.AttributePanel) as AttributePanel;
        }
    }

    private void OpenAttributePanel()
    {
        if (this.gameObject.activeSelf)
        {
            BtnRenovate.gameObject.SetActive(true);
            attributePanel = UIManager.Instance.PushPanel(UIPanelType.AttributePanel) as AttributePanel;
            Money.text = ShopManager.Instance.Money.ToString();
            renovateCount = 0;
            Refresh();
            RenovateMoney = 2 + autoRefreshWave * 2;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        this.gameObject.SetActive(false);
    }

    public void GoGarden()
    {
        UIManager.Instance.PopPanel();
        UIManager.Instance.PushPanel(UIPanelType.GardenPanel);
        AudioManager.Instance.PlayGardenMusic();
    }
}
