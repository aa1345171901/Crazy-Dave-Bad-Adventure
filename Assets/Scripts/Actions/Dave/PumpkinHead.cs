using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinHead : BaseProp
{
    public LifeResumeProp LifeResumePumpkin;

    public bool HasPumpkinHead { get; set; }
    public float PumpkinHeadResumeLife { get; set; }
    public float PumpkinAttackRate { get; set; }

    private readonly float LevelPercentage = 0.05f;
    private readonly float LevelAttackRate = 0.01f;

    private List<LifeResumeProp> lifeResumeProps = new List<LifeResumeProp>();

    public override void Reuse()
    {
        base.Reuse();
        HasPumpkinHead = false;
        PumpkinHeadResumeLife = 0.1f;
        PumpkinAttackRate = 0;
        var plants = GardenManager.Instance.PlantAttributes;

        float maxLifeResume = 0;
        foreach (var item in plants)
        {
            if (item.plantCard.plantType == PlantType.PumpkinHead)
            {
                HasPumpkinHead = true;
                int[] attributes = item.attribute;
                for (int i = 0; i < attributes.Length; i++)
                {
                    // ×Ö¶ÎÓ³Éä
                    var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
                    switch (attributes[i])
                    {
                        // 0 ´¥·¢ºóÉúÃüÖµ»Ö¸´±ÈÀý
                        case 0:
                            float lifeResume = (int)fieldInfo.GetValue(item) * LevelPercentage;
                            if (lifeResume > maxLifeResume)
                                maxLifeResume = lifeResume;
                            break;
                        // 4 Îª¹¥»÷µôÂäÄÏ¹Ï¸ÅÂÊ
                        case 4:
                            PumpkinAttackRate += (int)fieldInfo.GetValue(item) * LevelAttackRate;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        PumpkinHeadResumeLife = maxLifeResume;
        this.gameObject.SetActive(HasPumpkinHead);

        foreach (var item in lifeResumeProps)
        {
            GameObject.Destroy(item.gameObject);
        }
        lifeResumeProps.Clear();
    }

    public void Falling(Vector3 targetPos)
    {
        if (HasPumpkinHead)
        {
            if (Random.Range(0, 1f) < PumpkinAttackRate)
            {
                var pumpkin = GameObject.Instantiate(LifeResumePumpkin);
                pumpkin.transform.position = targetPos;
                lifeResumeProps.Add(pumpkin);
            }
        }
    }
}
