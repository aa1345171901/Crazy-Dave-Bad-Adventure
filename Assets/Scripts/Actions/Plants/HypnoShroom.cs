using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class HypnoShroom : Plant
{
    public override PlantType PlantType => PlantType.HypnoShroom;

    public AudioSource audioSource;
    public BoxCollider2D boxCollider;

    private int finalDamage;
    private float finalPercentageDamage;
    private int finalAttackCount;
    private float finalContinueRate;

    private readonly int LevelAttackCount = 2;
    private readonly float LevelContinueRate = 0.03f;
    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 0.1f;

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    public override void Reuse()
    {
        base.Reuse();
        this.gameObject.SetActive(true);
        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalDamage = 0;
        finalPercentageDamage = 1;
        finalAttackCount = 5;
        finalContinueRate = 0;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 基础伤害
                case 0:
                    finalDamage += (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage;
                    break;
                // 1 百分比伤害
                case 1:
                    finalPercentageDamage += (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage;
                    break;
                // 攻击次数
                case 2:
                    finalAttackCount += (int)fieldInfo.GetValue(plantAttribute) * LevelAttackCount;
                    break;
                // 连续触发概率
                case 3:
                    finalContinueRate = (int)fieldInfo.GetValue(plantAttribute) * LevelContinueRate;
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var aiMove = collision.GetComponent<AIMove>();
        if (aiMove)
        {
            bool canEnchanted = true;
            if (aiMove.zombieAnimation == null)
                canEnchanted = false;
            else
            switch (aiMove.zombieAnimation.zombieType)
            {
                case ZombieType.Zamboni:
                case ZombieType.Catapult:
                case ZombieType.Gargantuan:
                    canEnchanted = false;
                    break;
                default:
                    break;
            }
            if (canEnchanted && !aiMove.IsEnchanted)
            {
                audioSource.Play();
                aiMove.BeEnchanted(finalAttackCount, finalPercentageDamage, finalDamage);
                bool canContinue = Random.Range(0, 1f) < finalContinueRate ? true : false;
                if (!canContinue)
                {
                    boxCollider.enabled = false;
                    this.spriteRenderer.enabled = false;
                    StartCoroutine(DelayHide());
                }
            }
        }
    }

    IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
    }
}
