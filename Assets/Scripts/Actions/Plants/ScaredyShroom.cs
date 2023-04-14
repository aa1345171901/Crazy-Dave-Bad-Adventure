using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ScaredyShroom : PuffShroom
{
    public override PlantType PlantType => PlantType.ScaredyShroom;

    public AudioSource audioSource;

    private Dictionary<Collider2D, float> colliderDict = new Dictionary<Collider2D, float>();  // ��ʬ�Լ���ʬ�ϴ�����ʱ��

    private bool isCrying;

    public override void Reuse()
    {
        base.Reuse();
        colliderDict.Clear();
    }

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    private void Update()
    {
        var hit = Physics2D.OverlapCircle(this.transform.position, 2, TargetLayer);
        if (hit)
        {
            animator.SetBool("IsCrying", true);
            isCrying = true;
        }
        else
        {
            animator.SetBool("IsCrying", false);
            isCrying = false;
        }
        if (!isCrying)
        {
            if (Time.time - timer > finalCoolTime)
            {
                hit = Physics2D.OverlapBox(pos, size, 0, TargetLayer);
                if (hit)
                {
                    timer = Time.time;
                    StartCoroutine(CreateBullet());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (colliderDict.ContainsKey(collision))
        {
            // ÿ������˺�
            if (Time.time - colliderDict[collision] > 1)
            {
                var health = collision.GetComponent<Health>();
                if (health)
                {
                    audioSource.Play();
                    health.DoDamage(finalDamage, DamageType.ScaredyShroom);
                    colliderDict[collision] = Time.time;
                }
            }
        }
        else
        {
            if (TargetLayer.Contains(collision.gameObject.layer))
            {
                var health = collision.GetComponent<Health>();
                if (health)
                {
                    health.DoDamage(finalDamage, DamageType.ScaredyShroom);
                    audioSource.Play();
                    colliderDict.Add(collision, Time.time);
                }
            }
        }
    }
}
