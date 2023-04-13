using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Gravebuster : Plant
{
    public override PlantType PlantType => PlantType.Gravebuster;

    [Tooltip("������ȴʱ��")]
    public float CoolTime = 12f;
    [Tooltip("Ĭ�����ɸ���")]
    public int SwallowRate = 10;
    [Tooltip("Ǯ��Ԥ֪�壬ת����Ǯ��ֱ���ռ�")]
    public Coin Coin;
    public AudioSource audioSource;
    public GameObject Grave;

    private int finalSwallowRate;
    private float coinConversionRate;
    private float finalCoolTime;
    private float timer;

    private readonly float LevelDamage = 0.05f;
    private readonly float LevelCoolTime = 0.4f;
    private readonly int LevelRate = 2;
    private readonly float LevelPercentage = 0.1f;

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    public override void Reuse()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        float finalDamage = GardenManager.Instance.GravebusterDamage;
        finalSwallowRate = SwallowRate;
        finalCoolTime = CoolTime;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // �ֶ�ӳ��
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 ���ɸ���
                case 0:
                    finalSwallowRate += (int)fieldInfo.GetValue(plantAttribute) * LevelRate;
                    break;
                // 1 ����
                case 1:
                    finalDamage += (int)fieldInfo.GetValue(plantAttribute) * LevelDamage;
                    break;
                // ���ת����
                case 2:
                    coinConversionRate = (int)fieldInfo.GetValue(plantAttribute) * LevelPercentage;
                    break;
                // 6 ��ȴʱ��
                case 6:
                    finalCoolTime -= (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                default:
                    break;
            }
        }

        GardenManager.Instance.GravebusterDamage = finalDamage;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// ����ʬʱ�ж��ܲ�������
    /// </summary>
    public bool CanSwallow()
    {
        bool result = false;
        if (Time.time - timer > finalCoolTime)
        {
            result = Random.Range(0, 100) < finalSwallowRate ? true : false;
            if (result)
            {
                audioSource.Play();
                timer = Time.time;
            }              
        }
        return result;
    }

    private void Update()
    {
        this.transform.Translate(Vector3.down * 0.6f * Time.deltaTime);
        Grave.transform.Translate(Vector3.up * 0.6f * Time.deltaTime);
    }

    public void SetLayer(float y, int health)
    {
        int sortingOrder = (int)((-y + 10) * 10);
        spriteRenderer.sortingOrder = sortingOrder;
        Grave.transform.localPosition = new Vector3(0, -0.844f, 0);
        StartCoroutine("CoinCreate", health);
    }

    IEnumerator CoinCreate(int health)
    {
        yield return new WaitForSeconds(1);
        if (coinConversionRate != 0)
        {
            var coinItem = GameObject.Instantiate(Coin, this.transform);
            coinItem.Price = (int)(coinConversionRate * health * 2);
            coinItem.Digest();
        }
        timer = Time.time;
        this.gameObject.SetActive(false); 
    }
}
