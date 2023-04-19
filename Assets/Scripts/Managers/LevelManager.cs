using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TopDownPlate
{
    [Serializable]
    public enum ZombieType
    {
        Normal,
        Cone,
        Bucket,
        Screendoor,
        Flag,
        Football,
        Paper,
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
        public int GenerateCount;

        [Tooltip("每20s加强波，生成数量增加多少")]
        public int CountIncrement;

        [Tooltip("该僵尸该波间隔多长时间生成一次")]
        public float IntervalTime;

        [Tooltip("每20s加强波，生成间隔时间减少多少")]
        public float TimeIncrement;

        [Tooltip("僵尸上次生成时间")]
        public float lastGenerateTime = -4;
    }

    /// <summary>
    /// 每波的怪物占比
    /// </summary>
    [Serializable]
    public struct Wave
    {
        [Tooltip("生成的僵尸的数据")]
        public List<ZombieData> zombieData;
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
    }

    [AddComponentMenu("TopDownPlate/Managers/LevelManager")]
    public class LevelManager : BaseManager<LevelManager>
    {
        [Header("Instantiate Characters")]
        [Tooltip("游戏角色")]
        public Character PlayerPrefab;

        [Space(10)]
        [Header("LevelBounds")]
        [Tooltip("关卡的区域限制")]
        public Bounds LevelBounds = new Bounds(Vector3.zero, Vector3.one * 10);

        [Tooltip("摄像机的区域限制")]
        public Bounds CameraBounds = new Bounds(Vector3.zero, Vector3.one * 10);

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

        public float DurationPerWave = 60;

        private readonly float MaxEnemyCount = 80;

        public int IndexWave { get; set; }

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
                    nowWave.zombieData[i].lastGenerateTime = -nowWave.zombieData[i].IntervalTime;
                }
            }
            course = 0;
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
                        if (timer - zombie.IntervalTime - zombie.TimeIncrement * course >= zombie.lastGenerateTime)
                        {
                            if (Enemys.Count < MaxEnemyCount)
                            {
                                zombie.lastGenerateTime = timer;
                                StartCoroutine("CreateEnemies", zombie);
                            }
                        }
                    }

                    // 如果不是最后一波，留两秒背景音乐淡出
                    if (IndexWave < waves.Count && timer > DurationPerWave - 1)
                    {
                        AudioManager.Instance.FadeOutInBackMusic();
                    }
                }
                else
                {
                    GameManager.Instance.IsDaytime = true;
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
            }
        }

        private void CreatePlayer()
        {
            if (!isCreatePlayer)
            {
                Character newPlayer = (Character)Instantiate(PlayerPrefab, PlayerPrefab.transform.position, Quaternion.identity);
                newPlayer.name = PlayerPrefab.name;
                GameManager.Instance.SetPlayer(newPlayer);
                isCreatePlayer = true;
            }
        }

        IEnumerator CreateEnemies(ZombieData zombieData)
        {
            for (int i = 0; i < zombieData.GenerateCount + zombieData.CountIncrement * course; i++)
            {
                Health health = null;
                Gravebuster gravebuster = null;
                bool isGravebusterSwallow = false;
                foreach (var item in GardenManager.Instance.Gravebusters)
                {
                    if (item.CanSwallow())
                    {
                        isGravebusterSwallow = true;
                        gravebuster = item;
                        break;
                    }
                }

                float randomX = Random.Range(LevelBounds.min.x, LevelBounds.max.x);
                float randomY = Random.Range(LevelBounds.min.y, LevelBounds.max.y);

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
        public Character GetRecentlyEnemy(out bool direction, float range = 0)
        {
            direction = false;
            if (Enemys.Count <= 0)
                return null;
            Character target = null;
            float distance = range;
            foreach (var item in Enemys)
            {
                foreach (var zombie in item.Zombies)
                {
                    AIMove aIMove = zombie.FindAbility<AIMove>();
                    if (aIMove.AIParameter.Distance < distance)
                    {
                        target = zombie;
                        distance = aIMove.AIParameter.Distance;
                        direction = aIMove.AIParameter.IsPlayerRight;
                    }
                }
            }
            return target;
        }

        public void GameOver()
        {
            foreach (var item in Enemys)
            {
                foreach (var zombie in item.Zombies)
                {
                    var move = zombie.FindAbility<AIMove>();
                    move.SetBrainPos();
                }
            }
        }
    }
}