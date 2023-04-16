using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallNut : Plant
{
    public override PlantType PlantType => PlantType.TallNut;

    [Tooltip("����ֵ")]
    public int Health = 30;
    public Sun Sun;

    public AudioSource audioSource;

    private float timer;

    private float finalCoolTime;
    private float finalTwinRate;
    private int finalQuality;

    private List<ItemJump> itemJumps = new List<ItemJump>();  // Ǯ��������������

    private readonly float LevelRate = 0.03f;
    private readonly float LevelTime = 0.6f;

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    public override void Reuse()
    {
        base.Reuse();

        // ����˳����Ҫ��PlantCultivationPage��Ƶ��������Ӧ
        finalCoolTime = CoolTime;
        finalTwinRate = 0;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 2 ��ȴʱ��
                case 2:
                    finalCoolTime -= (int)fieldInfo.GetValue(plantAttribute) * LevelTime;
                    break;
                // 3 ��������
                case 3:
                    finalQuality = (int)fieldInfo.GetValue(plantAttribute);
                    break;
                // 4 ����˫������
                case 4:
                    finalTwinRate += (int)fieldInfo.GetValue(plantAttribute) * LevelRate;
                    break;
                default:
                    break;
            }
        }
    }
}
