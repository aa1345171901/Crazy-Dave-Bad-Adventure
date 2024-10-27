using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VocalConcert : BaseProp
{
    [Tooltip("雾机")]
    public GameObject Fogmachine;
    [Tooltip("麦克风")]
    public GameObject Mic;
    [Tooltip("灯光机")]
    public GameObject Lights;
    [Tooltip("音响")]
    public GameObject Speaker;
    [Tooltip("光线")]
    public List<GameObject> LightItems;

    public Light2D blueLight;
    [Tooltip("雾")]
    public ParticleSystem fog;

    [Tooltip("集齐演唱会套装后，每秒造成伤害的攻击范围")]
    public float DefaultRange = 2;
    [Tooltip("显示攻击范围")]
    public GameObject range;
    [Tooltip("攻击的对象Layer")]
    public LayerMask attackLayer;

    [Tooltip("音乐会使僵尸受伤的声音")]
    public AudioSource audioSource;

    /// <summary>
    /// 是否开启了演唱会
    /// </summary>
    public bool OpenVocalConcert { get; private set; }

    public bool isFog;
    private float fogTimer;
    private float micY;
    private Vector3 targetPos;

    private int finalDamage;
    private float incrementRange = 1;
    private float vocalConcertTimer;
    private Vector3 defaultRangeScale;

    private Character character;

    private float[] spectrum = new float[64];
    private readonly float FogDuration = 5;

    private void Awake()
    {
        Fogmachine.SetActive(false);
        Mic.SetActive(false);
        Lights.SetActive(false);
        Speaker.SetActive(false);
        range.SetActive(false);
        foreach (var item in LightItems)
        {
            item.SetActive(false);
        }
        blueLight.gameObject.SetActive(false);
        micY = Mic.transform.localPosition.y;
        DefaultAttackCoolingTime = 1;  // 每秒对周围僵尸造成伤害
        DefaultDamage = 10;
        defaultRangeScale = range.transform.localScale;
        character = GetComponentInParent<Character>();
    }

    public override void Reuse()
    {
        base.Reuse();
        var shop = ShopManager.Instance;
        Fogmachine.SetActive(shop.PurchasePropCount("fogmachine") >= 1);
        Mic.SetActive(shop.PurchasePropCount("mic") >= 1);
        Lights.SetActive(shop.PurchasePropCount("lights") >= 1);
        Speaker.SetActive(shop.PurchasePropCount("speaker") >= 1);
        OpenVocalConcert = Fogmachine.activeSelf && Mic.activeSelf && Lights.activeSelf && Speaker.activeSelf;
        range.SetActive(OpenVocalConcert);
        if (OpenVocalConcert)
        {
            AudioManager.Instance.PlayVocalConcertMusic(2);
            blueLight.gameObject.SetActive(true);
            this.audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
            character.SkeletonAnimation.AnimationState.SetAnimation(2, "Speak", true);
        }

        var userData = GameManager.Instance.UserData;
        finalDamage = Mathf.RoundToInt(DefaultDamage * 5 * (100f + userData.PercentageDamage) / 100);
        incrementRange =  (200 + userData.Range) / 200f;
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
        // 获取频谱
        AudioManager.Instance.BackmusicPlayer.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxSpectrum = GetMaxByArray.GetMaxSpectrum(spectrum);
        if (AudioManager.Instance.BackmusicPlayer.volume != 0)
            maxSpectrum /= AudioManager.Instance.BackmusicPlayer.volume;

        // 音响
        float scale = maxSpectrum + 1;
        Speaker.transform.localScale = new Vector3(scale, scale, scale);

        // 雾机
        if (Fogmachine.activeSelf)
        {
            if (maxSpectrum > 0.3f && !isFog)
            {
                isFog = true;
                fogTimer = Time.time;
                fog.Play();
            }
            if (isFog && Time.time - fogTimer > FogDuration)
            {
                isFog = false;
                fog.Stop();
            }
        }

        if (!OpenVocalConcert)
            return;
        // 灯光
        for (int i = 0; i < LightItems.Count; i++)
        {
            if (i <= 1)
                LightItems[i].SetActive(maxSpectrum > 0.1f && maxSpectrum <= 0.2f);
            else
                LightItems[i].SetActive(maxSpectrum > 0.2f);
        }
        blueLight.intensity = 0.3f + maxSpectrum * 2;

        // 麦克风
        if (maxSpectrum > 0.2f)
        {
            targetPos = new Vector3(Mic.transform.localPosition.x, micY + maxSpectrum * 3, 0);
        }
        if (targetPos != Vector3.zero)
        {
            Mic.transform.Translate((targetPos - Mic.transform.localPosition) * 6 * Time.deltaTime, Space.Self);
            if ((targetPos - Mic.transform.localPosition).magnitude < 0.1f)
            {
                targetPos = Vector3.zero;
            }
        }
        else
        {
            Mic.transform.Translate((new Vector3(Mic.transform.localPosition.x, micY) - Mic.transform.localPosition) * 3 * Time.deltaTime, Space.Self);
        }

        // 演唱会
        float realityRange = incrementRange * (maxSpectrum + DefaultRange);
        range.transform.localScale = defaultRangeScale * realityRange / DefaultRange;
        if (Time.time - vocalConcertTimer > DefaultAttackCoolingTime)
        {
            vocalConcertTimer = Time.time;
            var colliders = Physics2D.OverlapCircleAll(this.transform.position, realityRange, attackLayer);
            foreach (var item in colliders)
            {
                if (item.isTrigger)
                {
                    var health = item.GetComponent<Health>();
                    if (health)
                    {
                        audioSource.pitch = Random.Range(0.9f, 1.1f);
                        audioSource.Play();
                        health.DoDamage(finalDamage, DamageType.VocalConcert);
                    }
                }
            }
        }
    }

    public override void DayEnd()
    {
        foreach (var item in LightItems)
        {
            item.SetActive(false);
        }
        blueLight.intensity = 0;
    }
}

public static class GetMaxByArray
{
    /// <summary>
    /// 从频谱中获取频率中分贝最大的，做简单的音乐可视化 
    /// 频谱数组大小表示采样分辨率最小64 最大8196，为2的次幂
    /// 频谱数组对应某赫兹下的谱密度
    /// </summary>
    /// <returns></returns>
    public static float GetMaxSpectrum(float[] spectrum)
    {
        float result = 0;
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > result)
                result = spectrum[i];
        }
        return result;
    }
}
