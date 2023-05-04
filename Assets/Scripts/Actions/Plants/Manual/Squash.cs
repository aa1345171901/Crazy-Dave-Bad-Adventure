using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Squash : ManualPlant
{
    [Tooltip("����Ԥ֪�壬ת��������ֱ���ռ�")]
    public Sun sun;
    public AudioClip sit;
    public AudioClip boom;

    private float sunConversionRate;  // ����ת����
    private float immediateMortalityRate; // ��ͨ��ʬ������
    private float increasedInjury; // ���ͽ�ʬ����
    private float finalSittingRate;

    private readonly int LevelBasicDamage = 5;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.5f;
    private readonly float LevelImmediateMortality = 0.05f;
    private readonly float LevelIncreasedInjury = 0.2f;

    private readonly float JumpTime = 0.5f;

    private Character target;
    private Vector3 startPos;
    private bool isAttack;
    private float timer;

    public override void InitPlant(Card card, int sun)
    {
        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalDamage = Damage;
        finalCoolTime = CoolTime;
        finalSittingRate = 0;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 �����˺�
                case 0:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 1 �ٷֱ��˺�
                case 1:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 2 ��ȴʱ��
                case 2:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // 3 ����ת����
                case 3:
                    sunConversionRate = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100;
                    break;
                // 4��ͨ��ʬ������
                case 4:
                    immediateMortalityRate = (int)fieldInfo.GetValue(plantAttribute) * LevelImmediateMortality;
                    break;
                // ���ͽ�ʬ����
                case 5:
                    increasedInjury = (int)fieldInfo.GetValue(plantAttribute) * LevelIncreasedInjury + 1;
                    break;
                case 6:
                    finalSittingRate = (int)fieldInfo.GetValue(plantAttribute) * LevelImmediateMortality;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
        base.InitPlant(card, sun);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target != null || IsManual)
            return;
        if (TargetLayer.Contains(collision.gameObject.layer))
        {
            var character = collision.GetComponent<Character>();
            if (character)
            {
                target = character;
                isAttack = true;
                timer = 0;
                startPos = this.transform.position;
                audioSource.clip = sit;
                audioSource.Play();
            }
        }
    }

    protected override void Processblity()
    {
        base.Processblity();

        if (isAttack)
        {
            if (timer < JumpTime)
            {
                if (timer < JumpTime / 2)
                {
                    float upSpeed = (1 - timer / JumpTime / 2) * 5;
                    transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
                    transform.localScale = new Vector3(1, 1 + timer / JumpTime / 4, 1);
                }
                else
                {
                    float down = (timer - JumpTime / 2) / JumpTime / 2 * 5;
                    transform.Translate(Vector3.down * down * Time.deltaTime);
                    transform.localScale = new Vector3(1, 1.5f - (timer - JumpTime / 2) / JumpTime / 2, 1);
                }
                float process = timer / JumpTime;
                var lerp = Vector3.Lerp(startPos, target.transform.position, process);
                transform.position = new Vector3(lerp.x, transform.position.y, 0);

                timer += Time.deltaTime;
            }
            else
            {
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, 0);
                isAttack = false;
                StartCoroutine(AttackDelay());
            }
        }
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.05f);
        transform.localScale = new Vector3(1, 0.2f, 1);
        audioSource.clip = boom;
        audioSource.Play();
        Attack();
        if (Random.Range(0,1f) < finalSittingRate)
        {
            yield return new WaitForSeconds(0.3f);
            target = null;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
        }
    }

    private void Attack()
    {
        int sumHealth = 0;
        var colliders = Physics2D.OverlapCircleAll(this.transform.position, 1, TargetLayer);
        DoDamage(colliders, ref sumHealth);

        if (sunConversionRate != 0)
        {
            var sunItem = GameObject.Instantiate(sun, this.transform);
            sunItem.Price = (int)(sunConversionRate * sumHealth * 5);
            sunItem.Digest();
        }
    }

    protected void DoDamage(Collider2D[] colliders, ref int sumHealth)
    {
        foreach (var item in colliders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                if (health)
                {
                    float random = Random.Range(0, 1f);
                    // ��������
                    if (random < immediateMortalityRate && item.tag != "BigZombie")
                    {
                        sumHealth += health.maxHealth;
                        health.DoDamage(health.maxHealth, DamageType.Bomb, true);
                    }
                    else
                    {
                        if (increasedInjury > 0 && item.tag == "BigZombie")
                        {
                            int damage = (int)(finalDamage * increasedInjury);
                            sumHealth += damage > health.health ? health.health : damage;
                            health.DoDamage(damage, DamageType.Bomb);
                        }
                        else
                        {
                            sumHealth += finalDamage > health.health ? health.health : finalDamage;
                            health.DoDamage(finalDamage, DamageType.Bomb);
                        }
                    }
                }
            }
        }
    }

    private void IncreasedInjury(Collider2D[] colliders, ref int sumHealth)
    {
        foreach (var item in colliders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                if (health)
                {
                    int damage = (int)(finalDamage * increasedInjury);
                    sumHealth += damage > health.health ? health.health : damage;
                    health.DoDamage(damage, DamageType.Squash);
                }
            }
        }
    }
}
