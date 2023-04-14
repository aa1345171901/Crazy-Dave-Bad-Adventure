using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PuffShroom : Plant
{
    public override PlantType PlantType => PlantType.PuffShroom;

    [Tooltip("�����˺�")]
    public int Damage = 3;
    [Tooltip("������Χ")]
    public float Range = 3;
    [Tooltip("������ȴʱ��")]
    public float CoolTime = 2;
    [Tooltip("����Ŀ��")]
    public LayerMask TargetLayer;

    [Tooltip("�ӵ�����λ��")]
    public Transform BulletPos;
    [Tooltip("�ӵ�Ԥ����")]
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

    public override void Reuse()
    {
        base.Reuse();

        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalDamage = Damage;
        finalRage = Range;
        finalCoolTime = CoolTime;
        bulletSize = 1;
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
                    bulletSpeedMul = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // �����˺�
                case 5:
                    splashPercentage = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage;
                    break;
                // �ӵ�������
                case 6:
                    bulletAddRate = ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // �ӵ���С
                case 7:
                    bulletSize += ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);

        realRange = FacingDirections == FacingDirections.Right ? finalRage : -finalRage;
        pos = new Vector3(this.transform.position.x + realRange / 2, this.transform.position.y - 0.5f);
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
