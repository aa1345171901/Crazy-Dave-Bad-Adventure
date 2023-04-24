using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopDownPlate
{
    public class GameManager : BaseManager<GameManager>
    {
        [Header("����")]
        [Tooltip("ƽ�׹�")]
        public GameObject Pot;
        [Tooltip("ľ�")]
        public Hammer Hammer;
        [Tooltip("С�Ƴ�")]
        public LawnMower LawnMower;
        [Tooltip("������")]
        public TransferGate TransferGate;

        [Tooltip("����ĸ�����")]
        public Transform IceGroundContent;

        [Tooltip("�˺����߼���")]
        public List<BaseProp> specialPropLists;

        [Space(10)]
        [Header("����")]
        [Tooltip("��������")]
        public SceneTransition SceneTransition;
        [Tooltip("�˺�HUD����")]
        public Font HUDFont;

        public Character Player { get; protected set; }

        public UserData UserData;// { get; set; }

        private bool isEnd;
        /// <summary>
        /// ��Ϸ�Ƿ����
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
                        SaveManager.Instance.DeleteUserData();  // ������Ҫɾ��
                    }
                }
            }
        }

        private bool isDaytime;
        /// <summary>
        /// �Ƿ��ƽ�������������
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

                        Invoke("SaveData", 3f);  // 3s���ұ����ܼ���
                    }
                    else
                    {
                        SceneTransition.TransitionToNight();
                    }
                }
            }
        }

        public Transform BrainPos { get; set; }   // ��������λ��

        public bool IsZombieShock { get; set; } // �Ƿ���Pot������ʬ�󣬽�ʬ��λ����˺�

        public bool HaveMagnetic { get; set; } // �Ƿ�������Ҵ���

        public bool HaveBlackHole { get; set; } // �Ƿ���������ڶ�

        public int ZombieFlyDamage { get; set; }  // ��ͷ�˺�Ϊƽ�׹��˺� * ������/4 ���1;

        public bool IsOpenVocalConcert => vocalConcert.OpenVocalConcert;

        public float DecelerationRatio { get; set; } = 1; // �ڱ����ϵļ��ٱ���

        public bool CanAttack  => balls.Count == 0; // Ͷ����ʬ�Ƿ��ڹ���
        public List<GameObject> balls = new List<GameObject>(); // ���ڹ���������

        public List<Coin> Coins { get; set; } = new List<Coin>();  // �����������գ��ڽ������ʱ���룬��ʧʱRemove

        private BattlePanel battlePanel;
        private PausePanel pausePanel;
        private VocalConcert vocalConcert;

        public PumpkinHead pumpkinHead { get; private set; }

        private void Start()
        {
            LevelManager.Instance.Init();
            UIManager.Instance.ClearDict();  // �����˵���UIManager���ڲ���MonoBehaviour������Ҫ�ֶ������ֵ����
            LoadData();
        }

        private void SaveData()
        {
            SaveManager.Instance.SaveUserData();  // ÿ������ʱ����
        }

        private void LoadData()
        {
            SaveManager.Instance.LoadUserData();  // ��ȡ�û�����
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
        /// ��ȡ��������Ƿ�ӵ��
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
                            // �����ǽ����Եģ����Խ�������������ˢ��
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
        /// ÿ���̵��������
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
