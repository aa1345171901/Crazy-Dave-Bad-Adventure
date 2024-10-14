using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Starfruit : Plant
{
    public override PlantType PlantType => PlantType.Starfruit;

    [Tooltip("攻击伤害")]
    public int Damage = 5;
    [Tooltip("攻击冷却时间")]
    public float CoolTime = 2;

    [Tooltip("子弹发射位置")]
    public List<Transform> BulletPos;
    [Tooltip("子弹预制体")]
    public StarBullet StarBullet;

    private float timer;
    protected int finalDamage;
    protected float finalCoolTime;
    protected float bulletSpeedMul = 1;
    protected float splashPercentage;

    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.1f;

    public override void Reuse(bool randomPos = true)
    {
        base.Reuse(randomPos);

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
                // 冷却时间
                case 2:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // 子弹速度
                case 3:
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // 溅射伤害
                case 4:
                    splashPercentage = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
        animator.speed = 1 + CoolTime - finalCoolTime;
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            if (LevelManager.Instance.Enemys.Count > 0)
            {
                timer = Time.time;
                Invoke("CreatePeaBullet", 0.7f / animator.speed);
            }
        }
    }

    private void CreatePeaBullet()
    {
        foreach (var item in BulletPos)
        {
            var starBullet = GameObject.Instantiate(StarBullet, item);
            starBullet.Damage = finalDamage;
            starBullet.SplashPercentage = splashPercentage;
            starBullet.Speed *= bulletSpeedMul;
            starBullet.StarfruitPos = this.transform.position;
            starBullet.isRight = FacingDirections == FacingDirections.Right;
        }
    }
}
