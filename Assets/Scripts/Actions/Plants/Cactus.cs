using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Cactus : Plant
{
    public override PlantType PlantType => PlantType.Cactus;

    [Tooltip("攻击伤害")]
    public int Damage = 5;
    [Tooltip("攻击冷却时间")]
    public float CoolTime = 1.2f;
    [Tooltip("攻击目标")]
    public LayerMask TargetLayer;

    [Tooltip("子弹发射位置")]
    public Transform BulletPos;
    [Tooltip("子弹预制体")]
    public SpikeBullet SpikeBullet;

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
    private readonly float CriticalRate = 20;

    public override void Reuse(bool randomPos = true)
    {
        base.Reuse(randomPos);

        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalDamage = Damage;
        finalCoolTime = CoolTime;
        finalCriticalDamage = 1.5f;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 穿透个数
                case 0:
                    finalPenetrationCount = (int)fieldInfo.GetValue(plantAttribute) + 1;
                    break;
                // 1 冷却时间
                case 1:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    finalAttackAnimSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime * 2;
                    break;
                // 2 为基础伤害
                case 2:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 3 为百分比伤害
                case 3:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 子弹速度
                case 4:
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // 暴击伤害
                case 6:
                    finalCriticalDamage += 2 * (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100f;
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
            var direction = FacingDirections == FacingDirections.Right ? Vector2.right : Vector2.left;
            // 30个格子为最大宽度
            var hit = Physics2D.Raycast(this.transform.position, direction, 30, TargetLayer);
            if (hit)
            {
                animator.SetTrigger("Attack");
                animator.speed = finalAttackAnimSpeed;
                Invoke("CreateBullet", 0.88f / finalAttackAnimSpeed);
                timer = Time.time;
            }
        }
    }

    private void CreateBullet()
    {
        var spikeBullet = GameObject.Instantiate(SpikeBullet, BulletPos);
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
