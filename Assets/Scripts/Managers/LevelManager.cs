﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TopDownPlate
{
    [Serializable]
    public enum ZombieType
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal,
        /// <summary>
        /// 路障
        /// </summary>
        Cone,
        /// <summary>
        /// 铁桶
        /// </summary>
        Bucket,
        /// <summary>
        /// 铁栅栏
        /// </summary>
        Screendoor,
        /// <summary>
        /// 旗帜
        /// </summary>
        Flag,
        /// <summary>
        /// 橄榄球
        /// </summary>
        Football,
        /// <summary>
        /// 读报
        /// </summary>
        Paper,
        /// <summary>
        /// 撑杆跳
        /// </summary>
        Polevaulter,
        /// <summary>
        /// 气球
        /// </summary>
        Balloon,
        /// <summary>
        /// 冰车
        /// </summary>
        Zamboni,
        /// <summary>
        /// 投篮
        /// </summary>
        Catapult,
        /// <summary>
        /// 巨人
        /// </summary>
        Gargantuan,
        /// <summary>
        /// 僵王
        /// </summary>
        Boss,
    }

    /// <summary>
    /// 生成敌人的数据，包括预制体和生成数量
    /// </summary>
    [Serializable]
    public class ZombieData
    {
        [Tooltip("敌人种类")]
        public ZombieType ZombieType;

        [Tooltip("敌人预制体")]
        public GameObject EnemyPrefab;

        [Tooltip("一次生成敌人数量")]
        public List<int> GenerateCount;

        [Tooltip("该僵尸该波间隔多长时间生成一次")]
        public List<float> IntervalTime;

        [Tooltip("僵尸上次生成时间")]
        public float lastGenerateTime = -4;
    }

    /// <summary>
    /// 每波的怪物占比
    /// </summary>
    [Serializable]
    public class Wave
    {
        [Tooltip("生成的僵尸的数据")]
        public List<ZombieData> zombieData;

        [Tooltip("该波时间")]
        public float DurationPerWave = 60;
    }

    /// <summary>
    /// 僵尸种类与对应游戏物体的集合
    /// </summary>
    [Serializable]
    public struct ZombieDicts
    {
        public ZombieDicts(ZombieType zombieType, List<Character> zombies)
        {
            this.ZombieType = zombieType;
            this.Zombies = zombies;
        }

        public ZombieType ZombieType;
        public List<Character> Zombies;
    }

    public static class ZombieDictsExpand
    {
        public static void Add(this List<ZombieDicts> list, ZombieType zombieType, Character zombie)
        {
            bool contains = false;
            foreach (var item in list)
            {
                if (item.ZombieType == zombieType)
                {
                    item.Zombies.Add(zombie);
                    contains = true;
                    break;
                }
            }
            if (!contains)
            {
                list.Add(new ZombieDicts(zombieType, new List<Character>() { zombie }));
            }
        }

        public static void Remove(this List<ZombieDicts> list, ZombieType ZombieType, Character zombie)
        {
            foreach (var item in list)
            {
                if (item.ZombieType == ZombieType)
                {
                    item.Zombies.Remove(zombie);
                    break;
                }
            }
        }

        public static ZombieDicts Get(this List<ZombieDicts> list, ZombieType ZombieType)
        {
            ZombieDicts zombieDicts = new ZombieDicts(ZombieType, new List<Character>());
            foreach (var item in list)
            {
                if (item.ZombieType == ZombieType)
                {
                    zombieDicts = item;
                    break;
                }
            }
            return zombieDicts;
        }

        public static int Count(this List<ZombieDicts> list)
        {
            int count = 0;
            foreach (var item in list)
            {
                count += item.Zombies.Count;
            }
            return count;
        }
    }

    [AddComponentMenu("TopDownPlate/Managers/LevelManager")]
    public class LevelManager : BaseManager<LevelManager>
    {
        [Space(10)]
        [Header("LevelBounds")]
        [Tooltip("关卡的区域限制")]
        public Bounds LevelBounds = new Bounds(Vector3.zero, Vector3.one * 10);

        [Tooltip("摄像机的区域限制")]
        public Bounds CameraBounds = new Bounds(Vector3.zero, Vector3.one * 10);

        public List<GameObject> PrefabList;

        [ReadOnly]
        [Tooltip("每波僵尸生成的数据")]
        public List<Wave> waves;

        [ReadOnly]
        public List<ZombieDicts> Enemys;

        [ReadOnly]
        public List<ZombieDicts> CacheEnemys; // 敌人死后的对象池

        [ReadOnly]
        public List<ZombieDicts> EnchantedEnemys; // 被魅惑的敌人

        private bool isCreatePlayer = false;
        private float timer;   // 每波时间计时
        private int course; // 时间进程

        private Wave nowWave;

        /// <summary>
        /// 僵尸增量
        /// </summary>
        private float zombieIncrement;

        public float DurationPerWave = 60;

        private readonly float MaxEnemyCount = 100;

        public int IndexWave;

        protected override void Initialize()
        {
            base.Initialize();
            waves.Clear();
            foreach (var confWave in ConfManager.Instance.confMgr.waveTimer.items)
            {
                var waveData = new Wave();
                if (confWave.waveTime == 0)
                    waveData.DurationPerWave = int.MaxValue;
                else
                    waveData.DurationPerWave = confWave.waveTime;
                waveData.zombieData = new List<ZombieData>();
                var confZombieWave = ConfManager.Instance.confMgr.wave.waves[confWave.id];
                foreach (var confZombie in confZombieWave)
                {
                    var zombieData = new ZombieData();
                    zombieData.ZombieType = (ZombieType)confZombie.zombieType;
                    zombieData.EnemyPrefab = PrefabList[confZombie.zombieType];
                    zombieData.GenerateCount = confZombie.generateCount.ToList();
                    zombieData.IntervalTime = confZombie.intervalTime.ToList();
                    zombieData.lastGenerateTime = confZombie.firstGenerateTime - confZombie.intervalTime[0];
                    waveData.zombieData.Add(zombieData);
                }
                waves.Add(waveData);
            }
        }

        public void Init()
        {
            CreatePlayer();
            timer = 0;
            if (IndexWave < waves.Count)
                nowWave = waves[IndexWave];
            else
            {
                nowWave = waves[waves.Count - 1];
                for (int i = 0; i < nowWave.zombieData.Count; i++)
                {
                    nowWave.zombieData[i].lastGenerateTime = -nowWave.zombieData[i].IntervalTime[0];
                }
            }
            course = 0;
            DurationPerWave = nowWave.DurationPerWave;

            // 计算僵尸增量
            float increment = SaveManager.Instance.externalGrowthData.GetGrowSumValueByKey("curse");  // 成就诅咒增量
            increment += ShopManager.Instance.GetPurchaseTypeList(PropType.ZombieChange).Sum((e)=> e.value1); // 道具的增量
            increment /= 100f;
            zombieIncrement = Mathf.Atan(increment);  // 无限趋近 1.5f
        }

        /// <summary>
        /// 初始化其他模式
        /// </summary>
        public void InitOtherGameModes(int id)
        {
            CreatePlayer();
            timer = 0;
            course = 0;

            var confItem = ConfManager.Instance.confMgr.otherGameModeWavesParam.GetItemById(id);
            if (confItem != null)
            {
                nowWave = new Wave();
                if (confItem.waveTime == 0)
                    nowWave.DurationPerWave = int.MaxValue;
                else
                    nowWave.DurationPerWave = confItem.waveTime;
                nowWave.zombieData = new List<ZombieData>();
                var confZombieWave = ConfManager.Instance.confMgr.otherGameModeWaves.GetListsById(id);
                foreach (var confZombie in confZombieWave)
                {
                    var zombieData = new ZombieData();
                    zombieData.ZombieType = (ZombieType)confZombie.zombieType;
                    zombieData.EnemyPrefab = PrefabList[confZombie.zombieType];
                    zombieData.GenerateCount = confZombie.generateCount.ToList();
                    zombieData.IntervalTime = confZombie.intervalTime.ToList();
                    zombieData.lastGenerateTime = confZombie.firstGenerateTime - confZombie.intervalTime[0];
                    nowWave.zombieData.Add(zombieData);
                }
                IEnumerator DelayAddIndex()
                {
                    while (true)
                    {
                        yield return new WaitForSeconds(confItem.enhanceTime);
                        IndexWave++;
                    }
                }
                if (confItem.enhanceTime > 0)
                    StartCoroutine(DelayAddIndex());
            }
            else
            {
                nowWave = waves[0];
            }
            DurationPerWave = nowWave.DurationPerWave;
        }

        public void LoadTimer()
        {
            timer = DurationPerWave + 1;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsEnd)
            {
                if (timer < DurationPerWave)
                {
                    timer += Time.deltaTime;
                    GameManager.Instance.SetProgressSlider(timer / DurationPerWave);

                    if ((timer >= DurationPerWave / 3 && course == 0) || (timer >= DurationPerWave * 2 / 3 && course == 1))
                    {
                        course++;
                        GameManager.Instance.ShowTipsPanel(course == 1 ? TipsType.Approaching : TipsType.FinalWave);
                        AudioManager.Instance.PlayEffectSoundByName(course == 1 ? "approachingWave" : "finalWave");
                    }

                    // 遍历该波僵尸
                    for (int i = 0; i < nowWave.zombieData.Count; i++)
                    {
                        var zombie = nowWave.zombieData[i];
                        var realIntervalTime = zombie.IntervalTime[course];
                        // 受到诅咒影响，间隔时间缩短
                        realIntervalTime -= (realIntervalTime / 2) * zombieIncrement;
                        if (timer - realIntervalTime >= zombie.lastGenerateTime)
                        {
                            if (zombie.ZombieType == ZombieType.Gargantuan)
                            if (Enemys.Count() < MaxEnemyCount)
                            {
                                zombie.lastGenerateTime = timer;
                                StartCoroutine("CreateEnemies", zombie);
                            }
                        }
                    }

                    // 如果不是最后一波，留两秒背景音乐淡出
                    if (timer > DurationPerWave - 1)
                    {
                        AudioManager.Instance.FadeOutInBackMusic();
                    }
                }
                else
                {
                    switch (SaveManager.Instance.specialData.battleMode)
                    {
                        case BattleMode.None:
                            GameManager.Instance.IsDaytime = true;
                            WalkOff();
                            break;
                        case BattleMode.PropMode:
                        case BattleMode.PlantMode:
                        case BattleMode.PlayerMode:
                            GameManager.Instance.Victory();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void WalkOff()
        {
            if (EnchantedEnemys.Count > 0)
            {
                Enemys.AddRange(EnchantedEnemys);
                EnchantedEnemys.Clear();
            }
            // 该波攻势结束，僵尸退场
            if (Enemys.Count != 0)
            {
                foreach (var item in Enemys)
                {
                    foreach (var zombie in item.Zombies)
                    {
                        ZombieAnimation zombieAnimation = zombie.GetComponentInChildren<ZombieAnimation>();
                        if (zombieAnimation != null)
                        {
                            zombieAnimation.WalkOff();
                        }
                    }
                }
                Enemys.Clear();
            }
        }

        private void CreatePlayer()
        {
            if (!isCreatePlayer)
            {
                var playerPrefab = Resources.Load<Character>("Prefabs/Player/" + GameManager.Instance.UserData.characterName);
                Character newPlayer = (Character)Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
                newPlayer.name = playerPrefab.name;
                GameManager.Instance.SetPlayer(newPlayer);
                isCreatePlayer = true;
            }
        }

        IEnumerator CreateEnemies(ZombieData zombieData)
        {
            for (int i = 0; i < zombieData.GenerateCount[course]; i++)
            {
                Health health = null;
                Gravebuster gravebuster = null;
                bool isGravebusterSwallow = false;
                // 不从墓碑出的排除
                if (zombieData.ZombieType != ZombieType.Boss && zombieData.ZombieType != ZombieType.Gargantuan && zombieData.ZombieType != ZombieType.Catapult && zombieData.ZombieType != ZombieType.Balloon)
                    foreach (var item in GardenManager.Instance.Gravebusters)
                    {
                        if (item.CanSwallow())
                        {
                            isGravebusterSwallow = true;
                            gravebuster = item;
                            break;
                        }
                    }

                float randomX = LevelBounds.max.x;
                float randomY = 0;
                if (zombieData.ZombieType != ZombieType.Boss)
                {
                    randomX = Random.Range(LevelBounds.min.x, LevelBounds.max.x);
                    randomY = Random.Range(LevelBounds.min.y, LevelBounds.max.y);
                    // 0.5 刚好站在格子上
                    if (zombieData.ZombieType == ZombieType.Zamboni)
                        randomY = (int)Random.Range(LevelBounds.min.y, LevelBounds.max.y - 0.5f) + 0.5f;

                    if (zombieData.ZombieType == ZombieType.Gargantuan)
                        randomY = 1000;
                }

                bool cacheUsed = false;
                // 重置对象池中的物体
                var targetCacheEnemy = CacheEnemys.Get(zombieData.ZombieType).Zombies;
                if (targetCacheEnemy.Count > 0)
                {
                    var go = targetCacheEnemy[0];
                    ZombieAnimation zombieAnimation = go.GetComponentInChildren<ZombieAnimation>();
                    if (zombieAnimation != null)
                    {
                        targetCacheEnemy.RemoveAt(0);
                        if (!isGravebusterSwallow)
                            zombieAnimation.Reuse();
                        go.transform.position = new Vector3(randomX, randomY, 0);
                        cacheUsed = true;
                        health = go.Health;
                    }
                }

                // 没有使用到对象池才实例化
                if (!cacheUsed)
                {
                    var go = Instantiate(zombieData.EnemyPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
                    health = go.GetComponent<Health>();
                }

                // 墓碑吞噬
                if (isGravebusterSwallow && health != null)
                {
                    StartCoroutine("DelayDoDamage", health);
                    gravebuster.gameObject.SetActive(true);
                    gravebuster.transform.position = new Vector3(randomX, randomY + 1, 0);
                    gravebuster.SetLayer(randomY, health.maxHealth);
                }
                GameManager.Instance.AddHpBarZombie(health, zombieData.ZombieType);
                SaveManager.Instance.externalGrowthData.AddZombieCount((int)zombieData.ZombieType);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            }
        }

        IEnumerator DelayDoDamage(Health health)
        {
            yield return new WaitForSeconds(0.1f);
            health.DoDamage(health.maxHealth, DamageType.Gravebuster);
        }

        /// <summary>
        /// 获取最近敌人
        /// </summary>
        /// <param name="distance">在距离内才返回</param>
        /// <returns></returns>
        public Character GetRecentlyEnemy(out bool direction, float range, bool haveAlloyEye)
        {
            direction = false;
            if (Enemys.Count() <= 0)
                return null;
            Character target = null;
            float distance = range;
            foreach (var item in Enemys)
            {
                // 平底锅不能打到气球僵尸
                if (item.ZombieType != ZombieType.Balloon || haveAlloyEye)
                {
                    foreach (var zombie in item.Zombies)
                    {
                        IAIMove aIMove = zombie.FindAbility<IAIMove>();
                        if (aIMove.AIParameter.Distance < distance)
                        {
                            target = zombie;
                            distance = aIMove.AIParameter.Distance;
                            direction = aIMove.AIParameter.IsPlayerRight;
                        }
                    }
                }
            }
            return target;
        }

        public void GameOver()
        {
            SaveManager.Instance.SaveExternalGrowData();
            AchievementManager.Instance.SaveData();
            foreach (var item in Enemys)
            {
                foreach (var zombie in item.Zombies)
                {
                    var move = zombie.FindAbility<AIMove>();
                    if (move)
                        move.SetBrainPos();
                }
            }
        }

        public void Victory()
        {
            WalkOff();
        }
    }
}