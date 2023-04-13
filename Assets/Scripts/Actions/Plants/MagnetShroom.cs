using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class MagnetShroom : Plant
{
    public override PlantType PlantType => PlantType.MagentShroom;

    [Tooltip("吸取铁制品默认个数")]
    public int CutterCount = 1;
    [Tooltip("吸收冷却时间")]
    public float CoolTime = 12f;
    [Tooltip("吸收持续时间")]
    public float DurationTime = 2f;
    public AudioSource audioSource;

    private float timer;
    private bool isAbsorbing;  // 正在吸取

    private int finalCount;
    private float finalCoolTime;
    private float finalDurationTime;
    private int finalChangeCoin;

    private readonly float LevelCutterCount = 0.34f;
    private readonly float LevelTime = 0.4f;
    private readonly int LevelCoin = 10;

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
        finalCount = CutterCount;
        finalDurationTime = DurationTime;
        finalChangeCoin = 0;
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
                // 3 换取金币数
                case 3:
                    finalChangeCoin += (int)fieldInfo.GetValue(plantAttribute) * LevelCoin;
                    break;
                // 4 吸取个数
                case 4:
                    int level = (int)fieldInfo.GetValue(plantAttribute);
                    finalCount += (int)(level * LevelCutterCount);
                    if (level == 10)
                        finalCount = 5;
                    break;
                // 5 持续时间
                case 5:
                    finalDurationTime += (int)fieldInfo.GetValue(plantAttribute) * LevelTime / 2;
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        if (!isAbsorbing && Time.time - timer > finalCoolTime)
        {
            animator.SetBool("IsCoolling", false);
            // todo 吸铁判断
            if (false)
            {
                audioSource.Play();
                timer = Time.time;
                animator.SetBool("IsAttack", true);
                Invoke("StartAbsorbing", 0.5f);
            }
        }
        if (isAbsorbing && Time.time - timer > finalDurationTime)
        {
            timer = Time.time;
            isAbsorbing = false;
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsCoolling", true);
        }
        if (isAbsorbing)
        {
            // 增加铁制品
            //if (coins.Count < finalCount)
            //{
            //    var allCoin = GameManager.Instance.Coins;
            //    if (allCoin.Count > 0)
            //    {
            //        int index = Random.Range(0, allCoin.Count);
            //        var coin = allCoin[index];
            //        allCoin.Remove(coin);
            //        coins.Add(coin.transform.position, coin);
            //    }
            //}

            //foreach (var item in coins)
            //{
            //    float process = (Time.time - timer) / finalDurationTime;
            //    var lerp = Vector3.Lerp(item.Key, transform.position, process);
            //    item.Value.transform.position = new Vector3(lerp.x, lerp.y, 0);
            //}
        }
    }

    private void StartAbsorbing()
    {
        isAbsorbing = true;
        timer = Time.time;
    }
}
