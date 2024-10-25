using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class DarkCloud : BaseRandomRoaming
{
    public float range;
    public GameObject hit;

    protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;

    protected override void Init()
    {
        base.Init();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponentInChildren<AudioSource>();
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    private void OnDestroy()
    {
        AudioManager.Instance.AudioLists.Remove(audioSource);
    }

    public override void Reuse()
    {
        base.Reuse();
        var userData = GameManager.Instance.UserData;
        finalDamage = Mathf.RoundToInt((userData.Adrenaline / 5 + DefaultDamage) * (100f + userData.CriticalDamage) / 100);
    }

    protected override void SetSortingOrder(Vector3 direction)
    {
        base.SetSortingOrder(direction);
        int y = (int)((-this.transform.position.y + 11f) * 10);
        foreach (var item in GetComponentsInChildren<ParticleSystem>())
        {
            var renderer = item.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
                renderer.sortingOrder = y;
        }
        spriteRenderer.sortingOrder = y;
    }

    protected override void AttackTrigger(Vector3 targetPos)
    {
        base.AttackTrigger(targetPos);
        // 发动攻击
        if ((targetPos - this.transform.position).magnitude < 0.2f)
        {
            isAttack = true;
            StartCoroutine(Attack());
        }
    }

    public override IEnumerator Attack()
    {
        hit.SetActive(true);
        var colliders = Physics2D.OverlapCircleAll(this.transform.position, range, Trigger.layerMasks);
        foreach (var item in colliders)
        {
            if (item.isTrigger)
            {
                var health = item.GetComponent<Health>();
                if (health)
                {
                    health.DoDamage(finalDamage, DamageType.DarkCloud);
                }
            }
        }
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        yield return base.Attack();
        hit.SetActive(false);
    }
}
