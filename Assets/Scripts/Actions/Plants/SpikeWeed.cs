using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class SpikeWeed : Plant
{
    public override PlantType PlantType => PlantType.Spikeweed;

    public AudioSource audioSource;

    private Dictionary<Collider2D, float> colliderDict = new Dictionary<Collider2D, float>();  // ��ʬ�Լ���ʬ��ci����ʱ��

    [Tooltip("�����˺�")]
    public int Damage = 5;
    [Tooltip("���ƻ��ؾ�����")]
    public int DestroyingVehiclesCount = 1;
    [Tooltip("ÿ�������ƻ��ؾ�����")]
    public float LevelCutterCount = 0.34f;
    public float DecelerationPercentage = 0.2f;
    public float DecelerationTime = 0.5f;
    public float coolTimer = 1;

    public bool isSpikeRock;

    [Tooltip("����Ŀ��")]
    public LayerMask TargetLayer;

    private int finalDamage;
    private int finalDestroyingVehiclesCount;

    private float finalDecelerationPercentage;
    private float finalDecelerationTime;

    private float timer;

    private readonly float LeverDecelerationPercentage = 0.03f;
    private readonly float LeverDecelerationTime = 0.2f;
    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;

    public override void Reuse()
    {
        base.Reuse();
        colliderDict.Clear();
        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalDamage = Damage;
        finalDestroyingVehiclesCount = DestroyingVehiclesCount;
        finalDecelerationPercentage = DecelerationPercentage;
        finalDecelerationTime = DecelerationTime;

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
                // ���ƻ��ؾ�����
                case 2:
                    int level = (int)fieldInfo.GetValue(plantAttribute);
                    finalDestroyingVehiclesCount += (int)(level * LevelCutterCount);
                    if (level == 10 && isSpikeRock)
                            finalDestroyingVehiclesCount = 5;
                    break;
                // ���ٰٷֱ�
                case 3:
                    finalDecelerationPercentage += (int)fieldInfo.GetValue(plantAttribute) * LeverDecelerationPercentage;
                    break;
                // ����ʱ��
                case 4:
                    finalDecelerationTime += (int)fieldInfo.GetValue(plantAttribute) * LeverDecelerationTime;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);
    }

    private void Update()
    {
        if (Time.time - timer > coolTimer)
        {
            timer = Time.time;
            foreach (var item in colliderDict)
            {
                if (Time.time - item.Value > coolTimer)
                {
                    var health = item.Key.GetComponent<Health>();
                    if (health)
                    {
                        audioSource.Play();
                        health.DoDamage(finalDamage, DamageType.Spikeweed);
                        var aiMove = health.GetComponent<AIMove>();
                        if (aiMove)
                        {
                            aiMove.BeDecelerated(finalDecelerationPercentage, finalDecelerationTime);
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        if (colliderDict.ContainsKey(collision))
        {
            // ÿ������˺�
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
            if (TargetLayer.Contains(collision.gameObject.layer))
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
