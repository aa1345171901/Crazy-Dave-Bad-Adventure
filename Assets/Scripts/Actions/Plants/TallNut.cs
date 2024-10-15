using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class TallNut : Plant
{
    public override PlantType PlantType => PlantType.TallNut;

    [Tooltip("生命值")]
    public int Health = 30;

    public LayerMask TargetLayer;
    public LayerMask TargetAttackLayer;

    public ParticleSystem boom;

    public AudioSource audioSource;
    public AudioClip injuryClip;
    public AudioClip boomClip;

    private float finalBoomRate;
    private float finalCounterInjury;
    private float finalCounterInjuryRate;
    public int finalHealth;
    private int maxHealth;

    private readonly float LevelRate = 0.03f;
    private readonly float LevelHealth = 0.2f;
    private readonly float LevelCounterInjury = 0.1f;
    private readonly float LevelCounterInjuryRate = 0.04f;

    private readonly float BoomDamage = 0.5f;
    private readonly float defaultHealthMul = 0.5f;

    private Dictionary<Character, float> zombieDict = new Dictionary<Character, float>();
    private float coolTimer = 1; //  每只僵尸单独设置无敌时间

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    private void OnDestroy()
    {
        AudioManager.Instance.AudioLists.Remove(this.audioSource);
    }

    public override void Reuse(bool randomPos = true)
    {
        base.Reuse(randomPos);

        this.gameObject.SetActive(true);
        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalBoomRate = 0;
        finalCounterInjury = 1;
        finalCounterInjuryRate = 0.1f;
        finalHealth = Health + (int)(defaultHealthMul * GameManager.Instance.UserData.Botany);
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 最大生命值
                case 0:
                    finalHealth += (int)((int)fieldInfo.GetValue(plantAttribute) * LevelHealth * GameManager.Instance.UserData.Botany);
                    break;
                // 爆炸概率
                case 1:
                    finalBoomRate += (int)fieldInfo.GetValue(plantAttribute) * LevelRate;
                    break;
                // 反伤伤害
                case 2:
                    finalCounterInjury += (int)fieldInfo.GetValue(plantAttribute) * LevelCounterInjury;
                    break;
                // 反伤概率
                case 3:
                    finalCounterInjuryRate += (int)fieldInfo.GetValue(plantAttribute) * LevelCounterInjuryRate;
                    break;
                default:
                    break;
            }
        }
        animator.SetBool("Idel1", false);
        animator.SetBool("Idel2", false);
        this.maxHealth = finalHealth;
        audioSource.clip = injuryClip;
        GardenManager.Instance.TallNuts.Add(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (TargetLayer.Contains(collision.gameObject.layer))
        {
            var character = collision.GetComponent<Character>();
            if (character)
            {
                if (zombieDict.ContainsKey(character))
                {
                    if (Time.time - zombieDict[character] > coolTimer)
                    {
                        BeAttack(character, character.FindAbility<CollisionAttack>().realDamage);
                        zombieDict[character] = Time.time;
                    }
                }
                else
                {
                    BeAttack(character, character.FindAbility<CollisionAttack>().realDamage);
                    zombieDict.Add(character, Time.time);
                }
            }
        }

        if (TargetAttackLayer.Contains(collision.gameObject.layer))
        {
            var character = collision.GetComponentInParent<Character>();
            if (character)
            {
                if (zombieDict.ContainsKey(character))
                {
                    if (Time.time - zombieDict[character] > coolTimer)
                    {
                        BeAttack(character, character.FindAbility<AIAttack>().realDamage);
                        zombieDict[character] = Time.time;
                    }
                }
                else
                {
                    BeAttack(character, character.FindAbility<AIAttack>().realDamage);
                    zombieDict.Add(character, Time.time);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TargetLayer.Contains(collision.gameObject.layer))
        {
            var character = collision.GetComponent<Character>();
            if (zombieDict.ContainsKey(character))
                zombieDict.Remove(character);
        }
    }

    private void BeAttack(Character character, int damage)
    {
        this.finalHealth -= damage;
        if (finalHealth <= 0)
        {
            // 爆炸
            if (Random.Range(0, 1f) < finalBoomRate)
            {
                audioSource.clip = boomClip;
                audioSource.Play();
                boom.Play();
                var colliders = Physics2D.OverlapCircleAll(this.transform.position, 2, TargetLayer);
                foreach (var item in colliders)
                {
                    if (item.isTrigger)
                    {
                        var health = item.GetComponent<Health>();
                        health.DoDamage((int)(this.maxHealth * BoomDamage), DamageType.TallNut);
                    }
                }
                Invoke("DelayActive", 1f);
            }
            else
            {
                DelayActive();
            }
        }
        else
        {
            if (finalHealth < maxHealth / 3)
            {
                animator.SetBool("Idel1", false);
                animator.SetBool("Idel2", true);
            }
            else if (finalHealth < 2 * maxHealth / 3)
            {
                animator.SetBool("Idel1", true);
            }

            audioSource.Play();

            // 反伤
            if (Random.Range(0, 1f) < finalCounterInjuryRate)
            {
                character.Health.DoDamage((int)(damage * finalCounterInjury), DamageType.TallNut);
            }
        }
    }

    private void DelayActive()
    {
        GardenManager.Instance.TallNuts.Remove(this);
        this.gameObject.SetActive(false);
    }
}
