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

    private List<Health> healths = new List<Health>();
    public bool isCritical;

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
        if (healths.Count >= penetrationCount)
            return;
        if (TargetLayer.Contains(collision.gameObject.layer) && collision.isTrigger)
        {
            var health = collision.GetComponent<Health>();
            health.DoDamage(Damage, DamageType.Cactus, isCritical);
            audioSource.clip = Random.Range(0, 2) == 0 ? hit1 : hit2;
            audioSource.Play();
            healths.Add(health);
            if (healths.Count >= penetrationCount)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
