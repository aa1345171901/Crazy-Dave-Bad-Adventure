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

        public UserData UserData;// { get; protected set; } = new UserData();

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
                        // todo 阳光
                        GardenManager.Instance.Sun += UserData.Sunshine;
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

        public int PotDamage { get; set; }  // 平底锅的伤害，飞头伤害为平底锅伤害 * 菠菜数/4 最大1;

        private BattlePanel battlePanel;
        private VocalConcert vocalConcert;

        private void Start()
        {
            LevelManager.Instance.Init();
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
