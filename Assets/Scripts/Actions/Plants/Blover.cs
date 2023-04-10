using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Blover : Plant
{
    public override PlantType PlantType => PlantType.Blover;

    private readonly float LevelWindSpeed = 0.1f;
    private readonly int LevelWindResume = 1;

    public override void Reuse()
    {
        if (spriteRenderer == null)
            spriteRenderer = this.GetComponent<SpriteRenderer>();

        var levelBounds = LevelManager.Instance.LevelBounds;
        float x = levelBounds.min.x;
        // 0.5 �պ�վ�ڸ�����
        float randomY = (int)Random.Range(levelBounds.min.y, levelBounds.max.y - 0.5f) + 0.5f;
        this.transform.position = new Vector3(x, randomY, 0);
        int y = (int)((-randomY + 10) * 10);
        spriteRenderer.sortingOrder = y;
        FacingDirections = FacingDirections.Right;

        float finalWindSpeed = GardenManager.Instance.Windspeed;
        int finalResume = GardenManager.Instance.BloverResume;
        float finalWindage = GardenManager.Instance.Windage;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 2 ����
                case 2:
                    finalWindSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelWindSpeed;
                    if (finalWindage > 1)
                    {
                        finalWindage = 1 + (finalWindage - 1) / 5;
                    }
                    break;
                // 3 �ָ�
                case 3:
                    finalResume += (int)fieldInfo.GetValue(plantAttribute) * LevelWindResume;
                    break;
                // 4 ����
                case 4:
                    finalWindage += (int)fieldInfo.GetValue(plantAttribute) * LevelWindSpeed;
                    if (finalWindage > 1)
                    {
                        finalWindage = 1 + (finalWindage - 1) / 10;
                    }
                    break;
                default:
                    break;
            }
        }

        GardenManager.Instance.Windage = finalWindage;
        GardenManager.Instance.Windspeed = finalWindSpeed;
        GardenManager.Instance.BloverResume = finalResume;
        animator.speed = Random.Range(0.8f, 1.2f);
    }
}
