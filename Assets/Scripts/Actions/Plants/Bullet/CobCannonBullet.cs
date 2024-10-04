using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class CobCannonBullet : MonoBehaviour
{
    [Tooltip("攻击目标")]
    public LayerMask TargetLayer;
    public ParticleSystem bulletParticleSystem;
    public AudioSource audioSource;
    [Tooltip("阳光预知体，转换的阳光直接收集")]
    public Sun sun;

    public float finalRange { get; set; }
    public float immediateMortalityRate { get; set; }
    public float increasedInjury { get; set; }
    public int finalDamage { get; set; }
    public float sunConversionRate { get; set; }

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        StartCoroutine(DelayBoom());
    }

    IEnumerator DelayBoom()
    {
        yield return new WaitForSeconds(1f);
        audioSource.Play();
        int sumHealth = 0;
        var colliders = Physics2D.OverlapCircleAll(this.transform.position, finalRange, TargetLayer);
        DoDamage(colliders, ref sumHealth);

        if (sunConversionRate != 0)
        {
            var sunItem = GameObject.Instantiate(sun, this.transform);
            sunItem.Price = (int)(sunConversionRate * sumHealth * 5);
            sunItem.Digest();
        }
        yield return new WaitForSeconds(2);
        GameObject.Destroy(this.gameObject);
    }

    protected void DoDamage(Collider2D[] colliders, ref int sumHealth)
    {
        foreach (var item in colliders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                if (health)
                {
                    float random = Random.Range(0, 1f);
                    // 立即死亡
                    if (random < immediateMortalityRate && item.tag != "BigZombie")
                    {
                        sumHealth += health.maxHealth;
                        health.DoDamage(health.maxHealth, DamageType.Bomb, true);
                    }
                    else
                    {
                        if (increasedInjury > 0 && item.tag == "BigZombie")
                        {
                            int damage = (int)(finalDamage * increasedInjury);
                            sumHealth += damage > health.health ? health.health : damage;
                            health.DoDamage(damage, DamageType.Bomb);
                        }
                        else
                        {
                            sumHealth += finalDamage > health.health ? health.health : finalDamage;
                            health.DoDamage(finalDamage, DamageType.Bomb);
                        }
                    }
                }
            }
        }
    }
}
