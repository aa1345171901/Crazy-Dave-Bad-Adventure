using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Cattail : Plant
{
    public override PlantType PlantType => PlantType.Cattail;

    [Tooltip("攻击伤害")]
    public int Damage = 8;
    [Tooltip("攻击冷却时间")]
    public float CoolTime = 1.6f;

    [Tooltip("子弹发射位置")]
    public Transform BulletPos;
    [Tooltip("子弹预制体")]
    public CattailSpikeBullet CattailSpikeBullet;

    private float timer;
    private int finalPenetrationCount = 1;
    private float finalCoolTime;
    private int finalDamage;
    private float bulletSpeedMul = 1;
    private float finalCriticalDamage;
    private float finalAttackAnimSpeed = 1;

    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.1f;
    private readonly float CriticalRate = 30;

    public override void Reuse()
    {
        base.Reuse();

        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalDamage = Damage;
        finalCoolTime = CoolTime;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 为基础伤害
                case 0:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 1 为百分比伤害
                case 1:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 2 穿透个数
                case 2:
                    finalPenetrationCount = (int)fieldInfo.GetValue(plantAttribute) + 1;
                    break;
                // 3 暴击伤害
                case 3:
                    finalCriticalDamage = 2f + 2 * (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100f;
                    break;
                // 4 冷却时间
                case 4:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    finalAttackAnimSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime * 2;
                    break;
                // 子弹速度
                case 5:
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            if (LevelManager.Instance.Enemys.Count > 0)
            {
                animator.SetTrigger("Attack");
                animator.speed = finalAttackAnimSpeed;
                Invoke("CreatePeaBullet", 0.4f / finalAttackAnimSpeed);
                timer = Time.time;
            }
        }
    }

    private void CreatePeaBullet()
    {
        var spikeBullet = GameObject.Instantiate(CattailSpikeBullet, BulletPos);
        spikeBullet.Speed *= bulletSpeedMul;
        int damage = finalDamage;
        bool isCritical = Random.Range(0, 100) < CriticalRate;
        if (isCritical)
            damage = (int)(finalDamage * finalCriticalDamage);
        spikeBullet.isCritical = isCritical;
        spikeBullet.Damage = damage;
        spikeBullet.penetrationCount = finalPenetrationCount;
    }
}
