using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace TopDownPlate
{
    /// <summary>
    /// 伤害类型，也可作于成就的统计
    /// </summary>
    public enum DamageType
    {
        /// <summary>
        /// 僵尸
        /// </summary>
        Zombie,
        /// <summary>
        /// 角色的攻击
        /// </summary>
        Player,
        /// <summary>
        /// 木槌
        /// </summary>
        Hammer,
        /// <summary>
        /// 小推车
        /// </summary>
        LawnMower,
        /// <summary>
        /// 音乐会
        /// </summary>
        VocalConcert,
        /// <summary>
        /// 火焰
        /// </summary>
        Fire,
        /// <summary>
        /// 僵尸残骸
        /// </summary>
        ZombieFly,
        /// <summary>
        /// 游戏结束后互相伤害
        /// </summary>
        ZombieHurEachOther,
        /// <summary>
        /// 豌豆
        /// </summary>
        PeaBullet,
        /// <summary>
        /// 杨桃子弹
        /// </summary>
        StarBullet,
        /// <summary>
        /// 胆小菇，小喷菇毒弹
        /// </summary>
        ShroomBullet,
        /// <summary>
        /// 寒冰豌豆
        /// </summary>
        SnowBullet,
        /// <summary>
        /// 大嘴花
        /// </summary>
        Chomper,
        /// <summary>
        /// 香蒲和仙人掌的刺
        /// </summary>
        Spike,
        /// <summary>
        /// 灰烬伤害
        /// </summary>
        Bomb,
        /// <summary>
        /// 玉米投手
        /// </summary>
        Cornpult,
        /// <summary>
        /// 大喷菇，曾哥
        /// </summary>
        FumeShroom,
        /// <summary>
        /// 墓碑吞噬者
        /// </summary>
        Gravebuster,
        /// <summary>
        /// 胆小菇扎
        /// </summary>
        ScaredyShroom,
        /// <summary>
        /// 地刺
        /// </summary>
        Spikeweed,
        /// <summary>
        /// 高坚果
        /// </summary>
        TallNut,
        /// <summary>
        /// 坚果
        /// </summary>
        WallNut,
        /// <summary>
        /// 窝瓜
        /// </summary>
        Squash,
        /// <summary>
        /// 臭屁
        /// </summary>
        SmellyFart,
        /// <summary>
        /// 火精灵
        /// </summary>
        FireElf,
        /// <summary>
        /// 水精灵
        /// </summary>
        WaterElf,
        /// <summary>
        /// 乌云
        /// </summary>
        DarkCloud,
        /// <summary>
        /// 死神
        /// </summary>
        DeathGod,
        /// <summary>
        /// 枪
        /// </summary>
        Gun,
    }

    public class HUDPos
    {
        public Vector3 originPos;  // 文本的位置
        public Vector3 targetPos;  // 文本的位置
        public Vector3 screenPos; // 转换的屏幕坐标
        public Vector3 guiPos; // Gui的显示位置
        public Vector3 offset;

        public float curTime;
        public float time;
        public float height;

        public string hudValue;
        public bool isRecovery;
        public bool isCriticalHit;
    }

    [AddComponentMenu("TopDownPlate/Character/Health/Health")]
    public class Health : MonoBehaviour
    {
        [Header("HealthParameter")]
        [Tooltip("当前生命值")]
        public int health = 1;
        [Tooltip("最大生命值")]
        public int maxHealth = 1;
        [Tooltip("无敌时间")]
        public float InvincibleTime = 0.5f;

        [Space(10)]
        [Header("Event")]
        [Tooltip("受伤事件")]
        public UnityEvent<DamageType> Injured;
        [Tooltip("死亡事件")]
        public UnityEvent<DamageType> Dead;   // DamageType判断伤害来源
        public Action DeadAction;
        public Action InjuredAction;

        private Character character;
        private float lastInjuryTime = float.MinValue;

        private List<HUDPos> hudLists = new List<HUDPos>();  // 文本的位置
        private float hudShowTime = 2f;
        private int fontSize;

        private float timer;
        private int defaultMaxHealth;

        /// <summary>
        /// 是否无敌
        /// </summary>
        public bool isInvincible { get; set; }

        private void Start()
        {
            character = this.GetComponent<Character>();
            defaultMaxHealth = maxHealth;
            Reuse();
            float scale = Screen.height / 1080;
            fontSize = (int)(scale * 28);
        }

        public void Reuse()
        {
            if (character.CharacterType == CharacterTypes.Player)
            {
                this.maxHealth = GameManager.Instance.UserData.MaximumHP;
            }
            else
            {
                int waveIndex = LevelManager.Instance.IndexWave + 1;
                if (waveIndex < 5)
                {
                    this.maxHealth = defaultMaxHealth;
                }
                else if (waveIndex < 9)
                {
                    this.maxHealth = (int)(defaultMaxHealth * (waveIndex / 3f));
                }
                else if (waveIndex < 13)
                {
                    this.maxHealth = (int)(defaultMaxHealth * waveIndex * 2);
                }
                else if (waveIndex < 17)
                {
                    this.maxHealth = (int)(defaultMaxHealth * waveIndex * 4f);
                }
                else if (waveIndex < 21)
                {
                    this.maxHealth = defaultMaxHealth * waveIndex * 6;
                }
                else
                {
                    this.maxHealth = (int)(defaultMaxHealth * waveIndex * 20f);
                }
            }
            this.health = maxHealth;
        }

        public void DoDamage(int damage, DamageType damageType = DamageType.Zombie, bool isCriticalHit = false)
        {
            if (health <= 0)
            {
                if (!character.IsDead)
                {
                    character.IsDead = true;
                    Dead?.Invoke(damageType);
                }
                return;
            }
            if (damageType == DamageType.Fire)
            {
                if (health > 0)
                    health -= damage;
            }
            else
            {
                if (Time.time - lastInjuryTime < InvincibleTime)
                    return;
                if (isInvincible)
                    return;
                lastInjuryTime = Time.time;
                float finalArmor = 0;
                if (character.CharacterType == CharacterTypes.Player)
                {
                    finalArmor = GameManager.Instance.UserData.Armor / (50f + GameManager.Instance.UserData.Armor);
                    finalArmor = finalArmor > 0.9f ? 0.9f : finalArmor;
                }
                damage -= Mathf.RoundToInt(damage * finalArmor);
                health -= damage;
            }

            if (character != GameManager.Instance.Player)
            {
                SaveManager.Instance.AddDamageValue((int)damageType, damage);
            }
            else if (damageType == DamageType.Fire || damageType == DamageType.DeathGod)
            {
                SaveManager.Instance.AddTakingDamageValue((int)damageType + 1000, damage);
            }

            // 伤害hud
            if (SaveManager.Instance.systemData.IsHUD && gameObject.activeSelf)
                SetHUD(-damage, isCriticalHit);

            if (health <= 0)
            {
                void OnDead()
                {
                    character.IsDead = true;
                    Dead?.Invoke(damageType);
                    DeadAction?.Invoke();
                }
                if (character == GameManager.Instance.Player)
                {
                    // 南瓜头复活 优先，南瓜头每波
                    if (GameManager.Instance.pumpkinHead.HasPumpkinHead)
                    {
                        SetHUDText(GameTool.LocalText("grow_text"));
                        GameManager.Instance.pumpkinHead.HasPumpkinHead = false;
                        GameManager.Instance.pumpkinHead.gameObject.SetActive(false);
                        health = 0;
                        GameManager.Instance.AddHP((int)(character.Health.maxHealth * GameManager.Instance.pumpkinHead.PumpkinHeadResumeLife));
                    }
                    // 局外成长复活，总共次数，不是每波
                    else if (GameManager.Instance.resurrection < SaveManager.Instance.externalGrowthData.GetGrowSumValueByKey("resurrection"))
                    {
                        SetHUDText(GameTool.LocalText("grow_text"));
                        GameManager.Instance.resurrection++;
                        health = 0;
                        GameManager.Instance.AddHP(character.Health.maxHealth / 2 + 1);
                    }
                    else
                    {
                        OnDead();
                    }
                }
                else
                {
                    OnDead();
                }
            }
            else
            {
                Injured?.Invoke(damageType);
                InjuredAction?.Invoke();
            }
        }

        private void SetHUD(int hudValue, bool isCriticalHit = false)
        {
            HUDPos hudPos = new HUDPos();
            if (hudValue > 0)
            {
                hudPos.isRecovery = true;
                hudPos.originPos = this.transform.position + Vector3.up * UnityEngine.Random.Range(0.75f, 1.25f) + Vector3.left * 0.1f;
            }
            else
            {
                hudPos.originPos = this.transform.position + Vector3.up * UnityEngine.Random.Range(0.75f, 1.25f);
            }
            hudPos.isCriticalHit = isCriticalHit;
            hudPos.hudValue = hudValue > 0 ? "+" + hudValue.ToString() : hudValue.ToString();

            var offset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), hudPos.originPos.z);
            hudPos.height = Random.Range(0.15f, 0.3f);
            hudPos.time = Random.Range(0.3f, 0.5f);
            hudPos.offset = offset / hudPos.time;

            hudLists.Add(hudPos);
            StartCoroutine(CloseHUD(hudPos));
            timer = 0;
        }

        public void SetHUDText(string text)
        {
            HUDPos hudPos = new HUDPos();
            hudPos.isRecovery = true;
            hudPos.originPos = this.transform.position + Vector3.up * UnityEngine.Random.Range(0.75f, 1.25f) + Vector3.left * 0.1f;
            hudPos.isCriticalHit = false;
            hudPos.hudValue = text;

            var offset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), hudPos.originPos.z);
            hudPos.height = Random.Range(0.15f, 0.3f);
            hudPos.time = Random.Range(0.3f, 0.5f);
            hudPos.offset = offset / hudPos.time;

            hudLists.Add(hudPos);
            StartCoroutine(CloseHUD(hudPos));
            timer = 0;
        }

        IEnumerator CloseHUD(HUDPos hudPos)
        {
            yield return new WaitForSeconds(hudShowTime);
            hudLists.Remove(hudPos);
        }

        private void Update()
        {
            foreach (var item in hudLists)
            {
                item.curTime += Time.deltaTime;
                if (item.curTime < item.time)
                {
                    Vector3 newPos = item.originPos + item.offset * item.curTime;
                    newPos.y += Mathf.Cos(item.curTime / item.time * Mathf.PI - Mathf.PI / 2) * item.height;
                    item.targetPos = newPos;
                }

                // 屏幕坐标左上为0，0 右下为 screen.width, height
                item.screenPos = Camera.main.WorldToScreenPoint(item.targetPos);

                // gui坐标左下为0，0 右上为 screen.width, height
                item.guiPos = new Vector2(item.screenPos.x, Screen.height - item.screenPos.y);
            }
        }


        private void OnGUI()
        {
            foreach (var item in hudLists)
            {
                if (item.isCriticalHit)
                    GUI.color = Color.yellow;
                else
                    GUI.color = Color.white;
                if (character.CharacterType == CharacterTypes.Player)
                    GUI.color = Color.red;
                if (item.isRecovery)
                    GUI.color = Color.green;
                GUI.skin.label.fontSize = fontSize;
                GUI.skin.label.font = GameManager.Instance.HUDFont;
                GUI.Label(new Rect(item.guiPos.x, item.guiPos.y, Screen.width, Screen.height), item.hudValue);
            }
            if (hudLists.Count >= 0)
            {
                timer += Time.deltaTime;
                if (timer > hudShowTime)
                    hudLists.Clear();
            }
        }

        public void AddHealth(int value)
        {
            if (health + value > maxHealth)
                health = maxHealth;
            else
            {
                if (SaveManager.Instance.systemData.IsHUD)
                    SetHUD(value);
                health += value;
            }
        }
    }
}
