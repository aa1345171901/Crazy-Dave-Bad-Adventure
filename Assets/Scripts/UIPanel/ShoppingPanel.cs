using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

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

    [Tooltip("����")]
    public FlowerPotShopItem FlowerPotItem;

    public AudioSource audioSource;

    public SkeletonGraphic Dave;
    public List<AudioClip> daveAudioClips;
    private bool hasOtherDown;    // ����Ƿ�����Ʒ��������
    private object nowItem;        // ��ǰ����������item,�������ߵ�չʾҲ��shopping�������ʹ��object
    private object lastDownItem;  // �ϴβ���������Item,�����ͬ���ٲ���

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
                RenovateMoneyText.text = "ˢ��\n-" + renovateMoney.ToString();
            else
                RenovateMoneyText.text = "ˢ��\n" + "<color=#ff0000>-" + renovateMoney.ToString() + "</color>";
        }
    }
    private int renovateCount = 0;
    private int autoRefreshWave = -1;

    private readonly int ItemsCount = 4;

    private readonly string[] CannotAffordStrings = new string[] 
    {
        "���ܺã��Ҳ��䣬ӦΪ��û��ǮǮ",
        "ԽûǮ��ʱ��Խ�뻨Ǯ���Ҫ����l",
        "���ﳵ��Ķ���ֻ�ܿ����¼�"
    };
    private readonly string[] CannotPlantingStrings = new string[]
    {
        "û�л���ֲ�ﲻ����ֲ",
        "ֲ��ֻ����<color=#ff0000>����</color>������,�������ͨ��<color=#ff0000>ˢ��</color>���л�ȡ",
        "����ʳ���Ҫ����"
    };
    private readonly string CannotPlantLilypad = "��Ҷ��Ҫ��ֲ��<color=#ff0000>ˮ����</color>��\n��Ҫ�ȹ���<color=#ff0000>ˮ����</color>";
    private bool isTriggerCannotAfford;

    private int freeRefreshCount;  // ���ˢ�´���

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
            // ǰ����Ϊ������
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
        this.DialogText.text = CannotAffordStrings[index];
        isTriggerCannotAfford = true;
        lastDownItem = null;  // �������²�������
    }

    private void CanNotPlanting()
    {
        int index = UnityEngine.Random.Range(0, CannotPlantingStrings.Length);
        this.DialogText.text = CannotPlantingStrings[index];
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
        // ˢ��ǰ���¿���
        ShopManager.Instance.UpdateCardPool();
        audioSource.clip = renovateSounds;
        audioSource.Play();
        animator.SetTrigger("refresh");

        float lucky = GameManager.Instance.UserData.Lucky;
        /*
         * ���ְ�ɫƷ�ʵĸ���Ϊ100-���Ϻ�  
         * ��ɫ����Ϊ ����40ǰ ��20+����) 40-50(�̶�60) 50����Ϊ��100-��ɫ��10�� - ��ɫ-��ɫ��   
         * ��ɫ����Ϊ����/2%  
         * ��ɫ����Ϊ����/10%   
         * ��ɫ���30 ��ɫ�ⶥ60���30  ��ɫ���ʷⶥ30�� ��ɫ���ʷⶥ10
         */
        // ����10����ɴ�1000��ȡ�����Ӿ���,������������ǰ�������
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

        // ��1000��ѡ4����������4�����ߵ�Ʒ��
        HashSet<int> hashSet = RandomUtils.RandomCreateNumber(1000, 4);
        // ��Ʒ�ʷ���
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
        // ��ȡ��ͬ�ĵ��ߣ�ʹͬƷ�ʵ��߲���ͬ
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

        // ֲ�� todo
        var plantCards = ShopManager.Instance.PlantLists;
        hashSet = RandomUtils.RandomCreateNumber(plantCards.Count, 4);
        index = 0;
        foreach (var item in hashSet)
        {
            plantCardItems[index].gameObject.SetActive(true);
            plantCardItems[index].SetPlant(plantCards[item]);
            index++;
        }

        if (freeRefreshCount > 0)
        {
            freeRefreshCount--;
            FlowerPotItem.SetActive();
            if (freeRefreshCount == 0)
            {
                RenovateMoney = (renovateCount + 1) * (autoRefreshWave + 1) + (renovateCount + 1) * renovateCount / 2;
                renovateCount++;
            }
        }
        else
        {
            if (renovateCount != 0)
            {
                // ��Ǯˢ�¿ɵ����ȡ����
                ShopManager.Instance.Money -= RenovateMoney;
                FlowerPotItem.SetActive();
            }
            RenovateMoney = (renovateCount + 1) * (autoRefreshWave + 1) + (renovateCount + 1) * renovateCount / 2;
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
            // �ӻ�԰����
            attributePanel = UIManager.Instance.PushPanel(UIPanelType.AttributePanel) as AttributePanel;
            attributePanel.ShoppingPanel = this;
        }
    }

    private void OpenAttributePanel()
    {
        if (this.gameObject.activeSelf)
        {
            BtnRenovate.gameObject.SetActive(true);
            attributePanel = UIManager.Instance.PushPanel(UIPanelType.AttributePanel) as AttributePanel;
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
    /// ������Ʒʱ�����¼�
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
