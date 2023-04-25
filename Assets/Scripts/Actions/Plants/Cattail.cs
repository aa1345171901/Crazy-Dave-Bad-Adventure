using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Cattail : Plant
{
    public override PlantType PlantType => PlantType.Cattail;

    [Tooltip("�����˺�")]
    public int Damage = 8;
    [Tooltip("������ȴʱ��")]
    public float CoolTime = 1.6f;

    [Tooltip("�ӵ�����λ��")]
    public Transform BulletPos;
    [Tooltip("�ӵ�Ԥ����")]
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
                // 0 Ϊ�����˺�
                case 0:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 1 Ϊ�ٷֱ��˺�
                case 1:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 2 ��͸����
                case 2:
                    finalPenetrationCount = (int)fieldInfo.GetValue(plantAttribute) + 1;
                    break;
                // 3 �����˺�
                case 3:
                    finalCriticalDamage = 2f + 2 * (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100f;
                    break;
                // 4 ��ȴʱ��
                case 4:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    finalAttackAnimSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime * 2;
                    break;
                // �ӵ��ٶ�
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
