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

    private readonly float AttackAnimTime = 0.33f; // 攻击动画0.33f

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

    private void OnDestroy()
    {
        AudioManager.Instance.AudioLists.Remove(this.audioSource);
    }

    public override void ProcessAbility()
    {
        bool hasDown = false;
#if UNITY_ANDROID
        hasDown = Input.touchCount > 0;
#else
        hasDown = Input.GetMouseButtonDown(0);
#endif

        if (hasDown && Time.time - lastAttackTimer >= finalAttackCoolingTime)
        {
            this.gameObject.SetActive(true);
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
            lastAttackTimer = Time.time;
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
            var health = trigger2D.GetFirst(false)?.GetComponent<Health>();
            if (health)
            {
                health.DoDamage(finalDamage, DamageType.Hammer);
            }
            // 只对一只僵尸造成伤害
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
