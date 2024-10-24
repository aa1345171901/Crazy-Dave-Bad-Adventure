using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class DarkCloud : BaseProp
{
    public float speed;
    public float range;
    public GameObject hit;

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
    /// <summary>
    /// 随机的玩家附近的一个点，到达后更换漫游点
    /// </summary>
    protected Vector3 targetPlayerPos;
    private float lastAttackTimer;
    private float realSpeed;
    private int finalDamage;
    /// <summary>
    /// 正在攻击，不移动
    /// </summary>
    bool isAttack;

    protected SpriteRenderer spriteRenderer;
    protected CharacterProp characterProp;

    private void Start()
    {
        Trigger.OnTriggerEnter += TriggerEnter2D;
        Trigger.OnTriggerExit += TriggerExit2D;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        targetPlayerPos = GameManager.Instance.Player.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        var userData = GameManager.Instance.UserData;
        finalDamage = Mathf.RoundToInt((userData.Adrenaline / 5 + DefaultDamage) * (100f + userData.CriticalDamage) / 100);
    }

    private void Update()
    {
        Vector2 direction = Vector2.zero;
        Vector3 targetPos;
        realSpeed = speed;
        if (!isAttack && isPursuit && Time.time - lastAttackTimer > DefaultAttackCoolingTime && target.gameObject.activeSelf)
        {
            // 追击敌人
            targetPos = target.transform.position;
            direction = (targetPos - this.transform.position).normalized;
            realSpeed *= 2;
            // 发动攻击
            if ((targetPos - this.transform.position).magnitude < 0.2f)
            {
                isAttack = true;
                StartCoroutine(Attack());
            }
        }
        else if (targetPlayerPos != null)
        {
            targetPos = targetPlayerPos;
            direction = (targetPos - this.transform.position).normalized;
            if ((targetPos - this.transform.position).magnitude < 0.2f)
            {
                targetPlayerPos = GameManager.Instance.Player.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-4f, 2f), 0);
            }
            if ((targetPos - this.transform.position).magnitude > 2)
            {
                realSpeed *= 2;
            }
        }
        if (isAttack)
            realSpeed = 0;

        transform.Translate(direction * Time.deltaTime * realSpeed);
        int y = (int)((-this.transform.position.y + 11f) * 10);
        foreach (var item in GetComponentsInChildren<ParticleSystem>())
        {
            var renderer = item.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
                renderer.sortingOrder = y;
        }
        spriteRenderer.sortingOrder = y;
    }

    public virtual IEnumerator Attack()
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
        yield return new WaitForSeconds(0.45f);
        hit.SetActive(false);
        lastAttackTimer = Time.time;
        isPursuit = false;
        characterProp.targets.Remove(target);
        target = null;
        isAttack = false;
    }

    public override void DayEnd()
    {
        base.DayEnd();
    }
}
