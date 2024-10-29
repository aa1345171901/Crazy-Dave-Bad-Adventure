using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Blover : Plant
{
    public override PlantType PlantType => PlantType.Blover;

    private readonly float LevelWindSpeed = 0.1f;
    private readonly int LevelWindResume = 1;
    private readonly float defaultWindSpeed = 0.2f; // 默认风速
    private readonly float defaultWindage = 0.2f; // 默认风阻

    public override void Reuse(bool randomPos = true)
    {
        if (spriteRenderer == null)
            spriteRenderer = this.GetComponent<SpriteRenderer>();

        if (randomPos)
        {
            var levelBounds = LevelManager.Instance.LevelBounds;
            float x = levelBounds.min.x;
            // 0.5 刚好站在格子上
            float randomY = (int)Random.Range(levelBounds.min.y, levelBounds.max.y - 0.5f) + 0.5f;
            this.transform.position = new Vector3(x, randomY, 0);
            int y = (int)((-randomY + 10) * 10);
            spriteRenderer.sortingOrder = y;
            FacingDirections = FacingDirections.Right;
        }

        float finalWindSpeed = defaultWindSpeed + GardenManager.Instance.BloverEffect.Windspeed;
        int finalResume = GardenManager.Instance.BloverEffect.BloverResume;
        float finalWindage = defaultWindage + GardenManager.Instance.BloverEffect.Windage;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 2 风速
                case 2:
                    finalWindSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelWindSpeed;
                    break;
                // 3 恢复
                case 3:
                    finalResume += (int)fieldInfo.GetValue(plantAttribute) * LevelWindResume;
                    break;
                // 4 风阻
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

        GardenManager.Instance.BloverEffect.Windage = finalWindage;
        GardenManager.Instance.BloverEffect.Windspeed = finalWindSpeed;
        GardenManager.Instance.BloverEffect.BloverResume = finalResume;
        animator.speed = Random.Range(0.8f, 1.2f);
    }
}
