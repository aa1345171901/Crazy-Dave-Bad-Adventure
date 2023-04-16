using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class SunFlower : Plant
{
    public override PlantType PlantType => PlantType.SunFlower;

    [Tooltip("冷却时间")]
    public float CoolTime = 12f;
    public Sun Sun;

    public AudioSource audioSource;

    private float timer;

    private float finalCoolTime;
    private float finalTwinRate;
    private int finalQuality;

    private List<ItemJump> itemJumps = new List<ItemJump>();  // 钱币跳出动画参数

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

        // 属性顺序需要与PlantCultivationPage设计的文字相对应
        finalCoolTime = CoolTime;
        finalTwinRate = 0;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 2 冷却时间
                case 2:
                    finalCoolTime -= (int)fieldInfo.GetValue(plantAttribute) * LevelTime;
                    break;
                // 3 阳光质量
                case 3:
                    finalQuality = (int)fieldInfo.GetValue(plantAttribute);
                    break;
                // 4 掉落双倍概率
                case 4:
                    finalTwinRate += (int)fieldInfo.GetValue(plantAttribute) * LevelRate;
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            itemJumps.Clear();
            timer = Time.time;
            animator.SetTrigger("Attack");
            Invoke("DelayCreate", 0.5f);
        }
        foreach (var item in itemJumps)
        {
            if (Time.time - timer < item.time)
            {
                Vector3 newPos = this.transform.position + item.offsetSpeed * (Time.time - timer);
                newPos.y += Mathf.Cos((Time.time - timer) / item.time * Mathf.PI - Mathf.PI / 2) * item.height;
                item.item.transform.position = newPos;
            }
        }
    }

    protected virtual void DelayCreate()
    {
        audioSource.Play();
        CreateSun();
        if (finalTwinRate > 0)
        {
            if (Random.Range(0, 1f) < finalTwinRate)
                CreateSun();
        }
    }

    private void CreateSun()
    {
        var sun = GameObject.Instantiate(Sun);
        sun.transform.position = this.transform.position;
        var itemJump = new ItemJump(sun);
        Vector3 offset = new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-1, 1), sun.transform.rotation.z);
        itemJump.height = Random.Range(0.3f, 0.6f);
        itemJump.time = Random.Range(0.4f, 0.6f);
        itemJump.offsetSpeed = offset / itemJump.time;
        itemJumps.Add(itemJump);
        timer = Time.time;

        sun.Price = 25 + 5 * finalQuality;
        float scale = 1 + finalQuality / 10f;
        sun.transform.localScale = Vector3.one * scale;
    }
}
