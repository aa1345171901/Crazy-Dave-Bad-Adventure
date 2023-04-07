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

        [Tooltip("伤害道具集合")]
        public List<BaseProp> specialPropLists;

        [Space(10)]
        [Header("场景")]
        [Tooltip("场景过渡")]
        public SceneTransition SceneTransition;
        [Tooltip("伤害HUD字体")]
        public Font HUDFont;

        public Character Player { get; protected set; }

        public UserData UserData;// { get; set; }

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

        private BattlePanel battlePanel;
        private PausePanel pausePanel;
        private VocalConcert vocalConcert;

        private void Start()
        {
            LevelManager.Instance.Init();
            UIManager.Instance.ClearDict();  // 到主菜单后UIManager由于不是MonoBehaviour所以需要手动进行字典清空
            LoadData();
        }

        private void SaveData()
        {
            SaveManager.Instance.SaveUserData();  // 每波结束时保存
        }

        private void LoadData()
        {
            SaveManager.Instance.LoadUserData();  // 读取用户数据
            LoadPropMsg();
            if (SaveManager.Instance.IsLoadUserData)
            {
                foreach (var item in specialPropLists)
                {
                    item.DayEnd();
                }
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
            battlePanel = UIManager.Instance.PushPanel(UIPanelType.BattlePanel) as BattlePanel;
            vocalConcert = Player.GetComponentInChildren<VocalConcert>();
            specialPropLists.Add(vocalConcert);
            AudioManager.Instance.PlayEffectSoundByName("startWave");
            AudioManager.Instance.PlayBackMusic(2);
        }

        public void DoDamage(int damage, DamageType damageType = DamageType.Zombie)
        {
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
                    pausePanel = UIManager.Instance.PushPanel(UIPanelType.PausePanel) as PausePanel;
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
    }
}
