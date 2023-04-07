using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TopDownPlate
{
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
        PeaBullet,
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
        public float InvincibleTime = 0.1f;

        [Space(10)]
        [Header("Event")]
        [Tooltip("受伤事件")]
        public UnityEvent Injured;   // bool 传递是否暴击
        [Tooltip("死亡事件")]
        public UnityEvent<DamageType> Dead;   // DamageType判断伤害来源

        private Character character;
        private float lastInjuryTime = float.MinValue;

        private float finalArmor;
        private List<HUDPos> hudLists = new List<HUDPos>();  // 文本的位置
        private float hudShowTime = 0.5f;
        private int fontSize;

        private int defaultMaxHealth;

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
                    this.maxHealth = (int)(defaultMaxHealth * waveIndex);
                }
                else if (waveIndex < 17)
                {
                    this.maxHealth = (int)(defaultMaxHealth * waveIndex * 1.5f);
                }
                else if (waveIndex < 21)
                {
                    this.maxHealth = defaultMaxHealth * waveIndex * 3;
                }
                else
                {
                    this.maxHealth = (int)(defaultMaxHealth * waveIndex * 5f);
                }
            }
            this.health = maxHealth;
        }

        public void DoDamage(int damage, DamageType damageType = DamageType.Zombie, bool isCriticalHit = false)
        {
            if (health <= 0)
                return;
            if (damageType == DamageType.Fire)
            {
                health -= damage;
            }
            else
            {
                if (Time.time - lastInjuryTime < InvincibleTime)
                    return;
                lastInjuryTime = Time.time;
                damage -= Mathf.RoundToInt(damage * finalArmor);
                health -= damage;
            }

            if (health <= 0)
            {
                character.IsDead = true;
                Dead?.Invoke(damageType);
            }
            else
            {
                Injured?.Invoke();
            }

            // 伤害hud
            if (SaveManager.Instance.SystemData.IsHUD)
                SetHUD(-damage, isCriticalHit);
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
        }

        public void AddHealth(int value)
        {
            if (health + value > maxHealth)
                health = maxHealth;
            else
            {
                health += value;
                if (SaveManager.Instance.SystemData.IsHUD)
                    SetHUD(value);
            }
        }
    }
}
