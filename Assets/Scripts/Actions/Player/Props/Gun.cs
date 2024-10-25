using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class Gun : BaseRandomRoaming
{
    public Transform bulletPos;

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected int level;
    float angle;
    float targetAngle;
    float timer;
    bool isRotation;
    string clipAttackName = "pistolAttack";
    string clipIdelName = "pistolIdel";

    protected override void Init()
    {
        base.Init();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void Reuse()
    {
        base.Reuse();

        level = characterProp.GetGunLevel();

        var userData = GameManager.Instance.UserData;
        var confItem = ConfManager.Instance.confMgr.propCards.GetItemByTypeLevel(11, level);
        finalDamage = DefaultDamage;
        if (confItem != null)
        {
            finalDamage = Mathf.RoundToInt((userData.Power + confItem.value1) * (100f + userData.PercentageDamage) / 100);
            clipAttackName = confItem.propName + "Attack";
            clipIdelName = confItem.propName + "Idel";
            DefaultAttackCoolingTime = confItem.coolingTime;
        }
        animator.Play(clipIdelName);
    }

    protected override void SetSortingOrder(Vector3 direction)
    {
        base.SetSortingOrder(direction);
        if (!isAttack)
        {
            if (isPursuit)
                transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
            else
                transform.localScale = new Vector3(GameManager.Instance.Player.FacingDirection == FacingDirections.Right ? 1 : -1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        int y = (int)((-this.transform.position.y + 11f) * 10);
        spriteRenderer.sortingOrder = y;

        if (target != null)
        {
            var targetPos = target.transform.position;
            direction = (targetPos - this.transform.position).normalized;
            targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (Time.time - timer < 0.2f)
            {
                float process = (Time.time - timer) / 0.2f;
                transform.rotation = Quaternion.AngleAxis(Mathf.LerpAngle(angle, targetAngle, process), Vector3.forward);
            }
        }
    }

    protected override void AttackTrigger(Vector3 targetPos)
    {
        base.AttackTrigger(targetPos);
        if (!isRotation)
        {
            timer = Time.time;
            isRotation = true;
        }
        else
        {
            isAttack = true;
            StartCoroutine(Attack());
        }
    }

    public override IEnumerator Attack()
    {
        animator.Play(clipAttackName, 0, 0);
        yield return new WaitForSeconds(animator.GetLengthByName(clipAttackName));
        animator.Play(clipIdelName);
        yield return base.Attack();
        transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
        isRotation = false;
    }

    /// <summary>
    /// 动画机创建子弹
    /// </summary>
    public void CreateBullet()
    {
        var prefab = Resources.Load<GunBullet>("Prefabs/Props/GunBullet");
        var bullet = GameObject.Instantiate(prefab);
        bullet.damage = finalDamage;
        bullet.rotation = this.transform.rotation;

        bullet.transform.position = bulletPos.position;
    }
}
