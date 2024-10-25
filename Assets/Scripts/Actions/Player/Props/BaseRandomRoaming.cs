using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

/// <summary>
/// 在角色附近随机找位置漫游
/// </summary>
public class BaseRandomRoaming : BaseProp
{
    public float speed;
    [Tooltip("漫游在角色周围的随机位置")]
    public Vector2 offset;

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
    protected Vector3 targetPlayerPosOffset;
    private float lastAttackTimer;
    protected float realSpeed;

    protected int finalDamage;

    /// <summary>
    /// 正在攻击，不移动
    /// </summary>
    protected bool isAttack;

    protected CharacterProp characterProp;

    private void Start()
    {
        Trigger.OnTriggerEnter += TriggerEnter2D;
        Trigger.OnTriggerExit += TriggerExit2D;
        Init();
    }

    protected virtual void Init()
    {

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
        targetPlayerPosOffset = new Vector3(Random.Range(-offset.x, offset.x), Random.Range(-offset.y, offset.y), 0);
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
            AttackTrigger(targetPos);
        }
        else
        {
            targetPos = GameManager.Instance.Player.transform.position + targetPlayerPosOffset;
            direction = (targetPos - this.transform.position).normalized;
            if ((targetPos - this.transform.position).magnitude < 0.2f)
            {
                targetPlayerPosOffset = new Vector3(Random.Range(-offset.x, offset.x), Random.Range(-offset.y, offset.y), 0);
            }
            if ((targetPos - this.transform.position).magnitude > 2)
            {
                realSpeed *= 2;
            }
        }
        if (isAttack)
            realSpeed = 0;

        transform.Translate(direction * Time.deltaTime * realSpeed);
        SetSortingOrder(direction);
    }

    protected virtual void AttackTrigger(Vector3 targetPos)
    {

    }

    protected virtual void SetSortingOrder(Vector3 direction)
    {

    }

    public virtual IEnumerator Attack()
    {
        lastAttackTimer = Time.time;
        isPursuit = false;
        characterProp.targets.Remove(target);
        target = null;
        isAttack = false;
        yield return null;
    }

    public override void DayEnd()
    {
        base.DayEnd();
    }
}
