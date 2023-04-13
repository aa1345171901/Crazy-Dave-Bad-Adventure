using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Plantern : Plant
{
    public override PlantType PlantType => PlantType.Plantern;
    
    [Tooltip("ºÏ≤‚∑∂Œß")]
    public float Range = 1.5f;
    public float RangeAttackSpeed = 1.1f;

    public Light2D light2D;
    public CircleCollider2D circleCollider2D;

    private float finalRange;
    private float finalRangeAttackSpeed;
    private int finalRangeLifeResume;
    private float finalRangeDamage;

    private readonly float LevelRangeAttackSpeed = 0.05f;
    private readonly float LevelRange = 0.15f;
    private readonly int LevelLife = 1;
    private readonly float LevelDamage = 0.1f;

    public override void Reuse()
    {
        base.Reuse();

        //  Ù–‘À≥–Ú–Ë“™”ÎPlantCultivationPage…Ëº∆µƒŒƒ◊÷œ‡∂‘”¶
        finalRange = Range;
        finalRangeAttackSpeed = RangeAttackSpeed;
        finalRangeDamage = 1;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // ◊÷∂Œ”≥…‰
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 π•ª˜∑∂Œß
                case 0:
                    finalRange += (int)fieldInfo.GetValue(plantAttribute) * LevelRange;
                    break;
                // 1 ∑∂Œßπ•ÀŸ
                case 1:
                    finalRangeAttackSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelRangeAttackSpeed;
                    break;
                // 2 ∑∂Œß…˙√¸ª÷∏¥
                case 2:
                    finalRangeLifeResume = (int)fieldInfo.GetValue(plantAttribute) * LevelLife;
                    break;
                // 3 ∑∂Œß…À∫¶
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
