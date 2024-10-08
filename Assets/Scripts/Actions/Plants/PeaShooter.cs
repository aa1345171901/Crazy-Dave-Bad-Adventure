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

    [Tooltip("�����˺�")]
    public int Damage = 5;
    [Tooltip("������Χ")]
    public float Range = 5;
    [Tooltip("������ȴʱ��")]
    public float CoolTime = 2;
    [Tooltip("����Ŀ��")]
    public LayerMask TargetLayer;

    [Tooltip("�ӵ�����λ��")]
    public Transform BulletPos;
    [Tooltip("�ӵ�Ԥ����")]
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

    public override void Reuse()
    {
        base.Reuse();

        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalDamage = Damage;
        finalRage = Range;
        finalCoolTime = CoolTime;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 Ϊ�����˺�
                case 0:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 1 Ϊ�ٷֱ��˺�
                case 1:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // ��ⷶΧ
                case 2:
                    finalRage = Range * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // ��ȴʱ��
                case 3:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // �ӵ��ٶ�
                case 4:
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100 ) / 100;
                    break;
                // �����˺�
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
