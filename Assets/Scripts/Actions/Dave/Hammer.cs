using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Hammer : BaseProp
{
    public AudioSource audioSource;
    public AudioClip hammerTrue;
    public AudioClip hammerFalse;

    private Trigger2D trigger2D;
    private BoxCollider2D damageTrigger;
    private Animator animator;
    private float lastAttackTimer;

    private int finalDamage;
    private float finalAttackCoolingTime;

    private readonly float AttackAnimTime = 0.33f; // ��������0.33f

    private void Start()
    {
        animator = GetComponent<Animator>();
        damageTrigger = GetComponent<BoxCollider2D>();
        trigger2D = GetComponent<Trigger2D>();
        SetActiveFalse();
    }

    public override void Reuse()
    {
        var userData = GameManager.Instance.UserData;
        finalDamage = Mathf.RoundToInt((userData.Power * 1.5f + DefaultDamage)  * (100f + userData.PercentageDamage) / 100);
        finalAttackCoolingTime = DefaultAttackCoolingTime - (100f + userData.AttackSpeed) / 100;
        finalAttackCoolingTime = finalAttackCoolingTime < 0.5f ? 0.5f : finalAttackCoolingTime;

        if (!AudioManager.Instance.AudioLists.Contains(this.audioSource))
        {
            AudioManager.Instance.AudioLists.Add(this.audioSource);
            this.audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        }
    }

    public override void ProcessAbility()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTimer >= finalAttackCoolingTime)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
            lastAttackTimer = Time.time;
            this.gameObject.SetActive(true);
            damageTrigger.enabled = true;
            trigger2D.enabled = true;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
            animator.SetTrigger("Attack");
            Invoke("SetActiveFalse", AttackAnimTime);
        }

        if (trigger2D.IsTrigger && trigger2D.enabled)
        {
            audioSource.PlayOneShot(hammerTrue);
            var health = trigger2D.Target.GetComponent<Health>();
            if (health)
            {
                health.DoDamage(finalDamage, DamageType.Hammer);
            }
            // ֻ��һֻ��ʬ����˺�
            trigger2D.enabled = false;
        }
    }

    private void SetActiveFalse()
    {
        this.gameObject.SetActive(false);
        damageTrigger.enabled = false;
        trigger2D.enabled = false;
    }
}
