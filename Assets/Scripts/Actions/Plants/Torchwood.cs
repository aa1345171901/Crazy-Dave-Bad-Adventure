using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Torchwood : Plant
{
    public override PlantType PlantType => PlantType.Torchwood;

    private readonly float LevelAdd = 0.1f;
    private readonly float defaultDamageAdd = 1f;  // 豌豆伤害默认翻倍

    public override void Reuse()
    {
        if (spriteRenderer == null)
            spriteRenderer = this.GetComponent<SpriteRenderer>();

        var levelBounds = LevelManager.Instance.LevelBounds;
        float x = levelBounds.max.x;
        // 0.5 刚好站在格子上
        float randomY = (int)Random.Range(levelBounds.min.y, levelBounds.max.y - 0.5f) + 0.5f;
        this.transform.position = new Vector3(x, randomY, 0);
        int y = (int)((-randomY + 10) * 10);
        spriteRenderer.sortingOrder = y;
        FacingDirections = FacingDirections.Left;

        float finalDamageAdd = defaultDamageAdd + GardenManager.Instance.TorchwoodEffect.DamageAdd;
        float finalSplashDamage = GardenManager.Instance.TorchwoodEffect.SplashDamage;
        float finalPeaSpeed = GardenManager.Instance.TorchwoodEffect.PeaSpeed;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 增伤
                case 0:
                    finalDamageAdd += (int)fieldInfo.GetValue(plantAttribute) * LevelAdd;
                    break;
                // 1 溅射伤害
                case 1:
                    finalSplashDamage += (int)fieldInfo.GetValue(plantAttribute) * LevelAdd;
                    break;
                // 2 速度
                case 2:
                    finalPeaSpeed += (int)fieldInfo.GetValue(plantAttribute) * LevelAdd;
                    break;
                default:
                    break;
            }
        }

        GardenManager.Instance.TorchwoodEffect.DamageAdd = finalDamageAdd;
        GardenManager.Instance.TorchwoodEffect.SplashDamage = finalSplashDamage;
        GardenManager.Instance.TorchwoodEffect.PeaSpeed = finalPeaSpeed;
        animator.speed = Random.Range(0.8f, 1.2f);
    }
}
