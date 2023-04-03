using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class PeaBullet : MonoBehaviour
{
    public float Speed;
    public int Damage = 5;
    public LayerMask TargetLayer;
    public ParticleSystem bulletParticleSystem;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;
    public AudioClip hit1;
    public AudioClip hit2;

    public float SplashPercentage;

    private bool isTrigger;

    private readonly float MaxLiveTime = 15;

    private void Start()
    {
        Invoke("DestroyPeaBullet", MaxLiveTime);
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger)
            return;
        if (TargetLayer.Contains(collision.gameObject.layer))
        {
            isTrigger = true;
            var health = collision.GetComponent<Health>();
            health.DoDamage(Damage, DamageType.PeaBullet);
            spriteRenderer.enabled = false;
            bulletParticleSystem.Play();
            Invoke("DestroyPeaBullet", 1);
            audioSource.clip = Random.Range(0, 2) == 0 ? hit1 : hit2;
            audioSource.Play();

            // 溅射大于0则触发
            if (SplashPercentage > 0)
            {
                // 因为前面的1*1面积是大概率没人的, 所有是对该位置的后1*1方格造成伤害，
                var colliders = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(3, 1), 0, TargetLayer);
                foreach (var item in colliders)
                {
                    // 僵尸有两个collider，只对trigger那个造成伤害
                    if (item != collision && item.isTrigger)
                    {
                        var health2 = item.GetComponent<Health>();
                        health2.DoDamage((int)(Damage * SplashPercentage / 100));
                    }
                }
            }
        }
    }

    private void DestroyPeaBullet()
    {
        Destroy(this.gameObject);
    }
}
