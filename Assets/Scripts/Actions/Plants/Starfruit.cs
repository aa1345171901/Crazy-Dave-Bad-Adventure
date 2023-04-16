using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Starfruit : Plant
{
    public override PlantType PlantType => PlantType.Starfruit;

    [Tooltip("�����˺�")]
    public int Damage = 5;
    [Tooltip("������ȴʱ��")]
    public float CoolTime = 2;

    [Tooltip("�ӵ�����λ��")]
    public List<Transform> BulletPos;
    [Tooltip("�ӵ�Ԥ����")]
    public StarBullet StarBullet;

    private float timer;
    protected int finalDamage;
    protected float finalCoolTime;
    protected float bulletSpeedMul = 1;
    protected float splashPercentage;

    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.1f;

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
                // ��ȴʱ��
                case 2:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // �ӵ��ٶ�
                case 3:
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // �����˺�
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
