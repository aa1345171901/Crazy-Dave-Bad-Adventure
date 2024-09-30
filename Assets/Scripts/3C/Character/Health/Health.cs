using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TopDownPlate
{
    /// <summary>
    /// 伤害类型，也可作于成就的统计
    /// </summary>
    public enum DamageType
    {
        Zombie,
        Pot,
        Hammer,
        LawnMower,
        VocalConcert,
        Fire,
        ZombieFly,
        ZombieHurEachOther,  // 游戏结束后互相伤害
        PlantBullet,  // 豌豆
        Chomper,   // 大嘴花
        Cactus,  // 香蒲
        Bomb,   // 灰烬伤害
        Cornpult,  // 玉米投手
        FumeShroom,  // 喷菇
        Gravebuster,  // 墓碑吞噬者
        ScaredyShroom,  // 胆小菇扎
        Spikeweed,    // 地刺
        TallNut,   // 高坚果
        WallNut,  // 坚果
        Squash,  // 窝瓜
    }

    public class HUDPos
    {
        public Vector3 targetPos;  // 文本的位置
        public Vector3 screenPos; // 转换的屏幕坐标
        public Vector3 guiPos; // Gui的显示位置
        public Vector3 offset;
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

        private Character character;
        private float lastInjuryTime = float.MinValue;

        private float finalArmor;
        private List<HUDPos> hudLists = new List<HUDPos>();  // 文本的位置
        private float hudShowTime = 0.5f;
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
            fontSize = (int)(scale * 20);
        }

        public void Reuse()
        {
            if (character.CharacterType == CharacterTypes.Player)
            {
                this.maxHealth = GameManager.Instance.UserData.MaximumHP;
                finalArmor = GameManager.Instance.UserData.Armor / (50f + GameManager.Instance.UserData.Armor);
                finalArmor = finalArmor > 0.9f ? 0.9f : finalArmor;
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
                health -= damage;
            }
            else
            {
                if (Time.time - lastInjuryTime < InvincibleTime)
                    return;
                if (isInvincible)
                    return;
                lastInjuryTime = Time.time;
                damage -= Mathf.RoundToInt(damage * finalArmor);
                health -= damage;
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
            }
        }

        private void SetHUD(int hudValue, bool isCriticalHit = false)
        {
            HUDPos hudPos = new HUDPos();
            if (hudValue > 0)
            {
                hudPos.isRecovery = true;
                hudPos.targetPos = this.transform.position + Vector3.up * UnityEngine.Random.Range(0.3f, 0.6f) + Vector3.left * 0.1f;
            }
            else
            {
                hudPos.targetPos = this.transform.position + Vector3.up * UnityEngine.Random.Range(0.3f, 0.6f);
            }
            hudPos.isCriticalHit = isCriticalHit;
            hudPos.hudValue = hudValue > 0 ? "+" + hudValue.ToString() : hudValue.ToString();
            hudPos.offset = Vector3.up * Time.deltaTime * UnityEngine.Random.Range(0.3f, 0.6f);
            hudLists.Add(hudPos);
            StartCoroutine(CloseHUD(hudPos));
            timer = 0;
        }

        void SetHUDText(string text)
        {
            HUDPos hudPos = new HUDPos();
            hudPos.isRecovery = true;
            hudPos.targetPos = this.transform.position + Vector3.up * UnityEngine.Random.Range(0.3f, 0.6f) + Vector3.left * 0.1f;
            hudPos.isCriticalHit = false;
            hudPos.hudValue = text;
            hudPos.offset = Vector3.up * Time.deltaTime * UnityEngine.Random.Range(0.3f, 0.6f);
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
                item.targetPos += item.offset;

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
                health += value;
            if (SaveManager.Instance.systemData.IsHUD)
                SetHUD(value);
        }
    }
}
