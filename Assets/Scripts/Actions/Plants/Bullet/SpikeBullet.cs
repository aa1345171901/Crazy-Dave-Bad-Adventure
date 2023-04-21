using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class SpikeBullet : MonoBehaviour
{
    public float Speed = 5;
    public int Damage = 5;
    public float penetrationCount;
    public LayerMask TargetLayer;
    public AudioSource audioSource;
    public AudioClip hit1;
    public AudioClip hit2;

    /// <summary>
    /// 目标Health以及上次受伤时间
    /// </summary>
    private Dictionary<Health, float> healths = new Dictionary<Health, float>();
    public bool isCritical;
    public int damageCount;

    private readonly float MaxLiveTime = 15;

    private void Start()
    {
        Invoke("DestroyBullet", MaxLiveTime);
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageCount >= penetrationCount)
            return;

        if (TargetLayer.Contains(collision.gameObject.layer) && collision.isTrigger)
        {
            var health = collision.GetComponent<Health>();
            if (healths.ContainsKey(health))
            {
                if (Time.time - healths[health] > 1)
                {
                    DoDamage(health);
                }
            }
            else
            {
                DoDamage(health);
                healths.Add(health, Time.time);
            }
            if (damageCount >= penetrationCount)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    private void DoDamage(Health health)
    {
        health.DoDamage(Damage, DamageType.Cactus, isCritical);
        audioSource.clip = Random.Range(0, 2) == 0 ? hit1 : hit2;
        audioSource.Play();
        damageCount++;
    }

    private void DestroyBullet()
    {
        GameObject.Destroy(this.gameObject);
    }
}
