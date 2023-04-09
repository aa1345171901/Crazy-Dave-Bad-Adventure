using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Cactus : Plant
{
    public override PlantType PlantType => PlantType.Cactus;

    [Tooltip("�����˺�")]
    public int Damage = 5;
    [Tooltip("������ȴʱ��")]
    public float CoolTime = 1.2f;
    [Tooltip("����Ŀ��")]
    public LayerMask TargetLayer;

    [Tooltip("�ӵ�����λ��")]
    public Transform BulletPos;
    [Tooltip("�ӵ�Ԥ����")]
    public SpikeBullet SpikeBullet;

    private float timer;
    private int finalPenetrationCount = 1;
    private float finalCoolTime;
    private int finalDamage;
    private float bulletSpeedMul = 1;
    private float finalCriticalDamage = 1.5f;
    private float finalAttackAnimSpeed = 1;

    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.1f;
    private readonly float CriticalRate = 20;

    public override void Reuse()
    {
        base.Reuse();

        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalDamage = Damage;
        finalCoolTime = CoolTime;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 ��͸����
                case 0:
                    finalPenetrationCount = (int)fieldInfo.GetValue(plantAttribute) + 1;
                    break;
                // 1 ��ȴʱ��
                case 1:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    finalAttackAnimSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime * 2;
                    break;
                // 2 Ϊ�����˺�
                case 2:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 3 Ϊ�ٷֱ��˺�
                case 3:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // �ӵ��ٶ�
                case 4:
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // �����˺�
                case 6:
                    finalCriticalDamage = 1.5f + 2 * (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100f;
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
            // 30������Ϊ�����
            var hit = Physics2D.Raycast(this.transform.position, direction, 30, TargetLayer);
            if (hit)
            {
                animator.SetTrigger("Attack");
                animator.speed = finalAttackAnimSpeed;
                Invoke("CreatePeaBullet", 0.88f / finalAttackAnimSpeed);
                timer = Time.time;
            }
        }
    }

    private void CreatePeaBullet()
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
