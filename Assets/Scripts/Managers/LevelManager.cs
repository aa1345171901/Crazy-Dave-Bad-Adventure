using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    /// <summary>
    /// 生成敌人的数据，包括预制体和生成数量
    /// </summary>
    [System.Serializable]
    public struct ZombieData
    {
        [Tooltip("敌人预制体")]
        public GameObject EnemyPrefab;

        [Tooltip("一次生成敌人数量")]
        public float GenerateCount;

        [Tooltip("每20s加强波，生成数量增加多少")]
        public float CountIncrement;
    }

    /// <summary>
    /// 每波的怪物占比
    /// </summary>
    [System.Serializable]
    public struct Wave
    {
        [Tooltip("生成的僵尸的数据")]
        public List<ZombieData> zombieData;

        [Tooltip("该波间隔多长时间生成一次")]
        public float IntervalTime;

        [Tooltip("每20s加强波，生成间隔时间减少多少")]
        public float TimeIncrement;
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
        public List<Character> Enemys;

        [ReadOnly]
        public List<Character> CacheEnemys; // 敌人死后的对象池

        private bool isCreatePlayer = false;
        private float lastGenerateTime;
        private float timer;   // 每波时间计时
        private float intervalTime;   // 间隔时长
        private int course; // 时间进程

        private readonly float DurationPerWave = 60;

        public int IndexWave { get; set; }

        public void Init()
        {
            CreatePlayer();
            timer = 0;
            intervalTime = waves[IndexWave].IntervalTime;
            lastGenerateTime = -intervalTime;
            course = 0;
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
                        intervalTime += waves[IndexWave].TimeIncrement;
                        course++;
                        GameManager.Instance.ShowTipsPanel(course == 1 ? TipsType.Approaching : TipsType.FinalWave);
                        AudioManager.Instance.PlayEffectSoundByName(course == 1 ? "approachingWave" : "finalWave");
                    }

                    if (timer - intervalTime >= lastGenerateTime)
                    {
                        CreateEnemies();
                        lastGenerateTime = timer;
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
                    // 该波攻势结束，僵尸退场
                    if (Enemys.Count != 0)
                    {
                        foreach (var item in Enemys)
                        {
                            ZombieAnimation zombieAnimation = item.GetComponentInChildren<ZombieAnimation>();
                            if (zombieAnimation != null)
                            {
                                zombieAnimation.WalkOff();
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

        private void CreateEnemies()
        {
            List<ZombieData> zombieData = waves[IndexWave].zombieData;
            foreach (var item in zombieData)
            {
                for (int i = 0; i < item.GenerateCount + item.CountIncrement * course; i++)
                {
                    float randomX = Random.Range(LevelBounds.min.x, LevelBounds.max.x);
                    float randomY = Random.Range(LevelBounds.min.y, LevelBounds.max.y);

                    bool cacheUsed = false;

                    // 重置对象池中的物体
                    if (CacheEnemys.Count > 0)
                    {
                        var go = CacheEnemys[0];
                        ZombieAnimation zombieAnimation = go.GetComponentInChildren<ZombieAnimation>();
                        if (zombieAnimation != null)
                        {
                            CacheEnemys.RemoveAt(0);
                            zombieAnimation.Reuse();
                            go.transform.position = new Vector3(randomX, randomY, 0);
                            cacheUsed = true;
                        }
                    }

                    // 没有使用到对象池才实例化
                    if (!cacheUsed)
                        Instantiate(item.EnemyPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
                }
            }
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
                AIMove aIMove = item.FindAbility<AIMove>();
                if (aIMove.AIParameter.Distance < distance)
                {
                    target = item;
                    distance = aIMove.AIParameter.Distance;
                    direction = aIMove.AIParameter.IsPlayerRight;
                }
            }
            return target;
        }

        public void GameOver()
        {
            foreach (var item in Enemys)
            {
                var move = item.FindAbility<AIMove>();
                move.SetBrainPos();
            }
        }
    }
}