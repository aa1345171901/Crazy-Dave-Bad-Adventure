using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Chomper : Plant
{
    public override PlantType PlantType => PlantType.Chomper;

    public AudioSource audioSource;

    [Tooltip("消化时间，速度")]
    public float DigestSpeed = 8;
    [Tooltip("攻击范围")]
    public float Range = 1;
    [Tooltip("一次性吞噬个数")]
    public int SwallowCount = 1;
    [Tooltip("基础伤害，僵王巨人不能吞")]
    public int Damage = 5;

    [Tooltip("阳光预知体，转换的阳光直接收集")]
    public Sun sun;
    [Tooltip("钱币预知体，转换的钱币直接收集")]
    public Coin Coin;

    [Tooltip("吞噬目标, 优先吞噬再攻击")]
    public LayerMask AttackLayer;

    private float timer;

    private float finalDigestSpeed;
    private float finalRage;
    private int finalSwallowCount;
    private float coinConversionRate;  // 金币转换率
    private float sunConversionRate;  // 阳光转换率
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

        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalDigestSpeed = DigestSpeed;
        finalRage = Range;
        finalSwallowCount = SwallowCount;
        finalDamage = Damage;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 消化速度
                case 0:
                    finalDigestSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelDigestSpeed;
                    break;
                // 1 攻击检测范围
                case 1:
                    finalRage = Range * ((int)fieldInfo.GetValue(plantAttribute) * LevelRange + 100) / 100;
                    break;
                // 一次性吞噬个数
                case 2:
                    finalSwallowCount += (int)fieldInfo.GetValue(plantAttribute);
                    break;
                // 金币转换率
                case 3:
                    coinConversionRate = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100;
                    break;
                // 阳光转换率
                case 4:
                    sunConversionRate = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100;
                    break;
                // 基础伤害
                case 5:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 百分比伤害
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
            // 消化完毕，转换阳光和金币
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

            int count = 0; // 攻击或吞噬的个数
            // 优先检测吞噬目标，没有则检测攻击目标巨人，僵王  // todo 
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
                    timer = Time.time - finalDigestSpeed + 1;  // 攻击只有1s冷却
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
        // 检测到的数目不足再次进行检测
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
