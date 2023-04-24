using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Chomper : Plant
{
    public override PlantType PlantType => PlantType.Chomper;

    public AudioSource audioSource;

    [Tooltip("����ʱ�䣬�ٶ�")]
    public float DigestSpeed = 8;
    [Tooltip("������Χ")]
    public float Range = 1;
    [Tooltip("һ�������ɸ���")]
    public int SwallowCount = 1;
    [Tooltip("�����˺����������˲�����")]
    public int Damage = 5;

    [Tooltip("����Ԥ֪�壬ת��������ֱ���ռ�")]
    public Sun sun;
    [Tooltip("Ǯ��Ԥ֪�壬ת����Ǯ��ֱ���ռ�")]
    public Coin Coin;

    [Tooltip("����Ŀ��, ���������ٹ���")]
    public LayerMask AttackLayer;

    private float timer;

    private float finalDigestSpeed;
    private float finalRage;
    private int finalSwallowCount;
    private float coinConversionRate;  // ���ת����
    private float sunConversionRate;  // ����ת����
    private int finalDamage;

    private List<Health> targetHealthList = new List<Health>();
    private bool lastIsSwallow;

    private readonly float LevelDigestSpeed = -0.4f;
    private readonly float LevelRange = 15;
    private readonly float LevelPercentage = 10;
    private readonly int LevelBasicDamage = 2;

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    public override void Reuse()
    {
        base.Reuse();

        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalDigestSpeed = DigestSpeed;
        finalRage = Range;
        finalSwallowCount = SwallowCount;
        finalDamage = Damage;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 �����ٶ�
                case 0:
                    finalDigestSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelDigestSpeed;
                    break;
                // 1 ������ⷶΧ
                case 1:
                    finalRage = Range * ((int)fieldInfo.GetValue(plantAttribute) * LevelRange + 100) / 100;
                    break;
                // һ�������ɸ���
                case 2:
                    finalSwallowCount += (int)fieldInfo.GetValue(plantAttribute);
                    break;
                // ���ת����
                case 3:
                    coinConversionRate = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100;
                    break;
                // ����ת����
                case 4:
                    sunConversionRate = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100;
                    break;
                // �����˺�
                case 5:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // �ٷֱ��˺�
                case 6:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);

        realRange = FacingDirections == FacingDirections.Right ? finalRage + 0.5f: -finalRage - 0.5f;
        pos = new Vector3(this.transform.position.x + realRange / 2, this.transform.position.y - 0.5f);
        size = new Vector2(finalRage + 0.5f, 1f);
    }

    private void Update()
    {
        if (Time.time - timer > finalDigestSpeed)
        {
            animator.SetBool("IsDigest", false);
            // ������ϣ�ת������ͽ��
            if (targetHealthList.Count > 0)
            {
                if (lastIsSwallow)
                {
                    if (sunConversionRate != 0 || coinConversionRate != 0)
                    {
                        int sumHealth = 0;
                        foreach (var item in targetHealthList)
                        {
                            sumHealth += item.maxHealth;
                        }
                        if (sunConversionRate != 0)
                        {
                            var sunItem = GameObject.Instantiate(sun, this.transform);
                            sunItem.Price = (int)(sunConversionRate * sumHealth * 5);
                            sunItem.Digest();
                        }

                        if (coinConversionRate != 0)
                        {
                            var coinItem = GameObject.Instantiate(Coin, this.transform);
                            coinItem.Price = (int)(coinConversionRate * sumHealth * 2);
                            coinItem.Digest();
                        }
                    }
                }
                targetHealthList.Clear();
            }

            int count = 0; // ���������ɵĸ���
            // ���ȼ������Ŀ�꣬û�����⹥��Ŀ����ˣ�����  // todo 
            OverlapBox(ref count);
            if (count != 0)
            {
                timer = Time.time;
                animator.SetTrigger("Attack");
                StartCoroutine("Attack", true);
            }
            else
            {
                OverlapBox(ref count, false);
                if (count != 0)
                {
                    timer = Time.time - finalDigestSpeed + 1;  // ����ֻ��1s��ȴ
                    animator.SetTrigger("Attack");
                    StartCoroutine("Attack", false);
                }
            }
        }
    }

    private void OverlapBox(ref int count, bool isSwallow = true)
    {
        var colliders = Physics2D.OverlapBoxAll(pos, size, 0, AttackLayer);
        foreach (var item in colliders)
        {
            if (item.isTrigger && (!isSwallow || (isSwallow && item.tag != "BigZombie")))
            {
                var health = item.GetComponent<Health>();
                if (health)
                {
                    if (!targetHealthList.Contains(health))
                        targetHealthList.Add(health);
                    count++;
                }
            }
            if (count >= finalSwallowCount)
                break;
        }
    }

    IEnumerator Attack(bool isSwallow)
    {
        yield return new WaitForSeconds(0.8f);
        audioSource.Play();
        animator.SetBool("IsDigest", isSwallow);
        lastIsSwallow = isSwallow;
        // ��⵽����Ŀ�����ٴν��м��
        if (targetHealthList.Count < SwallowCount)
        {
            int count = targetHealthList.Count;
            OverlapBox(ref count, isSwallow);
        }
        foreach (var item in targetHealthList)
        {
            item.DoDamage(isSwallow ? item.maxHealth : finalDamage, DamageType.Chomper, isSwallow);
        }
    }
}
