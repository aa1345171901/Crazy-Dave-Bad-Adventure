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
    protected float SplashSizeX = 3;
    protected float SplashSizeY = 1;

    private bool isTrigger;

    private readonly float MaxLiveTime = 15;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
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
            DoDamage(health, Damage);
            spriteRenderer.enabled = false;
            bulletParticleSystem.Play();
            Invoke("DestroyPeaBullet", 1);
            audioSource.clip = Random.Range(0, 2) == 0 ? hit1 : hit2;
            audioSource.Play();

            // �������0�򴥷�
            if (SplashPercentage > 0)
            {
                // ��Ϊǰ���1*1����Ǵ����û�˵�, �����ǶԸ�λ�õĺ�1*1��������˺���
                var colliders = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(SplashSizeX, SplashSizeY), 0, TargetLayer);
                foreach (var item in colliders)
                {
                    // ��ʬ������collider��ֻ��trigger�Ǹ�����˺�
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
