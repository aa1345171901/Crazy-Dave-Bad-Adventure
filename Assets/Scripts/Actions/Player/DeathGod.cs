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
    Character player;
    bool isAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Reuse()
    {
        base.Reuse();
        player = GameManager.Instance.Player;
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();

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
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.32f);
        var colliders = Physics2D.OverlapCircleAll(transform.position, range, layerMask);
        int count = 0;
        foreach (var item in colliders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                if (health)
                {
                    var damage = player.Health.maxHealth * 3; // 消耗血量 * 10
                    health.DoDamage(damage, DamageType.DeathGod);
                    player.Health.AddHealth(Mathf.RoundToInt(player.Health.maxHealth * 3f / 10 / 6));
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
