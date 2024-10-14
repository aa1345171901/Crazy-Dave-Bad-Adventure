using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class SpikeWeed : Plant
{
    public override PlantType PlantType => isSpikeRock ? PlantType.Spikerock : PlantType.Spikeweed;

    public AudioSource audioSource;

    private Dictionary<Collider2D, float> colliderDict = new Dictionary<Collider2D, float>();  // 僵尸以及僵尸上ci受伤时间

    [Tooltip("攻击伤害")]
    public int Damage = 5;
    [Tooltip("可破坏载具数量")]
    public int DestroyingVehiclesCount = 1;
    [Tooltip("每级增加破坏载具数量")]
    public float LevelCutterCount = 0.34f;
    public float DecelerationPercentage = 0.2f;
    public float DecelerationTime = 0.5f;
    public float coolTimer = 1;

    public bool isSpikeRock;

    [Tooltip("攻击目标")]
    public LayerMask TargetLayer;

    private int finalDamage;
    private int finalDestroyingVehiclesCount;

    private float finalDecelerationPercentage;
    private float finalDecelerationTime;

    private float timer;
    private int destroyVehiclesCount;

    private readonly float LeverDecelerationPercentage = 0.03f;
    private readonly float LeverDecelerationTime = 0.2f;
    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;

    public override void Reuse(bool randomPos = true)
    {
        base.Reuse(randomPos);
        colliderDict.Clear();
        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalDamage = Damage;
        finalDestroyingVehiclesCount = DestroyingVehiclesCount;
        finalDecelerationPercentage = DecelerationPercentage;
        finalDecelerationTime = DecelerationTime;

        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 为基础伤害
                case 0:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 1 为百分比伤害
                case 1:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // 可破坏载具数量
                case 2:
                    int level = (int)fieldInfo.GetValue(plantAttribute);
                    finalDestroyingVehiclesCount += (int)(level * LevelCutterCount);
                    if (level == 10 && isSpikeRock)
                            finalDestroyingVehiclesCount = 5;
                    break;
                // 减速百分比
                case 3:
                    finalDecelerationPercentage += (int)fieldInfo.GetValue(plantAttribute) * LeverDecelerationPercentage;
                    break;
                // 减速时间
                case 4:
                    finalDecelerationTime += (int)fieldInfo.GetValue(plantAttribute) * LeverDecelerationTime;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        if (colliderDict.ContainsKey(collision))
        {
            // 每秒造成伤害
            if (Time.time - colliderDict[collision] > coolTimer)
            {
                var health = collision.GetComponent<Health>();
                if (health)
                {
                    audioSource.Play();
                    health.DoDamage(finalDamage, DamageType.Spikeweed);
                    colliderDict[collision] = Time.time;
                    var aiMove = health.GetComponent<AIMove>();
                    if (aiMove)
                    {
                        aiMove.BeDecelerated(finalDecelerationPercentage, finalDecelerationTime);
                    }
                }
            }
        }
        else
        {
            if (collision.gameObject.tag == "Car" && finalDestroyingVehiclesCount < destroyVehiclesCount)
            {
                destroyVehiclesCount++;
                ZombieProp zombieProp = collision.GetComponentInChildren<ZombieProp>();
                if (zombieProp && !zombieProp.isPuncture)
                {
                    zombieProp.Puncture();
                }
            }
            else if (TargetLayer.Contains(collision.gameObject.layer))
            {
                var health = collision.GetComponent<Health>();
                if (health)
                {
                    health.DoDamage(finalDamage, DamageType.Spikeweed);
                    audioSource.Play();
                    colliderDict.Add(collision, Time.time);
                    var aiMove = health.GetComponent<AIMove>();
                    if (aiMove)
                    {
                        aiMove.BeDecelerated(finalDecelerationPercentage, finalDecelerationTime);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (colliderDict.ContainsKey(collision))
            colliderDict.Remove(collision);
    }
}
