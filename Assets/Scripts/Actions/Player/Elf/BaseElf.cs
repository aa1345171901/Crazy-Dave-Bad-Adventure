using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class BaseElf : BaseProp
{
    public float speed;
    public float range;
    /// <summary>
    /// 攻击范围检测
    /// </summary>
    public Trigger2D Trigger;

    /// <summary>
    /// 是否追击，没有追击就跟随玩家
    /// </summary>
    protected bool isPursuit { get; set; }
    /// <summary>
    /// 目标
    /// </summary>
    protected GameObject target;
    protected Transform targetPlayerPos;
    private float lastAttackTimer;
    private float realSpeed;
    private float realRange;

    SpriteRenderer spriteRenderer;
    CharacterProp characterProp;

    private void Start()
    {
        Trigger.OnTriggerEnter += TriggerEnter2D;
        Trigger.OnTriggerExit += TriggerExit2D;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        Reuse();
    }

    private void TriggerEnter2D(Collider2D collider2D)
    {
        if (target == null && !characterProp.targets.Contains(collider2D.gameObject))
        {
            isPursuit = true;
            target = collider2D.gameObject;
            characterProp.targets.Add(target);
        }
    }

    private void TriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject == target)
        {
            isPursuit = false;
            characterProp.targets.Remove(target);
            target = null;
        }
    }

    public override void Reuse()
    {
        base.Reuse();
        characterProp = GameManager.Instance.Player.FindAbility<CharacterProp>();
        targetPlayerPos = characterProp.GetRandomPos();
        if (targetPlayerPos != null)
            this.transform.position = targetPlayerPos.position;
        realRange = range;
    }

    private void Update()
    {
        Vector2 direction = Vector2.zero;
        Vector3 targetPos;
        realSpeed = speed;
        if (isPursuit && Time.time - lastAttackTimer > DefaultAttackCoolingTime && target.gameObject.activeSelf)
        {
            // 追击敌人
            targetPos = target.transform.position;
            direction = (targetPos - this.transform.position).normalized;
            var colliders = Physics2D.OverlapCircleAll(transform.position, realRange, Trigger.layerMasks);
            // 发动攻击
            if (colliders.Length > 0)
            {
                lastAttackTimer = Time.time;
                var health = colliders[0].GetComponent<Health>();
                health.DoDamage(5, DamageType.FireElf, true);

                isPursuit = false;
                characterProp.targets.Remove(target);
                target = null;
            }
        }
        else if (targetPlayerPos != null)
        {
            targetPos = targetPlayerPos.position;
            direction = (targetPos - this.transform.position).normalized;
            if ((targetPos - this.transform.position).magnitude < 0.25f)
            {
                realSpeed = 0;
            }
        }
        transform.Translate(direction * Time.deltaTime * realSpeed);
        int y = (int)((-this.transform.position.y + 1f + 10) * 10);
        spriteRenderer.sortingOrder = y;
    }

    public override void DayEnd()
    {
        base.DayEnd();
    }
}
