using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public enum BulletType
{
    Pea,
    Shroom,
    Star,
    Snow,
}

public class PeaBullet : MonoBehaviour
{
    public virtual BulletType BulletType => BulletType.Pea;

    public float Speed;
    public int Damage = 5;
    public LayerMask TargetLayer;
    public ParticleSystem bulletParticleSystem;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;
    public AudioClip hit1;
    public AudioClip hit2;

    public Animator animator;

    public float SplashPercentage;
    protected float SplashSizeX = 3;
    protected float SplashSizeY = 1;

    private bool isTrigger;

    private readonly float MaxLiveTime = 15;

    protected Vector3 direction;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Invoke("DestroyPeaBullet", MaxLiveTime);
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        direction = Vector3.right;
        if (BulletType == BulletType.Pea)
        {
            float damageAdd = GardenManager.Instance.TorchwoodEffect.DamageAdd;
            Debug.Log(damageAdd);
            if (damageAdd <= 1)
            {

            }
            else if (damageAdd > 1 && damageAdd <= 2)
            {
                animator.SetBool("1", true);
            }
            else if (damageAdd <= 3)
            {
                animator.SetBool("2", true);
            }
            else if (damageAdd > 3)
            {
                animator.SetBool("3", true);
            }
            Damage = (int)(Damage * damageAdd);
            SplashPercentage += GardenManager.Instance.TorchwoodEffect.SplashDamage;
            Speed *= GardenManager.Instance.TorchwoodEffect.PeaSpeed;
        }
    }

    private void Update()
    {
        transform.Translate(direction * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger)
            return;
        if (TargetLayer.Contains(collision.gameObject.layer))
        {
            isTrigger = true;
            var health = collision.GetComponent<Health>();
            DoDamage(health, Damage);
            spriteRenderer.enabled = false;
            bulletParticleSystem.Play();
            Invoke("DestroyPeaBullet", 1);
            audioSource.clip = Random.Range(0, 2) == 0 ? hit1 : hit2;
            audioSource.Play();

            // 溅射大于0则触发
            if (SplashPercentage > 0)
            {
                // 因为前面的1*1面积是大概率没人的, 所有是对该位置的后1*1方格造成伤害，
                var colliders = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(SplashSizeX, SplashSizeY), 0, TargetLayer);
                foreach (var item in colliders)
                {
                    // 僵尸有两个collider，只对trigger那个造成伤害
                    if (item != collision && item.isTrigger)
                    {
                        var health2 = item.GetComponent<Health>();
                        DoDamage(health2, (int)(Damage * SplashPercentage / 100));
                    }
                }
            }
        }
    }

    protected virtual void DoDamage(Health health, int damage)
    {
        health.DoDamage(Damage, DamageType.PlantBullet);
    }

    protected virtual void DestroyPeaBullet()
    {
        GameObject.Destroy(this.gameObject);
    }
}
