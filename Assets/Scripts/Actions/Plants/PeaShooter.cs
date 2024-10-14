using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PeaShooter : Plant
{
    public override PlantType PlantType => PlantType.Peashooter;

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
    protected int finalDamage;
    protected float finalRage;
    protected float finalCoolTime;
    protected float bulletSpeedMul = 1;
    protected float splashPercentage;

    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.2f;

    public override void Reuse(bool randomPos = true)
    {
        base.Reuse(randomPos);

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
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);

        realRange = FacingDirections == FacingDirections.Right ? finalRage : -finalRage;
        pos = new Vector3(this.transform.position.x + realRange / 2, this.transform.position.y);
        size = new Vector2(finalRage, 1);
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime && spriteRenderer != null)
        {          
            var hit = Physics2D.OverlapBox(pos, size, 0, TargetLayer);
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

    protected virtual void Attack(string trigger)
    {
        animator.SetTrigger(trigger);
        timer = Time.time;
        Invoke("CreatePeaBullet", 0.05f);
    }

    protected virtual PeaBullet CreatePeaBullet()
    {
        var peaBullet = GameObject.Instantiate(PeaBullet, BulletPos);
        peaBullet.Damage = finalDamage;
        peaBullet.SplashPercentage = splashPercentage;
        peaBullet.Speed *= bulletSpeedMul;
        return peaBullet;
    }
}
