using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class FumeShroom : Plant
{
    public override PlantType PlantType => PlantType.FumeShroom;

    [Tooltip("¹¥»÷ÉËº¦")]
    public int Damage = 5;
    [Tooltip("¹¥»÷·¶Î§")]
    public float Range = 3.5f;
    [Tooltip("¹¥»÷ÀäÈ´Ê±¼ä")]
    public float CoolTime = 2;
    [Tooltip("¹¥»÷Ä¿±ê")]
    public LayerMask TargetLayer;
    [Tooltip("¹¥»÷Ö¡¶¯»­")]
    public SpriteRenderer AttackImage;

    public AudioSource audioSource;

    protected float timer;
    protected int finalDamage;
    protected float finalRage;
    protected float finalCoolTime;
    protected int finalCriticalHitRate;
    protected float finalCriticalHitDamage = 1.5f;
    protected int finalDoubleDamageRate;

    private readonly int LevelBasicDamage = 1;
    private readonly float LevelPercentage = 10;
    private readonly float LevelCoolTime = 0.1f;
    private readonly int LevelCriticalHitRate = 5;
    private readonly int LevelDoubleDamageRate = 3;

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(this.audioSource);
    }

    public override void Reuse()
    {
        base.Reuse();

        // ÊôÐÔË³ÐòÐèÒªÓëPlantCultivationPageÉè¼ÆµÄÎÄ×ÖÏà¶ÔÓ¦
        finalDamage = Damage;
        finalRage = Range;
        finalCoolTime = CoolTime;
        int[] attributes = plantAttribute.attribute;
        for (int i = 0; i < attributes.Length; i++)
        {
            // ×Ö¶ÎÓ³Éä
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            switch (attributes[i])
            {
                // 0 Îª»ù´¡ÉËº¦
                case 0:
                    finalDamage = (int)fieldInfo.GetValue(plantAttribute) * LevelBasicDamage + finalDamage;
                    break;
                // 1 Îª°Ù·Ö±ÈÉËº¦
                case 1:
                    finalDamage = (int)(finalDamage * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100);
                    break;
                // ¼ì²â·¶Î§
                case 2:
                    finalRage = Range * ((int)fieldInfo.GetValue(plantAttribute) * LevelPercentage + 100) / 100;
                    break;
                // ÀäÈ´Ê±¼ä
                case 3:
                    finalCoolTime = CoolTime - (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // ±©»÷ÂÊ
                case 4:
                    finalCriticalHitRate = (int)fieldInfo.GetValue(plantAttribute) * LevelCriticalHitRate;
                    break;
                // ±©»÷ÉËº¦
                case 5:
                    finalCriticalHitDamage = 1.5f + (int)fieldInfo.GetValue(plantAttribute) * LevelCoolTime;
                    break;
                // ÉËº¦¶ÎÊý·­±¶¸ÅÂÊ
                case 6:
                    finalDoubleDamageRate = (int)fieldInfo.GetValue(plantAttribute) * LevelDoubleDamageRate;
                    break;
                default:
                    break;
            }
        }
        finalDamage = (int)(finalDamage * (GameManager.Instance.UserData.Botany * 2 + 100) / 100f);

        realRange = FacingDirections == FacingDirections.Right ? finalRage : -finalRage;
        pos = new Vector3(this.transform.position.x + realRange / 2, this.transform.position.y - 0.5f);
        size = new Vector2(finalRage, 1);
        AttackImage.transform.localScale = new Vector3(finalRage / Range, 1, 1);
        AttackImage.sortingOrder = spriteRenderer.sortingOrder + 1;
    }

    private void Update()
    {
        if (Time.time - timer > finalCoolTime)
        {
            var collider = Physics2D.OverlapBox(pos, size, 0, TargetLayer);
            if (collider)
            {
                animator.SetTrigger("Attack");
                timer = Time.time;
                Invoke("Attack", 0.4f);
            }
        }
    }

    protected virtual void Attack()
    {
        var colliders = Physics2D.OverlapBoxAll(pos, size, 0, TargetLayer);
        bool isCriticalHit = Random.Range(0, 100) < finalCriticalHitRate ? true : false;
        bool isDoubleDamage = Random.Range(0, 100) < finalDoubleDamageRate ? true : false;
        int damage = isCriticalHit ?(int)(finalDamage * finalCriticalHitDamage) : finalDamage;
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

    public class DoubleDamage
    {
        public Health Health;
        public int damage;
        public bool isCriticalHit;

        public DoubleDamage(Health health, int damage, bool isCriticalHit)
        {
            this.Health = health;
            this.damage = damage;
            this.isCriticalHit = isCriticalHit;
        }
    }

    protected IEnumerator DoDoubleDamage(DoubleDamage doubleDamage)
    {
        yield return new WaitForSeconds(0.1f);
        doubleDamage.Health.DoDamage(doubleDamage.damage, DamageType.FumeShroom, doubleDamage.isCriticalHit);
    }
}
