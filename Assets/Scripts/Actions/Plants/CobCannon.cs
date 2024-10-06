using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class CobCannon : Plant
{
    public override PlantType PlantType => PlantType.CobCannon;

    [Tooltip("攻击伤害")]
    public int Damage = 50;
    [Tooltip("攻击冷却时间")]
    public float CoolTime = 12f;
    [Tooltip("爆炸范围")]
    public float Range = 2f;
    public AudioSource audioSource;

    [Tooltip("子弹预制体")]
    public CobCannonBullet CobCannonBullet;

    private float timer;
    private float finalCoolTime;
    private int finalDamage;
    private float finalRange;
    private float sunConversionRate;  // 阳光转换率
    private float immediateMortalityRate; // 普通僵尸即死率
    private float increasedInjury; // 大型僵尸增伤

    private readonly int LevelBasicDamage = 5;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.5f;
    private readonly float LevelImmediateMortality = 0.05f;
    private readonly float LevelIncreasedInjury = 0.2f;

#if UNITY_EDITOR
    private void Start()
    {
        Reuse();
    }
#endif

    private void Awake()
    {
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    private void OnDestroy()
    {
        AudioManager.Instance.AudioLists.Remove(audioSource);
    }

    public override void Reuse()
    {
        base.Reuse();

        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalDamage = Damage;
        finalCoolTime = CoolTime;
        finalRange = Range;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 基础伤害
                case 0:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 1 百分比伤害
                case 1:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 2 冷却时间
                case 2:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // 3 阳光转换率
                case 3:
                    sunConversionRate = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage / 100;
                    break;
                // 4肾上腺，5普通僵尸即死率
                case 5:
                    immediateMortalityRate = (int)fieldInfo.GetValue(plantAttribute) * LevelImmediateMortality;
                    break;
                // 大型僵尸增伤
                case 6:
                    increasedInjury = (int)fieldInfo.GetValue(plantAttribute) * LevelIncreasedInjury + 1;
                    break;
                //  爆炸范围
                case 7:
                    finalRange = finalRange * (((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
        this.transform.localScale = Vector3.one * finalRange / Range;
        timer = Time.time - finalCoolTime * 3 / 4;

        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            if (LevelManager.Instance.Enemys.Count > 0)
            {
                StartCoroutine(CreateBullet());
            }
        }
    }

    IEnumerator CreateBullet()
    {
        var enemys = LevelManager.Instance.Enemys;
        Vector3 endPos = Vector3.zero;
        if (enemys.Count > 0)
        {
            int randomIndex = Random.Range(0, enemys.Count);
            var targetList = LevelManager.Instance.Enemys[randomIndex].Zombies;
            if (targetList.Count > 0)
            {
                int i = Random.Range(0, targetList.Count);
                endPos = (GameManager.Instance.Player.transform.position + targetList[i].transform.position) / 2;
            }
            animator.SetTrigger("Attack");
            timer = Time.time;
        }
        else
        {
            yield break;
        }
        yield return new WaitForSeconds(1.1f);
        audioSource.Play();
        var cobCannonBullet = GameObject.Instantiate(CobCannonBullet, this.transform);
        cobCannonBullet.transform.position = endPos;
        int damage = finalDamage;
        cobCannonBullet.finalDamage = finalDamage;
        cobCannonBullet.finalRange = finalRange;
        cobCannonBullet.immediateMortalityRate = immediateMortalityRate;
        cobCannonBullet.increasedInjury = increasedInjury;
        cobCannonBullet.sunConversionRate = sunConversionRate;
}
}
