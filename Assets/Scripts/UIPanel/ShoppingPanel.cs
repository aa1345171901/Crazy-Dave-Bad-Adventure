using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShoppingPanel : BasePanel,EventListener<PropPurchaseEvent>
{
    public Transform PropContent;
    public Transform PlantContent;

    public GameObject PropItemPrefab;
    public GameObject PlantItemPrefab;

    public UIButton BtnRenovate;
    public Text RenovateMoneyText;
    public AudioClip renovateSounds;

    public Text DialogText;
    public Text Money;

    [Tooltip("花盆")]
    public FlowerPotShopItem FlowerPotItem;

    public AudioSource audioSource;

    public SkeletonGraphic Dave;
    public List<AudioClip> daveAudioClips;
    private bool hasOtherDown;    // 鼠标是否在物品和属性上
    private object nowItem;        // 当前播放语音的item,后续道具的展示也放shopping这里，所以使用object
    private object lastDownItem;  // 上次播放语音的Item,如果相同则不再播放

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
                RenovateMoneyText.text = GameTool.LocalText("common_refresh") + "\n-" + renovateMoney.ToString();
            else
                RenovateMoneyText.text = GameTool.LocalText("common_refresh") + "\n" + "<color=#ff0000>-" + renovateMoney.ToString() + "</color>";
        }
    }
    private int renovateCount = 0;
    private int autoRefreshWave = -1;

    private readonly int ItemsCount = 4;

    private readonly string[] CannotAffordStrings = new string[] 
    {
        "cannot_buy1",
        "cannot_buy2",
        "cannot_buy3"
    };
    private readonly string[] CannotPlantingStrings = new string[]
    {
        "cannot_plant1",
        "cannot_plant2",
        "cannot_plant3"
    };
    private readonly string CannotPlantLilypad = GameTool.LocalText("cannot_plant4");
    private bool isTriggerCannotAfford;

    private int freeRefreshCount;  // 免费刷新次数

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

    private void OnDestroy()
    {
        AudioManager.Instance.AudioLists.Remove(this.audioSource);
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
                nowItem = item;
                break;
            }
        }
        if (!hasDown)
            isTriggerCannotAfford = false;
        if (hasDown || hasOtherDown)
            Invoke("PlayDaveSounds", 0.3f);
    }

    private void PlayDaveSounds()
    {
        if (!audioSource.isPlaying && nowItem != lastDownItem)
        {
            lastDownItem = nowItem;
            int index = UnityEngine.Random.Range(0, daveAudioClips.Count);
            audioSource.clip = daveAudioClips[index];
            audioSource.Play();
            var track = Dave.AnimationState.SetAnimation(1, "ShopingSpeak", false);
            // 前三个为长语音
            if (index < 3)
            {
                track.Complete += (e) =>
                {
                    var track = Dave.AnimationState.SetAnimation(1, "ShopingSpeak", false);
                    track.Complete += (e) =>
                    {
                        Dave.AnimationState.SetAnimation(1, "ShopingSpeak", false);
                    };
                };
            }
        }
    }

    private void InitItems()
    {
        for (int i = 0; i < ItemsCount; i++)
        {
            PropCardItem propItem = GameObject.Instantiate(PropItemPrefab, PropContent).GetComponent<PropCardItem>();
            propItem.CannotAfford += CannotAfford;
            propItem.CanNotPurchaseWaterPot += () =>
            {
                this.DialogText.text = FlowerPotItem.infoNoPurchase;
                isTriggerCannotAfford = true;
                lastDownItem = null;
            };
            propItem.gameObject.SetActive(false);
            propCardItems.Add(propItem);

            PlantCardItem plant = GameObject.Instantiate(PlantItemPrefab, PlantContent).GetComponent<PlantCardItem>();
            plant.CannotAfford += CannotAfford;
            plant.CanNotPlanting += CanNotPlanting;
            plant.CanNotPlantingLilypad += CanNotPlantingLilypad;
            plant.gameObject.SetActive(false);
            plantCardItems.Add(plant);
        }
        shopItems.AddRange(propCardItems);
        shopItems.AddRange(plantCardItems);
    }

    private void CannotAfford()
    {
        int index = UnityEngine.Random.Range(0, CannotAffordStrings.Length);
        this.DialogText.text = GameTool.LocalText(CannotAffordStrings[index]);
        isTriggerCannotAfford = true;
        lastDownItem = null;  // 买不起重新播放语音
    }

    private void CanNotPlanting()
    {
        int index = UnityEngine.Random.Range(0, CannotPlantingStrings.Length);
        this.DialogText.text = GameTool.LocalText(CannotPlantingStrings[index]);
        isTriggerCannotAfford = true;
        lastDownItem = null;
    }

    private void CanNotPlantingLilypad()
    {
        this.DialogText.text = CannotPlantLilypad;
        isTriggerCannotAfford = true;
        lastDownItem = null;
    }

    public void Refresh()
    {
        if (ShopManager.Instance.Money <= RenovateMoney)
            return;
        // 刷新前更新卡池
        ShopManager.Instance.UpdateCardPool();
        audioSource.clip = renovateSounds;
        audioSource.Play();
        animator.SetTrigger("refresh");

        float lucky = GameManager.Instance.UserData.LuckyProperties;
        /*
         * 出现白色品质的概率为100-蓝紫红  
         * 蓝色概率为 幸运40前 （20+幸运) 40-50(固定60) 50以上为（100-白色（10） - 紫色-红色）   
         * 紫色概率为幸运/2%  
         * 红色概率为幸运/10%   
         * 白色最低30 蓝色封顶60最低30  紫色概率封顶30， 红色概率封顶10
         */
        // 都乘10换算成从1000中取，增加精度,后面的区间需加前面的区间
        float redProbability = 10 * lucky / 10;
        if (redProbability > 100)
            redProbability = 100;
        float purpleProbability = 10 * lucky / 2;
        if (purpleProbability > 300)
            purpleProbability = 300;
        float blueProbability = 10 * (20 + lucky);
        if (lucky > 40 && lucky < 50)
            blueProbability = 600;
        if (lucky >= 50)
            blueProbability = 10 * (100 - 30 - purpleProbability / 10 - redProbability / 10);
        purpleProbability += redProbability;
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

        propDicts.Clear();
        // 植物 
        for (int i = 0; i < 4; i++)
        {
            int random = Random.Range(0, 101);
            int quality = 1;
            if (random > 40 && random <= 70)
            {
                quality = 2;
            }
            if (random > 70 && random <= 90)
            {
                quality = 3;
            }
            if (random > 90 && random <= 100)
            {
                quality = 4;
            }
            if (!propDicts.ContainsKey(quality))
                propDicts[quality] = 0;
            if (quality == 4 && propDicts[quality] == 3)
            {
                propDicts[3] = 1;
            }
            else
                propDicts[quality]++;
        }
        index = 0;
        foreach (var item in propDicts)
        {
            var plantCards = ShopManager.Instance.PlantDicts[1];
            if (ShopManager.Instance.PlantDicts.ContainsKey(item.Key))
                plantCards = ShopManager.Instance.PlantDicts[item.Key];
            HashSet<int> propHashSet = RandomUtils.RandomCreateNumber(plantCards.Count, item.Value);
            foreach (var hash in propHashSet)
            {
                plantCardItems[index].gameObject.SetActive(true);
                plantCardItems[index].SetPlant(plantCards[index]);
                index++;
            }
        }

        if (freeRefreshCount > 0)
        {
            freeRefreshCount--;
            FlowerPotItem.SetActive();
            if (freeRefreshCount == 0)
            {
                RenovateMoney = (renovateCount + 1) * (autoRefreshWave + 1) + (renovateCount + 1) * renovateCount / 4;
                renovateCount++;
            }
        }
        else
        {
            if (renovateCount != 0)
            {
                // 发钱刷新可点击获取花盆
                ShopManager.Instance.Money -= RenovateMoney;
            }
            // 刷新设置花盆
            FlowerPotItem.SetActive();
            RenovateMoney = (renovateCount + 1) * (autoRefreshWave + 1) + (renovateCount + 1) * renovateCount / 4;
            renovateCount++;
        }
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
            attributePanel = GameManager.Instance.GetAttributePanel();
            attributePanel.ShoppingPanel = this;
        }
    }

    private void OpenAttributePanel()
    {
        if (this.gameObject.activeSelf)
        {
            BtnRenovate.gameObject.SetActive(true);
            attributePanel = GameManager.Instance.GetAttributePanel();
            attributePanel.ShoppingPanel = this;
            Money.text = ShopManager.Instance.Money.ToString();
            renovateCount = 0;
            Refresh();
            freeRefreshCount = ShopManager.Instance.PurchasePropCount("key");
            if (freeRefreshCount > 0)
                RenovateMoney = 0;
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

    public void OpenBag()
    {
        BagPanel bagpanel = UIManager.Instance.PushPanel(UIPanelType.BagPanel) as BagPanel;
        bagpanel.ShoppingPanel = this;
        bagpanel.AutoClose = true;
    }

    public void SetSoundsItem(bool hasDown, object nowItem = null)
    {
        this.nowItem = nowItem;
        this.hasOtherDown = hasDown;
    }

    /// <summary>
    /// 购买商品时触发事件
    /// </summary>
    /// <param name="eventType"></param>
    public void OnEvent(PropPurchaseEvent eventType)
    {
        if (eventType.propName == "key")
        {
            freeRefreshCount++;
            RenovateMoney = 0;
        }
    }

    private void OnEnable()
    {
        this.EventStartListening();
    }

    private void OnDisable()
    {
        this.EventStopListening();
    }
}
