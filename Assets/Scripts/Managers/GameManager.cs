using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TopDownPlate
{
    public class GameManager : BaseManager<GameManager>
    {
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

        public UserData UserData { get; set; }

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
                        SaveManager.Instance.DeleteUserData();  // 死亡需要删档
                        LevelManager.Instance.GameOver();
                        ShowTipsPanel(TipsType.GameOver);
                        AudioManager.Instance.StopBackMusic();
                        AudioManager.Instance.PlayEffectSoundByName("GameOver");
                        AchievementManager.Instance.SetAchievementType4();
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
                        ClearHpBar();
                        battlePanel.GetGold();
                        AudioManager.Instance.PlayEffectSoundByName("winmusic");

                        GardenManager.Instance.Sun += UserData.Sunshine;

                        Invoke("SaveData", 3f);  // 3s后金币必能能加完
                    }
                    else
                    {
                        PlayerEnable = !isDaytime;
                        SceneTransition.TransitionToNight();
                    }
                }
            }
        }
        public bool PlayerEnable { get; set; } = true;

        public Transform BrainPos { get; set; }   // 死后脑子位置

        public bool IsZombieShock { get; set; } // 是否有Pot打死僵尸后，僵尸部位冲击伤害

        public bool HaveMagnetic { get; set; } // 是否有吸金币磁体

        public bool HaveBlackHole { get; set; } // 是否有吸阳光黑洞

        public int ZombieFlyDamage { get; set; }  // 飞头伤害为平底锅伤害 * 菠菜数/4 最大1;

        public bool IsOpenVocalConcert => vocalConcert.OpenVocalConcert;

        public bool IsFog => vocalConcert.isFog;

        public float DecelerationRatio { get; set; } = 1; // 在冰面上的减速比例

        public bool CanAttack => balls.Count == 0; // 投篮僵尸是否在攻击
        public List<GameObject> balls = new List<GameObject>(); // 正在攻击的篮球

        public List<Coin> Coins { get; set; } = new List<Coin>();  // 用于吸金菇吸收，在金币生成时加入，消失时Remove

        /// <summary>
        /// 局外花园种子的植物只在当波有效
        /// </summary>
        public List<Plant> plantSeedCard { get; set; } = new List<Plant>();

        private BattlePanel battlePanel;
        private PausePanel pausePanel;
        public AttributePanel attributePanel { get; private set; }

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
                AchievementManager.Instance.SetAchievementType3(headNum);
            }
        }

        public int killNum { get; set; }

        /// <summary>
        /// 已经使用的复活次数
        /// </summary>
        public int resurrection { get; set; }

        public BattleMode nowMode { get; set; }

        /// <summary>
        /// 是否能时间回溯，本局删档了可以
        /// </summary>
        public bool canBackInTime { get; set; }

        /// <summary>
        /// 有血条的僵尸统一管理
        /// </summary>
        public HashSet<Health> haveHpBarZombie { get; set; } = new HashSet<Health>();

        private void Start()
        {
            UIManager.Instance.ClearDict();  // 到主菜单后UIManager由于不是MonoBehaviour所以需要手动进行字典清空
            LoadData();
            StartCoroutine(CreateSeedCard());
        }

        private void SaveData()
        {
            SaveManager.Instance.SaveUserData();  // 每波结束时保存
            SaveManager.Instance.SaveExternalGrowData();
            AchievementManager.Instance.SaveData();
        }

        private void LoadData()
        {
            UserData = new UserData("dave");
            nowMode = SaveManager.Instance.LoadSpecialData();
            LevelManager.Instance.Init();
            ExternlGrow();
            switch (nowMode)
            {
                case BattleMode.None:
                    if (!SaveManager.Instance.IsLoadUserData)
                        AchievementManager.Instance.SetAchievementType1();
                    IntoNormalMode();
                    break;
                case BattleMode.PlayerMode:
                    ShopManager.Instance.Money = 100000;
                    GardenManager.Instance.Sun = 2500000;
                    LevelManager.Instance.IndexWave = 18;
                    IntoNormalMode();
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
            //var plantCard1 = ConfManager.Instance.confMgr.data.plantCards.PlantCards.Last();
            //var plantAttribute1 = new PlantAttribute(plantCard1);
            //plantAttribute1.CultivatePlant(true);
            //GardenManager.Instance.PlantAttributes.Add(plantAttribute1);
            //GardenManager.Instance.FlowerPotCount++;
            //GardenManager.Instance.IsLoadPlantData = true;
            //GardenManager.Instance.PlantsGoToWar();

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
                                ShopManager.Instance.UpdateCardPool();
                                var list = ShopManager.Instance.PropDicts[level];
                                var propCard = list[Random.Range(0, list.Count)];
                                ShopManager.Instance.PurchaseProp(propCard, 0, null, true);
                            }
                            break;
                        case GrowType.StartPlant:
                            int max = level * 40;
                            int min = (level - 1) * 40;
                            if (min == 120)
                                min = 80;
                            var targetList = new List<PlantCard>();
                            foreach (var plant in ShopManager.Instance.PlantLists)
                            {
                                if (plant.defaultPrice >= min && plant.defaultPrice <= max)
                                {
                                    targetList.Add(plant);
                                }
                            }
                            var hashSet = RandomUtils.RandomCreateNumber(targetList.Count, Mathf.Max(1, level - 2));
                            foreach (var index in hashSet)
                            {
                                var plantCard = targetList[index];
                                var plantAttribute = new PlantAttribute(plantCard);
                                plantAttribute.CultivatePlant(true);
                                GardenManager.Instance.PlantAttributes.Add(plantAttribute);
                                GardenManager.Instance.FlowerPotCount++;
                                if (plantAttribute.isManual)
                                {
                                    GardenManager.Instance.CardslotPlant.Add(plantAttribute);
                                }
                            }
                            GardenManager.Instance.IsLoadPlantData = true;
                            GardenManager.Instance.PlantsGoToWar();
                            break;
                        case GrowType.StartSun:
                            GardenManager.Instance.Sun = sum;
                            break;
                        case GrowType.StartGold:
                            ShopManager.Instance.Money = sum;
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
                LevelManager.Instance.LoadTimer();
                isDaytime = true;
                OpenShop();
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
                if (item.propType != PropType.None)
                    SetPropDamage(item.propType, item.value1, item.coolingTime);
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

        public void DoDamage(int damage, ZombieType zombieType, DamageType damageType = DamageType.Zombie)
        {
            if (IsEnd)
                return;
            int beforeHp = Player.Health.health;
            Player.Health.DoDamage(damage, damageType);
            if (damageType == DamageType.Zombie)
            {
                SaveManager.Instance.AddTakingDamageValue((int)zombieType, beforeHp - Player.Health.health);
            }
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

        public void SetRunSliderWidth(float value)
        {
            battlePanel?.SetRunSliderWidth(value);
        }

        public void SetDashSlider(int count, float value, int remainCount)
        {
            battlePanel?.SetDashSlider(count, value, remainCount);
        }

        public void SetCloseAttack(bool isCloseAttack)
        {
            battlePanel.SetCloseAttack(isCloseAttack);
        }

        public void NextWave()
        {
            SaveData();
            IsDaytime = false;
            LevelManager.Instance.IndexWave++;
            LevelManager.Instance.Init();
            AudioManager.Instance.PlayBackMusic(2);
            AudioManager.Instance.PlayEffectSoundByName("startWave");
            GardenManager.Instance.PlantsGoToWar();
            foreach (var item in typeof(UserData).GetFields())
            {
                if (item.FieldType == typeof(int))
                {
                    var value = (int)item.GetValue(UserData);
                    AchievementManager.Instance.SetAchievementType10(item.Name, value);
                }
            }
            battlePanel.UpdatePlantPage();
            ClearSeedCards();
            Reuse();
        }

        /// <summary>
        /// 每波商店结束调用
        /// </summary>
        private void Reuse()
        {
            if (ShopManager.Instance.GetPurchaseTypeList(PropType.DeathGod).Count > 0)
            {
                var maxHp = Mathf.RoundToInt(UserData.MaximumHP * 0.9f);
                UserData.MaximumHP = maxHp <= 0 ? 1 : maxHp;
            }
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
            PlayerEnable = !IsDaytime;
            UIManager.Instance.PushPanel(UIPanelType.ShopingPanel);
            AudioManager.Instance.StopBackMusic();
            AudioManager.Instance.PlayEffectSoundByName("zamboni");
            AudioManager.Instance.PlayShoppingMusic(2.5f);
        }

        public void SetPropDamage(PropType propDamageType, int defaultDamage, float coolingTime)
        {
            switch (propDamageType)
            {
                case PropType.LawnMower:
                    SetAddPropDamage<LawnMower>(defaultDamage, coolingTime);
                    break;
                case PropType.Hammer:
                    SetPropDamage<Hammer>(defaultDamage, coolingTime);
                    break;
                case PropType.SmellyFart:
                    SetPropDamage<SmellyFart>(defaultDamage, coolingTime, true);
                    break;
                case PropType.FireElf:
                    SetPropDamage<FireElf>(defaultDamage, coolingTime);
                    break;
                case PropType.WaterElf:
                    SetPropDamage<WaterElf>(defaultDamage, coolingTime);
                    break;
                case PropType.DarkCloud:
                    SetAddPropDamage<DarkCloud>(defaultDamage, coolingTime);
                    break;
                case PropType.DeathGod:
                    SetPropDamage<DeathGod>(defaultDamage, coolingTime, true);
                    break;
                case PropType.Gun:
                    var countDict = new Dictionary<string, int>();
                    foreach (var item in ShopManager.Instance.GetPurchaseTypeList(propDamageType))
                    {
                        if (!countDict.ContainsKey(item.propName))
                            countDict[item.propName] = 0;
                        countDict[item.propName]++;
                    }
                    var count = countDict.Values.Max();
                    var haveCount = specialPropLists.Where((e) => e is Gun).Count();
                    if (count > haveCount)
                        SetAddPropDamage<Gun>(defaultDamage, coolingTime);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 全局只有一个
        /// </summary>
        void SetPropDamage<T>(int defaultDamage, float coolingTime, bool isPlayerParent = false) where T : BaseProp
        {
            bool haveProp = specialPropLists.Contains<T>();
            if (!haveProp)
            {
                var prefab = Resources.Load<T>("Prefabs/Props/" + typeof(T).Name);
                var propGo = GameObject.Instantiate(prefab);
                if (isPlayerParent)
                {
                    propGo.transform.SetParent(Player.transform, false);
                }
                propGo.DefaultDamage = defaultDamage;
                propGo.DefaultAttackCoolingTime = coolingTime;
                specialPropLists.Add(propGo);
            }
        }

        /// <summary>
        /// 可以有多个
        /// </summary>
        void SetAddPropDamage<T>(int defaultDamage, float coolingTime) where T : BaseProp
        {
            var prefab = Resources.Load<T>("Prefabs/Props/" + typeof(T).Name);
            var target = GameObject.Instantiate(prefab);
            target.DefaultDamage = defaultDamage;
            target.DefaultAttackCoolingTime = coolingTime;
            target.gameObject.SetActive(true);
            if (!specialPropLists.Contains(target))
                specialPropLists.Add(target);
        }

        public void RemoveProp(PropCard propCard, bool isSellAll, int sellCount)
        {
            int count = ShopManager.Instance.PurchasePropCount(propCard.propName);
            if (propCard.propType == PropType.None)
            {
                switch (propCard.propName)
                {
                    case "PortalCard":
                        if (count == 0)
                            RemoveTransferGate();
                        break;
                    case "Spinacia":
                        if (count == 0)
                            IsZombieShock = false;
                        break;
                    case "magnetic":
                        if (count == 0)
                            HaveMagnetic = false;
                        break;
                    case "blackhole":
                        if (count == 0)
                            HaveBlackHole = false;
                        break;
                    case "cardSlot":
                        int slotNum = GardenManager.Instance.SlotNum -= sellCount;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (propCard.propType)
                {
                    case PropType.LawnMower:
                        RemoveProp<LawnMower>(count, isSellAll);
                        break;
                    case PropType.Hammer:
                        RemoveProp<Hammer>(count);
                        break;
                    case PropType.SmellyFart:
                        RemoveProp<SmellyFart>(count);
                        break;
                    case PropType.FireElf:
                        count = ShopManager.Instance.GetPurchaseTypeList(propCard.propType).Count;
                        RemoveProp<FireElf>(count);
                        break;
                    case PropType.WaterElf:
                        count = ShopManager.Instance.GetPurchaseTypeList(propCard.propType).Count;
                        RemoveProp<WaterElf>(count);
                        break;
                    case PropType.DarkCloud:
                        RemoveProp<DarkCloud>(count, isSellAll);
                        break;
                    case PropType.DeathGod:
                        RemoveProp<DeathGod>(count);
                        break;
                    case PropType.Gun:
                        var countDict = new Dictionary<string, int>();
                        foreach (var item in ShopManager.Instance.GetPurchaseTypeList(propCard.propType))
                        {
                            if (!countDict.ContainsKey(item.propName))
                                countDict[item.propName] = 0;
                            countDict[item.propName]++;
                        }
                        if (countDict.Values.Count != 0)
                            count = countDict.Values.Max();
                        RemoveProp<Gun>(count, isSellAll);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 全局只有一个的特殊道具，全卖了才删
        /// </summary>
        void RemoveProp<T>(int count) where T : BaseProp
        {
            if (count == 0)
            {
                var prop = specialPropLists.GetValue<T>();
                if (prop != null)
                {
                    specialPropLists.Remove(prop);
                    GameObject.Destroy(prop.gameObject);
                }
            }
        }

        /// <summary>
        /// 全局可以有多个的道具，卖一个删一个
        /// </summary>
        void RemoveProp<T>(int count, bool isSellAll) where T : BaseProp
        {
            var targetPropList = new List<T>();
            foreach (var item in specialPropLists)
            {
                if (item is T target)
                {
                    targetPropList.Add(target);
                }
            }
            if (isSellAll)
            {
                foreach (var item in targetPropList)
                {
                    specialPropLists.Remove(item);
                }
                foreach (var item in targetPropList)
                {
                    GameObject.Destroy(item.gameObject);
                }
            }
            else
            {
                var target = targetPropList.First();
                specialPropLists.Remove(target);
                GameObject.Destroy(target.gameObject);
            }
        }

        public void SetTransferGate()
        {
            bool haveTransferGate = specialPropLists.Contains<TransferGate>();
            if (!haveTransferGate)
            {
                var transferGatePrefab = Resources.Load<TransferGate>("Prefabs/Props/TransferGates");
                var transferGate = GameObject.Instantiate(transferGatePrefab);
                specialPropLists.Add(transferGate);
                transferGate.gameObject.SetActive(true);
                transferGate.SetTransferGate();
            }
        }

        public void RemoveTransferGate()
        {
            int count = ShopManager.Instance.PurchasePropCount("PortalCard");
            if (count == 0)
            {
                var transferGate = specialPropLists.GetValue<TransferGate>();
                if (transferGate != null)
                {
                    specialPropLists.Remove(transferGate);
                    GameObject.Destroy(transferGate.gameObject);
                }
            }
        }

        public void Victory()
        {
            LevelManager.Instance.Victory();
            ClearHpBar();
            SaveManager.Instance.DeleteUserData();
            SaveManager.Instance.SaveExternalGrowData();
            AchievementManager.Instance.SaveData();
            isEnd = true;
            UIManager.Instance.PushPanel(UIPanelType.VictoryPanel);
        }

        public AttributePanel GetAttributePanel()
        {
            attributePanel = UIManager.Instance.PushPanel(UIPanelType.AttributePanel) as AttributePanel;
            return attributePanel;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isEnd)
            {
                if (pausePanel != null && pausePanel.gameObject.activeSelf)
                {
                    pausePanel.Close();
                }
                else
                {
                    if (UIManager.Instance.TopPanel is BagPanel)
                        UIManager.Instance.PopPanel();
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
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShopManager.Instance.PurchaseProp(ConfManager.Instance.confMgr.propCards.PropCards.Find((e) => e.propName == ConfManager.Instance.confMgr.propCards.GetItemByTypeLevel(7, 1).propName), 1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ShopManager.Instance.PurchaseProp(ConfManager.Instance.confMgr.propCards.PropCards.Find((e) => e.propName == ConfManager.Instance.confMgr.propCards.GetItemByTypeLevel(8, 2).propName), 1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ShopManager.Instance.PurchaseProp(ConfManager.Instance.confMgr.propCards.PropCards.Find((e) => e.propName == ConfManager.Instance.confMgr.propCards.GetItemByTypeLevel(9, 3).propName), 1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ShopManager.Instance.PurchaseProp(ConfManager.Instance.confMgr.propCards.PropCards.Find((e) => e.propName == ConfManager.Instance.confMgr.propCards.GetItemByTypeLevel(10, 2).propName), 1);
            }
        }

        public void Pause()
        {
            pausePanel = UIManager.Instance.PushPanel(UIPanelType.PausePanel) as PausePanel;
        }

        /// <summary>
        /// 每波结束，清理外部花园种植的植物
        /// </summary>
        public void ClearSeedCards()
        {
            while (plantSeedCard.Count > 0)
            {
                var plant = plantSeedCard[0];
                if (plant.plantAttribute.plantCard.plantType == PlantType.Gravebuster)
                {
                    GardenManager.Instance.Gravebusters.Remove(plant as Gravebuster);
                }
                plantSeedCard.RemoveAt(0);
                GameObject.Destroy(plant.gameObject);
            }
        }

        /// <summary>
        /// 周期生成外部花园植物卡片
        /// </summary>
        IEnumerator CreateSeedCard()
        {
            float loopTime = ConfManager.Instance.confMgr.gameIntParam.GetItemByKey("seedCardDefaultTime").value;
            List<int> placePlants = new List<int>();
            foreach (var item in SaveManager.Instance.externalGrowthData.plantPlace)
            {
                if (item.value != 0)
                    placePlants.Add(item.value);
            }
            loopTime -= placePlants.Count * ConfManager.Instance.confMgr.gameIntParam.GetItemByKey("seedCardReduceTime").value;
            while (true)
            {
                yield return new WaitForSeconds(loopTime);
                if (isDaytime)
                    continue;
                int random = Random.Range(0, placePlants.Count);
                var plantType = placePlants[random];
                var seedCardGo = Resources.Load<PlantSeedCard>("Prefabs/Plants/PlantSeedCard/PlantSeedCard");
                var seedCard = GameObject.Instantiate(seedCardGo);
                seedCard.plantType = plantType;
                Vector3 offset = new Vector3(Random.Range(-4, 4f), Random.Range(-2f, 1f), 0);
                seedCard.targetPos = Player.transform.position + offset;
                seedCard.transform.position = new Vector3(seedCard.targetPos.x, Player.transform.position.y + 3, seedCard.transform.position.z);
            }
        }

        public void KillZombie(DamageType damageType, Vector3 deadPos)
        {
            if (damageType == DamageType.Player)
            {
                killNum++;
                if (killNum >= 4)
                {
                    killNum = 0;
                    var vampireScepterCount = ShopManager.Instance.PurchasePropCount("vampireScepter");
                    if (vampireScepterCount > 0)
                        CreateEnergyBall(vampireScepterCount, deadPos);
                }
            }
            else if (damageType == DamageType.DeathGod)
            {
                CreateEnergyBall(1, deadPos);
            }
        }

        /// <summary>
        /// 创建增加最大生命值能量球
        /// </summary>
        void CreateEnergyBall(int addValue, Vector3 startPos)
        {
            var energyBall = Resources.Load<EnergyBall>("Prefabs/Props/EnergyBall");
            var energyBallGo = GameObject.Instantiate(energyBall);
            energyBallGo.addMaxHealthValue = addValue;
            energyBallGo.transform.position = startPos;
        }

        public void AddMaxHealth(int addValue)
        {
            UserData.MaximumHP += addValue;
            Player.Health.maxHealth += addValue;
            AddHP(addValue);
        }

        public void AddHpBarZombie(Health health, ZombieType zombieType)
        {
            if (zombieType == ZombieType.Gargantuan || zombieType == ZombieType.Boss)
            {
                if (!haveHpBarZombie.Contains(health))
                    haveHpBarZombie.Add(health);
                battlePanel.AddZombieHpBar(health, zombieType);
            }
        }

        public void RemoveHpBarZombie(Health health)
        {
            haveHpBarZombie.Remove(health);
            battlePanel.RemoveZombieHpBar(health);
        }

        public void ClearHpBar()
        {
            haveHpBarZombie.Clear();
            battlePanel.ClearZombieHpBar();
        }
    }
}
