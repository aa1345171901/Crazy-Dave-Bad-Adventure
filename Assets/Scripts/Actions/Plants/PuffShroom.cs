using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PuffShroom : Plant
{
    public override PlantType PlantType => PlantType.PuffShroom;

    [Tooltip("攻击伤害")]
    public int Damage = 3;
    [Tooltip("攻击范围")]
    public float Range = 3;
    [Tooltip("攻击冷却时间")]
    public float CoolTime = 2;
    [Tooltip("攻击目标")]
    public LayerMask TargetLayer;

    [Tooltip("子弹发射位置")]
    public Transform BulletPos;
    [Tooltip("子弹预制体")]
    public ShroomBullet ShroomBullet;

    protected float timer;
    protected int finalDamage;
    protected float finalRage;
    protected float finalCoolTime;
    protected float bulletSpeedMul = 1;
    protected float splashPercentage;
    protected float bulletAddRate;
    protected float bulletSize;

    protected readonly int LevelBasicDamage = 1;
    protected readonly float LevelPercentage = 10;
    protected readonly float LevelCoolTime = 0.1f;

    public override void Reuse(bool randomPos = true)
    {
        base.Reuse(randomPos);

        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalDamage = Damage;
        finalRage = Range;
        finalCoolTime = CoolTime;
        bulletSize = 1;
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
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // 溅射伤害
                case 5:
                    splashPercentage = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage;
                    break;
                // 子弹变多概率
                case 6:
                    bulletAddRate = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // 子弹大小
                case 7:
                    bulletSize += ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage) / 100;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);

        realRange = FacingDirections == FacingDirections.Right ? finalRage : -finalRage;
        pos = new Vector3(this.transform.position.x + realRange / 2, this.transform.position.y);
        finalCoolTime = finalCoolTime < 0.1f ? 0.1f : finalCoolTime;
        size = new Vector2(finalRage, bulletSize);
        this.transform.localScale = new Vector3(Mathf.Sign(this.transform.localScale.x) * bulletSize, bulletSize, bulletSize);
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            var hit = Physics2D.OverlapBox(pos, size, 0, TargetLayer);
            if (hit)
            {
                timer = Time.time;
                StartCoroutine(CreateBullet());
            }
        }
    }

    protected IEnumerator CreateBullet()
    {
        InitBullet();
        yield return new WaitForSeconds(0.05f);
        int i = 0;
        float bulletAdd = bulletAddRate;
        for (; i < bulletAdd; i++)
        {
            InitBullet();
            yield return new WaitForSeconds(0.05f);
        }
        bulletAdd -= i - 1;

        if (Random.Range(0, 1f) < bulletAdd)
            InitBullet();
    }

    protected void InitBullet()
    {
        animator.SetTrigger("Attack");
        var bullet = GameObject.Instantiate(ShroomBullet, BulletPos);
        bullet.Damage = finalDamage;
        bullet.SplashPercentage = splashPercentage;
        bullet.Speed *= bulletSpeedMul;
        bullet.BulletSize = bulletSize;
    }
}
