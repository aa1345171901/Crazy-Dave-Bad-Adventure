using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PeaShooter : Plant
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Sprite attack0Trigger;
    public Sprite attack1Trigger;
    public Sprite attack2Trigger;

    [Tooltip("攻击伤害")]
    public int Damage = 5;
    [Tooltip("攻击范围")]
    public float Range = 5;
    [Tooltip("攻击冷却时间")]
    public float CoolTime = 2;
    [Tooltip("攻击目标")]
    public LayerMask TargetLayer;

    [Tooltip("子弹发射位置")]
    public Transform BulletPos;
    [Tooltip("子弹预制体")]
    public PeaBullet PeaBullet;

    private float timer;
    private int finalDamage;
    private float finalRage;
    private float finalCoolTime;
    private float bulletSpeedMul = 1;
    private float splashPercentage;

    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.2f;

    public override void Reuse()
    {
        base.Reuse();
        var levelBounds = LevelManager.Instance.LevelBounds;
        float randomX = Random.Range(levelBounds.min.x, levelBounds.max.x);
        // 0.5 刚好站在格子上
        float randomY = (int)Random.Range(levelBounds.min.y, levelBounds.max.y - 0.5f) + 0.5f;
        this.transform.position = new Vector3(randomX, randomY, 0);
        int y = (int)((-randomY + 10) * 10);
        spriteRenderer.sortingOrder = y;
        // 如果在右半部分则面向左
        if (randomX > levelBounds.max.x / 2)
            FacingDirections = FacingDirections.Left;
        else
            FacingDirections = FacingDirections.Right;

        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalDamage = Damage;
        finalRage = Range;
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
                // 检测范围
                case 2:
                    finalRage = Range * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // 冷却时间
                case 3:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // 子弹速度
                case 4:
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100 ) / 100;
                    break;
                // 溅射伤害
                case 5:
                    splashPercentage = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany + 100) / 100f);
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            var hit = Physics2D.Raycast(this.transform.position, Vector2.right, finalRage, TargetLayer);
            if (hit)
            {
                if (spriteRenderer.sprite == attack0Trigger)
                {
                    Attack("Attack0");
                }
                else if (spriteRenderer.sprite == attack1Trigger)
                {
                    Attack("Attack1-3");
                }
                else if (spriteRenderer.sprite == attack2Trigger)
                {
                    Attack("Attack2-3");
                }
            }
        }
    }

    private void Attack(string trigger)
    {
        animator.SetTrigger(trigger);
        timer = Time.time;
        Invoke("CreatePeaBullet", 0.05f);
        // 双发
        // Invoke("CreatePeaBullet", 0.15f);
    }

    private void CreatePeaBullet()
    {
        var peaBullet = GameObject.Instantiate(PeaBullet, BulletPos);
        peaBullet.Damage = finalDamage;
        peaBullet.SplashPercentage = splashPercentage;
        float speedMul = FacingDirections == FacingDirections.Right ? 1 : -1;
        peaBullet.Speed *= speedMul * bulletSpeedMul;
    }
}
