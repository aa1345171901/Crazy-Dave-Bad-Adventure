using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class DeathGod : BaseProp
{
    public float range;
    public LayerMask layerMask;

    private float lastAttackTimer;
    Animator animator;
    AudioSource audioSource;
    bool isAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    private void OnDestroy()
    {
        AudioManager.Instance.AudioLists.Remove(audioSource);
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();

        var player = GameManager.Instance.Player;
        if (Time.time - lastAttackTimer > DefaultAttackCoolingTime && !isAttack)
        {
            var lifeRate = (float)player.Health.health / player.Health.maxHealth;
            if (lifeRate > 0.5f)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, range, layerMask);
                if (colliders.Length > 4)
                {
                    player.Health.DoDamage(Mathf.RoundToInt(player.Health.maxHealth * 3f / 10), DamageType.DeathGod);
                    isAttack = true;
                    animator.Play("Attack");
                    audioSource.Play();
                    StartCoroutine(Attack());
                }
            }
        }
        foreach (var item in GetComponentsInChildren<ParticleSystem>())
        {
            var renderer = item.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
                renderer.sortingOrder = player.LayerOrder + 1;
        }
        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            item.sortingOrder = player.LayerOrder + 1;
        }
        transform.localScale = new Vector3(GameManager.Instance.Player.FacingDirection == FacingDirections.Right ? 1 : -1, 1, 1);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.32f);
        var player = GameManager.Instance.Player;
        var colliders = Physics2D.OverlapCircleAll(transform.position, range, layerMask);
        int count = 0;
        foreach (var item in colliders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                if (health)
                {
                    var damage = Mathf.RoundToInt(player.Health.maxHealth * 3f / 2); // 消耗血量 * 5
                    health.DoDamage(damage == 0 ? 1 : damage, DamageType.DeathGod);
                    var addMax = Mathf.RoundToInt(player.Health.maxHealth * 3f / 10 / 6);
                    player.Health.AddHealth(addMax == 0 ? 1 : addMax);
                    count++;
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
        yield return new WaitForSeconds(0.32f - count * 0.01f);
        lastAttackTimer = Time.time;
        isAttack = false;
    }

    public override void DayEnd()
    {
        base.DayEnd();
    }
}
