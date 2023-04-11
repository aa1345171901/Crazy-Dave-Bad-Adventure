using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class CherryBomb : ManualPlant
{
    [Tooltip("爆炸范围，圆的半径")]
    public float Range = 1f;
    [Tooltip("大型僵尸目标")]
    public LayerMask BigTargetLayer;
    [Tooltip("阳光预知体，转换的阳光直接收集")]
    public Sun sun;
    public AudioClip boom;

    protected float finalRange;

    private float sunConversionRate;  // 阳光转换率
    private float immediateMortalityRate; // 普通僵尸即死率
    private float increasedInjury; // 大型僵尸增伤

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
                // 0 爆炸范围
                case 0:
                    finalRange = finalRange * (((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 1 基础伤害
                case 1:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 2 百分比伤害
                case 2:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 3 冷却时间
                case 3:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // 4 阳光转换率
                case 4:
                    sunConversionRate = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100;
                    break;
                // 5肾上腺，6普通僵尸即死率
                case 6:
                    immediateMortalityRate = (int)fieldInfo.GetValue(plantAttribute) * LevelImmediateMortality;
                    break;
                // 大型僵尸增伤
                case 7:
                    increasedInjury = (int)fieldInfo.GetValue(plantAttribute) * LevelIncreasedInjury + 1;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
        base.InitPlant(card, sun);
    }

    protected override void PlacePlant()
    {
        base.PlacePlant();
        Invoke("PlayBoom", 0.05f);
        animator.Play("Boom");
        Invoke("Boom", 0.5f);
    }

    private void PlayBoom()
    {
        audioSource.clip = boom;
        audioSource.Play();
    }

    private void Boom()
    {
        int sumHealth = 0;
        LayerMask targetLayer = increasedInjury > 0 ? TargetLayer: TargetLayer | BigTargetLayer;
        var colloders = Physics2D.OverlapCircleAll(this.transform.position, finalRange, targetLayer);
        foreach (var item in colloders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                float random = Random.Range(0, 1f);
                Debug.Log(random);
                // 立即死亡
                if (random < immediateMortalityRate && TargetLayer.Contains(item.gameObject.layer))
                {
                    sumHealth += health.maxHealth;
                    health.DoDamage(health.maxHealth, DamageType.CherryBomb, true);
                }
                else
                {
                    sumHealth += finalDamage > health.health ? health.health : finalDamage;
                    health.DoDamage(finalDamage, DamageType.CherryBomb);
                }
            }
        }

        if (increasedInjury > 0)
        {
            colloders = Physics2D.OverlapCircleAll(this.transform.position, finalRange, BigTargetLayer);
            foreach (var item in colloders)
            {
                if (item.isTrigger)
                {
                    var health = item.GetComponent<Health>();
                    int damage = (int)(finalDamage * increasedInjury);
                    sumHealth += damage > health.health ? health.health : damage;
                    health.DoDamage(damage, DamageType.CherryBomb);
                }
            }
        }

        if (sunConversionRate != 0)
        {
            var sunItem = GameObject.Instantiate(sun, this.transform);
            sunItem.Price = (int)(sunConversionRate * sumHealth * 5);
            sunItem.Digest();
        }

        Invoke("DestroyCherry", 1);
    }

    private void DestroyCherry()
    {
        Destroy(this.gameObject);
    }
}
