using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class GloomShoom : FumeShroom
{
    public override PlantType PlantType => PlantType.GloomShroom;

    public override void Reuse()
    {
        base.Reuse();
        AttackImage.transform.localScale = new Vector3(finalRage / Range, finalRage / Range, finalRage / Range);
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            var collider = Physics2D.OverlapCircle(pos, finalRage, TargetLayer);
            if (collider)
            {
                animator.SetTrigger("Attack");
                timer = Time.time;
                Invoke("Attack", 0.3f);
                Invoke("Attack", 0.6f);
                Invoke("Attack", 0.9f);
            }
        }
    }

    protected override void Attack()
    {
        var colliders = Physics2D.OverlapCircleAll(pos, finalRage, TargetLayer);
        bool isCriticalHit = Random.Range(0, 100) < finalCriticalHitRate ? true : false;
        bool isDoubleDamage = Random.Range(0, 100) < finalDoubleDamageRate ? true : false;
        int damage = isCriticalHit ? (int)(finalDamage * finalCriticalHitDamage) : finalDamage;
        foreach (var item in colliders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                health.DoDamage(damage, DamageType.FumeShroom, isCriticalHit);
                if (isDoubleDamage)
                    StartCoroutine("DoDoubleDamage", new DoubleDamage(health, damage, isCriticalHit));
            }
        }
        audioSource.Play();
    }
}
