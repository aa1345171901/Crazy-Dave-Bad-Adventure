using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class AshPlant : ManualPlant
{
    [Tooltip("爆炸范围")]
    public float Range = 1f;
    [Tooltip("大型僵尸目标")]
    public LayerMask BigTargetLayer;
    [Tooltip("阳光预知体，转换的阳光直接收集")]
    public Sun sun;
    public AudioClip boom;

    protected float finalRange;

    protected float sunConversionRate;  // 阳光转换率
    protected float immediateMortalityRate; // 普通僵尸即死率
    protected float increasedInjury; // 大型僵尸增伤

    private readonly int LevelBasicDamage = 5;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.5f;
    private readonly float LevelImmediateMortality = 0.05f;
    private readonly float LevelIncreasedInjury = 0.2f;

    public override void InitPlant(Card card, int sun)
    {
        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalDamage = Damage;
        finalCoolTime = CoolTime;
        finalRange = Range;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 基础伤害
                case 0:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 1 百分比伤害
                case 1:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 2 冷却时间
                case 2:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // 3 阳光转换率
                case 3:
                    sunConversionRate = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100;
                    break;
                // 4肾上腺，5普通僵尸即死率
                case 5:
                    immediateMortalityRate = (int)fieldInfo.GetValue(plantAttribute) * LevelImmediateMortality;
                    break;
                // 大型僵尸增伤
                case 6:
                    increasedInjury = (int)fieldInfo.GetValue(plantAttribute) * LevelIncreasedInjury + 1;
                    break;
                //  爆炸范围
                case 8:
                    finalRange = finalRange * (((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
        this.transform.localScale = Vector3.one * finalRange / Range;
        base.InitPlant(card, sun);
    }

    protected override void PlacePlant()
    {
        base.PlacePlant();
        StartCoroutine(BoomDelay());
    }

    protected virtual IEnumerator BoomDelay()
    {
        yield return null;
    }

    protected virtual void Boom()
    {

    }

    protected void DoDamage(Collider2D[] colliders, ref int sumHealth)
    {
        foreach (var item in colliders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                if (health)
                {
                    float random = Random.Range(0, 1f);
                    // 立即死亡
                    if (random < immediateMortalityRate && TargetLayer.Contains(item.gameObject.layer))
                    {
                        sumHealth += health.maxHealth;
                        health.DoDamage(health.maxHealth, DamageType.Bomb, true);
                    }
                    else
                    {
                        sumHealth += finalDamage > health.health ? health.health : finalDamage;
                        health.DoDamage(finalDamage, DamageType.Bomb);
                    }
                }
            }
        }
    }

    protected void IncreasedInjury(Collider2D[] colliders, ref int sumHealth)
    {
        foreach (var item in colliders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                if (health)
                {
                    int damage = (int)(finalDamage * increasedInjury);
                    sumHealth += damage > health.health ? health.health : damage;
                    health.DoDamage(damage, DamageType.Bomb);
                }
            }
        }
    }
}
