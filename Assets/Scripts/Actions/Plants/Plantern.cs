using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Plantern : Plant
{
    public override PlantType PlantType => PlantType.Plantern;
    
    [Tooltip("检测范围")]
    public float Range = 1.5f;

    public Light2D light2D;
    public CircleCollider2D circleCollider2D;

    private float finalRange;
    private float finalRangeAttackSpeed;
    private int finalRangeLifeResume;
    private float finalRangeDamage;

    private readonly float LevelRangeAttackSpeed = 0.01f;
    private readonly float LevelRange = 0.15f;
    private readonly int LevelLife = 1;
    private readonly float LevelDamage = 0.1f;

    private readonly float defaultDamage = 0.1f;  // 默认伤害
    private float defaultRangeAttackSpeed = 0.01f;

    public override void Reuse(bool randomPos = true)
    {
        base.Reuse(randomPos);

        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalRange = Range;
        finalRangeAttackSpeed = defaultRangeAttackSpeed;
        finalRangeDamage = defaultDamage;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 攻击范围
                case 0:
                    finalRange += (int)fieldInfo.GetValue(plantAttribute) * LevelRange;
                    break;
                // 1 范围攻速
                case 1:
                    finalRangeAttackSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelRangeAttackSpeed;
                    break;
                // 2 范围生命恢复
                case 2:
                    finalRangeLifeResume = (int)fieldInfo.GetValue(plantAttribute) * LevelLife;
                    break;
                // 3 范围伤害
                case 3:
                    finalRangeDamage += (int)fieldInfo.GetValue(plantAttribute) * LevelDamage;
                    break;
                default:
                    break;
            }
        }

        light2D.pointLightOuterRadius = finalRange;
        circleCollider2D.radius = finalRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Character>();
        if (player == GameManager.Instance.Player)
        {
            var attack = player.FindAbility<CharacterAttack>();
            attack.PlanternAttackSpeed += finalRangeAttackSpeed;
            if (finalRangeDamage > 0)
            {
                attack.PlanternDamage += finalRangeDamage;
            }

            if (finalRangeLifeResume > 0)
            {
                player.FindAbility<CharacterLifeRecovery>().PlanternLifeRecovery += finalRangeLifeResume;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<Character>();
        if (player == GameManager.Instance.Player)
        {
            var attack = player.FindAbility<CharacterAttack>();
            attack.PlanternAttackSpeed -= finalRangeAttackSpeed;
            if (finalRangeDamage > 0)
            {
                attack.PlanternDamage -= finalRangeDamage;
            }

            if (finalRangeLifeResume > 0)
            {
                player.FindAbility<CharacterLifeRecovery>().PlanternLifeRecovery -= finalRangeLifeResume;
            }
        }
    }
}
