using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class SmellyFart : BaseProp
{
    [Tooltip("造成伤害的LayerMask")]
    public LayerMask targetLayer;

    public AudioSource audioSource;

    private int finalDamage;
    private float finalDecelerated;

    private float timer;
    private bool haveDecelerated;

    /// <summary>
    /// 当前造成伤害的时间以及对应的目标Key
    /// </summary>
    private Dictionary<Health, float> healthDict = new Dictionary<Health, float>();

    public override void Reuse()
    {
        base.Reuse();
        var userData = GameManager.Instance.UserData;
        finalDamage = Mathf.RoundToInt((userData.Botany / 5f + DefaultDamage) * (100f + userData.PercentageDamage) / 100);
        finalDecelerated = 0;
        var lifeTime = ShopManager.Instance.PurchasePropCount("purpleGarlic");
        var confMint = ConfManager.Instance.confMgr.propCards.GetItemByName("mint");
        if (confMint != null)
        {
            finalDecelerated = (ShopManager.Instance.PurchasePropCount("mint") * confMint.value1) / 100f;
        }
        haveDecelerated = finalDecelerated > 0;
        foreach (var item in GetComponentsInChildren<ParticleSystem>())
        {
            ParticleSystem.MainModule mainModule = item.main;
            mainModule.startLifetime = lifeTime;
        }
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();

        timer += Time.deltaTime;
        // 0.5f检测一次粒子范围伤害
        if (timer > 0.5f)
        {
            timer = 0;
            foreach (var particleSystem in GetComponentsInChildren<ParticleSystem>())
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
                int particleCount = particleSystem.GetParticles(particles);

                for (int i = 0; i < particleCount; i++)
                {
                    Vector3 position = particles[i].position;
                    float radius = particles[i].GetCurrentSize(particleSystem); // 获取每个粒子的大小

                    // 检测范围内的碰撞体
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius / 2, targetLayer);
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.isTrigger)
                        {
                            var health = collider.GetComponent<Health>();
                            if (health)
                            {
                                void doDamage()
                                {
                                    healthDict[health] = Time.time;
                                    health.DoDamage(finalDamage, DamageType.SmellyFart); 
                                    if (!audioSource.isPlaying)
                                        audioSource.Play();
                                    if (haveDecelerated)
                                    {
                                        var zombie = collider.GetComponent<AIMove>();
                                        if (zombie)
                                            zombie.BeDecelerated(finalDecelerated, 1);
                                    }
                                }

                                if (!healthDict.ContainsKey(health))
                                {
                                    doDamage();
                                }
                                // 大于间隔时间再次受伤
                                else if (Time.time - healthDict[health] > DefaultAttackCoolingTime)
                                {
                                    doDamage();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        if (!AudioManager.Instance.AudioLists.Contains(this.audioSource))
        {
            AudioManager.Instance.AudioLists.Add(this.audioSource);
            this.audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        }
    }

    private void OnDisable()
    {
        AudioManager.Instance.AudioLists.Remove(this.audioSource);
    }
}
