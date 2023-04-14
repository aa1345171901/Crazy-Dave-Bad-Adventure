using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPea : PeaShooter
{
    public override PlantType PlantType => PlantType.SnowPea;

    public float DecelerationPercentage = 0.2f;
    public float DecelerationTime = 3f;

    private float finalDecelerationPercentage;
    private float finalDecelerationTime;

    private readonly float LeverDecelerationPercentage = 0.03f;
    private readonly float LeverDecelerationTime = 0.5f;

    public override void Reuse()
    {
        base.Reuse();

        finalDecelerationPercentage = DecelerationPercentage;
        finalDecelerationTime = DecelerationTime;

        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // ���ٰٷֱ�
                case 6:
                    finalDecelerationPercentage += (int)fieldInfo.GetValue(plantAttribute) * LeverDecelerationPercentage;
                    break;
                // ����ʱ��
                case 7:
                    finalDecelerationTime += (int)fieldInfo.GetValue(plantAttribute) * LeverDecelerationTime;
                    break;
                default:
                    break;
            }
        }
    }

    protected override PeaBullet CreatePeaBullet()
    {
        var snowPeaBullet =  base.CreatePeaBullet() as SnowPeaBullet;
        snowPeaBullet.DecelerationPercentage = finalDecelerationPercentage;
        snowPeaBullet.DecelerationTime = finalDecelerationTime;
        return snowPeaBullet;
    }
}
