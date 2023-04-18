using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class IceShroom : ManualPlant
{
    [Tooltip("冰冻时间")]
    public float FrostTime = 3;

    public AudioClip frost;

    private float finalFrostTime;
    private float finalFrostAttackSpeed;

    private readonly float LevelCoolTime = 0.8f;
    private readonly float LevelFrostTime = 0.5f;
    private readonly float LevelFrostAttackSpeed = 0.1f;

    public override void InitPlant(Card card, int sun)
    {
        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalDamage = Damage;
        finalCoolTime = CoolTime;
        finalFrostTime = FrostTime;
        finalFrostAttackSpeed = 1;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 冰冻时间
                case 0:
                    finalFrostTime += LevelFrostTime * (int)fieldInfo.GetValue(plantAttribute);
                    break;
                // 1 冷却时间
                case 1:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // 3 冰冻期间攻速增加
                case 3:
                    finalFrostAttackSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelFrostAttackSpeed;
                    break;
                default:
                    break;
            }
        }
        base.InitPlant(card, sun);
    }

    protected override void PlacePlant()
    {
        base.PlacePlant();
        StartCoroutine(Frost());
    }

    IEnumerator Frost()
    {
        yield return new WaitForSeconds(0.05f);
        audioSource.clip = frost;
        audioSource.Play();
        animator.Play("Frost");
        yield return new WaitForSeconds(0.5f);
        var enemys = new List<ZombieDicts>(LevelManager.Instance.Enemys);
        foreach (var item in enemys)
        {
            foreach (var zombie in item.Zombies)
            {
                var aiMove = zombie.FindAbility<AIMove>();
                aiMove.BeDecelerated(1, finalFrostTime);
            }
        }
        GameManager.Instance.Player.FindAbility<CharacterAttack>().IceShroomAttackSpeed += finalFrostAttackSpeed;
        yield return new WaitForSeconds(1);
        this.plant.enabled = false;
        yield return new WaitForSeconds(finalFrostTime - 1);
        GameManager.Instance.Player.FindAbility<CharacterAttack>().IceShroomAttackSpeed -= finalFrostAttackSpeed;
        Destroy(this.gameObject);
    }
}
