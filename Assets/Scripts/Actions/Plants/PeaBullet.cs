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
        }
    }

    private void DestroyPeaBullet()
    {
        Destroy(this.gameObject);
    }
}
