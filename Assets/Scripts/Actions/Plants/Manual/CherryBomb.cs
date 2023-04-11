using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class CherryBomb : ManualPlant
{
    [Tooltip("��ը��Χ��Բ�İ뾶")]
    public float Range = 1f;
    [Tooltip("���ͽ�ʬĿ��")]
    public LayerMask BigTargetLayer;
    [Tooltip("����Ԥ֪�壬ת��������ֱ���ռ�")]
    public Sun sun;
    public AudioClip boom;

    protected float finalRange;

    private float sunConversionRate;  // ����ת����
    private float immediateMortalityRate; // ��ͨ��ʬ������
    private float increasedInjury; // ���ͽ�ʬ����

    private readonly int LevelBasicDamage = 5;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.5f;
    private readonly float LevelImmediateMortality = 0.05f;
    private readonly float LevelIncreasedInjury = 0.2f;

    public override void InitPlant(Card card, int sun)
    {
        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalDamage = Damage;
        finalCoolTime = CoolTime;
        finalRange = Range;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 ��ը��Χ
                case 0:
                    finalRange = finalRange * (((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 1 �����˺�
                case 1:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 2 �ٷֱ��˺�
                case 2:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 3 ��ȴʱ��
                case 3:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // 4 ����ת����
                case 4:
                    sunConversionRate = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100;
                    break;
                // 5�����٣�6��ͨ��ʬ������
                case 6:
                    immediateMortalityRate = (int)fieldInfo.GetValue(plantAttribute) * LevelImmediateMortality;
                    break;
                // ���ͽ�ʬ����
                case 7:
                    increasedInjury = (int)fieldInfo.GetValue(plantAttribute) * LevelIncreasedInjury + 1;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
        base.InitPlant(card, sun);
    }

    protected override void PlacePlant()
    {
        base.PlacePlant();
        Invoke("PlayBoom", 0.05f);
        animator.Play("Boom");
        Invoke("Boom", 0.5f);
    }

    private void PlayBoom()
    {
        audioSource.clip = boom;
        audioSource.Play();
    }

    private void Boom()
    {
        int sumHealth = 0;
        LayerMask targetLayer = increasedInjury > 0 ? TargetLayer: TargetLayer | BigTargetLayer;
        var colloders = Physics2D.OverlapCircleAll(this.transform.position, finalRange, targetLayer);
        foreach (var item in colloders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                float random = Random.Range(0, 1f);
                Debug.Log(random);
                // ��������
                if (random < immediateMortalityRate && TargetLayer.Contains(item.gameObject.layer))
                {
                    sumHealth += health.maxHealth;
                    health.DoDamage(health.maxHealth, DamageType.CherryBomb, true);
                }
                else
                {
                    sumHealth += finalDamage > health.health ? health.health : finalDamage;
                    health.DoDamage(finalDamage, DamageType.CherryBomb);
                }
            }
        }

        if (increasedInjury > 0)
        {
            colloders = Physics2D.OverlapCircleAll(this.transform.position, finalRange, BigTargetLayer);
            foreach (var item in colloders)
            {
                if (item.isTrigger)
                {
                    var health = item.GetComponent<Health>();
                    int damage = (int)(finalDamage * increasedInjury);
                    sumHealth += damage > health.health ? health.health : damage;
                    health.DoDamage(damage, DamageType.CherryBomb);
                }
            }
        }

        if (sunConversionRate != 0)
        {
            var sunItem = GameObject.Instantiate(sun, this.transform);
            sunItem.Price = (int)(sunConversionRate * sumHealth * 5);
            sunItem.Digest();
        }

        Invoke("DestroyCherry", 1);
    }

    private void DestroyCherry()
    {
        Destroy(this.gameObject);
    }
}
