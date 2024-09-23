using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopDownPlate
{
    public class GameManager : BaseManager<GameManager>
    {
        [Header("道具")]
        [Tooltip("平底锅")]
        public GameObject Pot;
        [Tooltip("木槌")]
        public Hammer Hammer;
        [Tooltip("小推车")]
        public LawnMower LawnMower;
        [Tooltip("传送门")]
        public TransferGate TransferGate;

        [Tooltip("冰面的父物体")]
        public Transform IceGroundContent;

        [Tooltip("伤害道具集合")]
        public List<BaseProp> specialPropLists;

        [Space(10)]
        [Header("场景")]
        [Tooltip("场景过渡")]
        public SceneTransition SceneTransition;
        [Tooltip("伤害HUD字体")]
        public Font HUDFont;

        public Character Player { get; protected set; }

        public UserData UserData { get; set; } = new UserData();

        private bool isEnd;
        /// <summary>
        /// 游戏是否结束
        /// </summary>
        public bool IsEnd 
        {
            get
            {
                return isEnd;
            }
            set
            {
                if (value != isEnd)
                {
                    isEnd = value;
                    if (isEnd)
                    {
                        LevelManager.Instance.GameOver();
                        ShowTipsPanel(TipsType.GameOver);
                        AudioManager.Instance.StopBackMusic();
                        AudioManager.Instance.PlayEffectSoundByName("GameOver");
                        SaveManager.Instance.DeleteUserData();  // 死亡需要删档
                    }
                }
            }
        }

        private bool isDaytime;
        /// <summary>
        /// 是否攻势结束，到白天了
        /// </summary>
        public bool IsDaytime
        {
            get
            {
                return isDaytime;
            }
            set
            {
                if (value != isDaytime)
                {
                    isDaytime = value;
                    if (isDaytime)
                    {
                        foreach (var item in specialPropLists)
                        {
                            item.DayEnd();
                        }
                        SceneTransition.TransitionToDaytime();

                        Invoke("OpenShop", SceneTransition.TransitionTime * 2);
                        battlePanel.GetGold();
                        AudioManager.Instance.PlayEffectSoundByName("winmusic");

                        GardenManager.Instance.Sun += UserData.Sunshine;

                        Invoke("SaveData", 3f);  // 3s后金币必能能加完
                    }
                    else
                    {
                        SceneTransition.TransitionToNight();
                    }
                }
            }
        }

        public Transform BrainPos { get; set; }   // 死后脑子位置

        public bool IsZombieShock { get; set; } // 是否有Pot打死僵尸后，僵尸部位冲击伤害

        public bool HaveMagnetic { get; set; } // 是否有吸金币磁体

        public bool HaveBlackHole { get; set; } // 是否有吸阳光黑洞

        public int ZombieFlyDamage { get; set; }  // 飞头伤害为平底锅伤害 * 菠菜数/4 最大1;

        public bool IsOpenVocalConcert => vocalConcert.OpenVocalConcert;

        public bool IsFog => vocalConcert.isFog;

        public float DecelerationRatio { get; set; } = 1; // 在冰面上的减速比例

        public bool CanAttack  => balls.Count == 0; // 投篮僵尸是否在攻击
        public List<GameObject> balls = new List<GameObject>(); // 正在攻击的篮球

        public List<Coin> Coins { get; set; } = new List<Coin>();  // 用于吸金菇吸收，在金币生成时加入，消失时Remove

        private BattlePanel battlePanel;
        private PausePanel pausePanel;
        private VocalConcert vocalConcert;

        public PumpkinHead pumpkinHead { get; private set; }

        private int headNum;
        /// <summary>
        /// 本次通过击杀僵尸获取的头数量
        /// </summary>
        public int HeadNum
        {
            get
            {
                return headNum;
            }
            set
            {
                headNum = value;
                battlePanel?.SetGrowText();
            }
        }

        public BattleMode nowMode { get; set; }

        private void Start()
        {
            LevelManager.Instance.Init();
            UIManager.Instance.ClearDict();  // 到主菜单后UIManager由于不是MonoBehaviour所以需要手动进行字典清空
            LoadData();
        }

        private void SaveData()
        {
            SaveManager.Instance.SaveUserData();  // 每波结束时保存
            SaveManager.Instance.SaveExternalGrowData();
        }

        private void LoadData()
        {
            nowMode = SaveManager.Instance.LoadSpecialData();
            ExternlGrow();
            switch (nowMode)
            {
                case BattleMode.None:
                    IntoNormalMode();
                    break;
                case BattleMode.BossMode:
                    IntoNormalMode();
                    ShopManager.Instance.Money = 100000;
                    GardenManager.Instance.Sun = 2500000;
                    LevelManager.Instance.IndexWave = 18;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 局外成长属性增加
        /// </summary>
        void ExternlGrow()
        {
            // 特殊模式或者没有读取存档增加局外成长属性
            if (nowMode != BattleMode.None || !SaveManager.Instance.IsLoadUserData)
            {
                foreach (var item in SaveManager.Instance.externalGrowthData.keys)
                {
                    var confItem = ConfManager.Instance.confMgr.externlGrow.GetItemByKey(item);
                    var growType = (GrowType)confItem.growType;
                    var sum = SaveManager.Instance.externalGrowthData.GetGrowSumValueByKey(item);
                    int level = SaveManager.Instance.externalGrowthData.GetLevelByKey(item);
                    switch (growType)
                    {
                        case GrowType.Attribute:
                            UserData.AddValue(item, sum);
                            break;
                        case GrowType.SlotNum:
                            GardenManager.Instance.SlotNum += sum;
                            break;
                        case GrowType.StartProp:
                            if (level != 0)
                            {
                                var list = ShopManager.Instance.PropDicts[level];
                                var propCard = list[Random.Range(0, list.Count)];
                                ShopManager.Instance.PurchaseProp(propCard, 0);
                            }
                            break;
                        case GrowType.StartPlant:
                            break;
                        case GrowType.StartSun:
                            break;
                        case GrowType.StartGold:
                            break;
                        case GrowType.LifeTime:
                            break;
                        case GrowType.Curse:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 执行特殊道具初始化，有数据加载则打开商店
        /// </summary>
        void IntoNormalMode()
        {
            LoadPropMsg();
            if (SaveManager.Instance.IsLoadUserData)
            {
                foreach (var item in specialPropLists)
                {
                    item.DayEnd();
                }

                GardenManager.Instance.LoadCrater();

                SceneTransition.TransitionToDaytime();
                OpenShop();
                LevelManager.Instance.LoadTimer();
                isDaytime = true;
                SaveManager.Instance.IsLoadUserData = false;
            }
        }

        /// <summary>
        /// 读取特殊道具是否拥有
        /// </summary>
        private void LoadPropMsg()
        {
            var purchasedProps = ShopManager.Instance.PurchasedProps;
            foreach (var item in purchasedProps)
            {
                if (item.propDamageType != PropDamageType.None)
                    SetPropDamage(item.propDamageType, item.defalutDamage, item.coolingTime);
                else
                {
                    switch (item.propName)
                    {
                        case "PortalCard":
                            SetTransferGate();
                            break;
                        case "Spinacia":
                            IsZombieShock = true;
                            break;
                        case "magnetic":
                            HaveMagnetic = true;
                            // 由于是降属性的，所以解锁完能力后不再刷新
                            RemoveShopPropDict(item);
                            break;
                        case "blackhole":
                            HaveBlackHole = true;
                            RemoveShopPropDict(item);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void RemoveShopPropDict(PropCard item)
        {
            var propDictsQuality = ShopManager.Instance.PropDicts[item.quality];
            PropCard remove = null;
            foreach (var prop in propDictsQuality)
            {
                if (prop.propName == item.propName)
                {
                    remove = prop;
                }
            }
            if (remove != null)
                propDictsQuality.Remove(remove);
        }

        public void SetPlayer(Character character)
        {
            this.Player = character;
#if !UNITY_ANDROID
            battlePanel = UIManager.Instance.PushPanel(UIPanelType.BattlePanel) as BattlePanel;
#else
            battlePanel = UIManager.Instance.PushPanel(UIPanelType.MobieBattlePanel) as BattlePanel;
#endif
            vocalConcert = Player.GetComponentInChildren<VocalConcert>();
            pumpkinHead = Player.GetComponentInChildren<PumpkinHead>();
            pumpkinHead.gameObject.SetActive(false);
            specialPropLists.Add(vocalConcert);
            specialPropLists.Add(pumpkinHead);
            AudioManager.Instance.PlayEffectSoundByName("startWave");
            AudioManager.Instance.PlayBackMusic(2);
        }

        public void DoDamage(int damage, DamageType damageType = DamageType.Zombie)
        {
            if (IsEnd)
                return;
            Player.Health.DoDamage(damage, damageType);
            battlePanel?.SetHPBar(Player.Health.health, Player.Health.maxHealth);
        }

        public void AddHP(int value)
        {
            Player.Health.AddHealth(value);
            battlePanel?.SetHPBar(Player.Health.health, Player.Health.maxHealth);
        }

        public void ShowTipsPanel(TipsType tipsType)
        {
            TipsPanel tipsPanel = UIManager.Instance.PushPanel(UIPanelType.TipsPanel) as TipsPanel;
            tipsPanel.SetTips(tipsType);
        }

        public void SetProgressSlider(float value)
        {
            battlePanel?.SetProgressSlider(value);
        }

        public void SetRunSlider(float value)
        {
            battlePanel?.SetRunSlider(value);
        }

        public void NextWave()
        {
            IsDaytime = false;
            LevelManager.Instance.IndexWave++;
            LevelManager.Instance.Init();
            AudioManager.Instance.PlayBackMusic(2);
            AudioManager.Instance.PlayEffectSoundByName("startWave");
            GardenManager.Instance.PlantsGoToWar();
            battlePanel.UpdatePlantPage();
            Reuse();
        }

        /// <summary>
        /// 每波商店结束调用
        /// </summary>
        private void Reuse()
        {
            Player.Reuse();
            Player.Health.Reuse();
            battlePanel.SetHPBar(Player.Health.health, Player.Health.maxHealth);
            foreach (var item in specialPropLists)
            {
                item.Reuse();
            }
        }

        private void OpenShop()
        {
            UIManager.Instance.PushPanel(UIPanelType.ShopingPanel);
            AudioManager.Instance.StopBackMusic();
            AudioManager.Instance.PlayEffectSoundByName("zamboni");
            AudioManager.Instance.PlayShoppingMusic(2.5f);
        }

        public void SetPropDamage(PropDamageType propDamageType, int defaultDamage, float coolingTime)
        {
            switch (propDamageType)
            {
                case PropDamageType.None:
                    break;
                case PropDamageType.LawnMower:
                    LawnMower.DefaultDamage = defaultDamage;
                    LawnMower.DefaultAttackCoolingTime = coolingTime;
                    LawnMower.gameObject.SetActive(true);
                    if (!specialPropLists.Contains(LawnMower))
                        specialPropLists.Add(LawnMower);
                    break;
                case PropDamageType.Hammer:
                    Hammer.DefaultDamage = defaultDamage;
                    Hammer.DefaultAttackCoolingTime = coolingTime;
                    if (!specialPropLists.Contains(Hammer))
                        specialPropLists.Add(Hammer);
                    break;
                case PropDamageType.VocalConcert:
                    break;
                default:
                    break;
            }
        }

        public void SetTransferGate()
        {
            TransferGate.SetTransferGate();
            specialPropLists.Add(TransferGate);
        }

        public void Victory()
        {
            LevelManager.Instance.Victory();
            isEnd = true;
            UIManager.Instance.PushPanel(UIPanelType.VictoryPanel);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pausePanel != null && pausePanel.gameObject.activeSelf)
                {
                    pausePanel.Close();
                }
                else
                {
                    Pause();
                }
            }
            if (!IsDaytime)
            {
                foreach (var item in specialPropLists)
                {
                    item.ProcessAbility();
                }
            }
        }

        public void Pause()
        {
            pausePanel = UIManager.Instance.PushPanel(UIPanelType.PausePanel) as PausePanel;
        }
    }
}
