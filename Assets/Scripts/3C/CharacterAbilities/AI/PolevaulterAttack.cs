using Spine;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[AddComponentMenu("TopDownPlate/AI/Ability/Attack/Polevaulter Attack")]
public class PolevaulterAttack : AIAttack
{
    [Space(10)]
    [Tooltip("会发动近战攻击的距离，大于时使用远程")]
    public float AttackRange = 3f;

    [Tooltip("该僵尸远程攻击前摇动画名")]
    public string AttackBeforeAnimation = "Attack_Before";
    [Tooltip("该僵尸远程攻击动画名")]
    public string AttackAnimation = "Attack";
    [Tooltip("该僵尸远程后摇动画名")]
    public string AttackAfterAnimation = "Attack_After";

    [Tooltip("该僵尸近战攻击前摇动画名")]
    public string AttackMeleeBeforeAnimation = "MeleeAttack_Before";
    [Tooltip("该僵尸近战攻击动画名")]
    public string AttackMeleeAnimation = "MeleeAttack";

    [Tooltip("标枪预制体")]
    public ZombiePole Pole;
    [Tooltip("标枪发射位置")]
    public Transform PolePos;
    [Tooltip("远程攻击的射线")]
    public LineRenderer lineRenderer;

    [Tooltip("攻击的触发器")]
    public BoxCollider2D AttackBoxColider;
    [Tooltip("攻击的拖尾")]
    public List<GameObject> Trails;

    [ReadOnly]
    public TrackEntry trackEntry;
    [ReadOnly]
    public Vector3 direction;
    [ReadOnly]
    public bool isAttacking;

    private Vector3 attackDirection;

    private float timer;
    private Trigger2D attackTrigger;

    private ZombieAnimation zombieAnimation;

    private bool isRushAttack;

    protected override void Initialization()
    {
        base.Initialization();
        attackTrigger = AttackBoxColider.GetComponent<Trigger2D>();
        zombieAnimation = GetComponentInChildren<ZombieAnimation>();
    }

    public override void Reuse()
    {
        base.Reuse();
        trackEntry = null;
        isRushAttack = false;
        audioSource = null;
        int waveIndex = LevelManager.Instance.IndexWave + 1;
        if (waveIndex < 4)
        {
            this.realDamage = Damage;
        }
        else if (waveIndex < 9)
        {
            this.realDamage = (int)((Damage + 1.5f) * (waveIndex / 4f));
        }
        else if (waveIndex < 13)
        {
            this.realDamage = (int)((Damage + 2.5f) * (waveIndex / 3f));
        }
        else if (waveIndex < 17)
        {
            this.realDamage = (int)((Damage + 3.5f) * (waveIndex / 1.5f));
        }
        else if (waveIndex < 21)
        {
            this.realDamage = (int)((Damage + 4.5f) * waveIndex);
        }
        else
        {
            this.realDamage = (int)((Damage + 5.5f) * waveIndex * 1.5f);
        }
        SetTrailAndColliderActive(false, false, AttackBoxColider);
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
        if (aiMove.decelerationPercentage == 0)
            return;
        if (character.State.AIStateType == AIStateType.Init || character.IsDead)
        {
            audioSource?.Stop();
            return;
        }

        if (isAttacking && !aiMove.IsEnchanted)
        {
            if (character.FacingDirection == FacingDirections.Left)
                direction = new Vector3(-direction.x, direction.y);
            if (attackDirection != Vector3.zero)
                lineRenderer.SetPositions(new Vector3[2] { Vector3.zero, attackDirection * 10 });
            else
                lineRenderer.SetPositions(new Vector3[2] { Vector3.zero, direction * 10 });
        }

        if (isRushAttack && aiMove.AIParameter.Distance < 0.4f && trackEntry != null)
        {
            Attack();
        }
        JudgeTrigger(attackTrigger, AttackBoxColider);

        if (Time.time - timer < AttackJudgmentTime)
            return;
        timer = Time.time;
        if (trackEntry != null)
            return;
        // 判断此时是否攻击
        float random = Random.Range(0, 1f);
        float nowAttackProbability = aiMove.AIParameter.Distance < AttackRange ? realAttackProbability : realAttackProbability * 2;
        if (random > nowAttackProbability)
            return;
        if (aiMove.AIParameter.Distance < AttackRange)
        {
            MelleAttack();
        }
        else if (!GameManager.Instance.IsFog)
        {
            Attack_Before();
        }
    }

    private void JudgeTrigger(Trigger2D trigger2D, BoxCollider2D boxCollider2D)
    {
        // 角色在攻击触发器中且触发器打开
        if (trigger2D.IsTrigger && boxCollider2D.enabled)
        {
            if (GameManager.Instance.IsEnd || aiMove.IsEnchanted)
            {
                var target = trigger2D.GetFirst(false)?.GetComponent<Character>();
                if (target != null && !healths.Contains(target.Health))
                {
                    target.Health.DoDamage(realDamage, DamageType.ZombieHurEachOther);
                    healths.Add(target.Health);
                }
            }
            else if (trigger2D.GetFirst(true) != null)
            {
                GameManager.Instance.DoDamage(realDamage, zombieAnimation.zombieType);
            }
        }
    }

    private void MelleAttack()
    {
        healths.Clear();
        // 前摇时随机选择僵尸AudioSource,如果没在播放则播放
        audioSource = AudioManager.Instance.RandomPlayZombieSounds();
        isRushAttack = true;
        // 攻击前摇，冲刺
        aiMove.MoveSpeed *= 2;
        controller.BoxCollider.enabled = false;
        SetTrailAndColliderActive(true, false, AttackBoxColider);
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackMeleeBeforeAnimation, false);
        trackEntry.Complete += (e) =>
        {
            Attack();
        };
    }

    private void Attack()
    {
        // 攻击前摇完了攻击， 速度为0
        aiMove.MoveSpeed = 0;
        controller.BoxCollider.enabled = true;
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackMeleeAnimation, false);
        SetTrailAndColliderActive(true, true, AttackBoxColider);
        trackEntry.Complete += (e) =>
        {
            // 魅惑攻击次数判断
            if (aiMove.IsEnchanted)
            {
                if (attackCount > 0)
                    attackCount--;
                else
                {
                    LevelManager.Instance.EnchantedEnemys.Remove(zombieAnimation.zombieType, this.character);
                    character.Health.DoDamage(character.Health.maxHealth, DamageType.ZombieHurEachOther);
                }
            }

            isRushAttack = false;
            // 攻击完僵直0.2s设置移动
            Invoke("SpeedRecovery", 0.2f);
            SetTrailAndColliderActive(false, false, AttackBoxColider);
            trackEntry = null;
            skeletonAnimation.AnimationState.ClearTrack(1);
            audioSource = null;
            isAttacking = false;
        };
    }

    private void Attack_Before()
    {
        isAttacking = true;
        audioSource = AudioManager.Instance.RandomPlayZombieSounds();

        // 攻击前摇，蓄力时画射线
        aiMove.MoveSpeed = 0;
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackBeforeAnimation, false);
        trackEntry.Complete += (e) =>
        {
            trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAnimation, false);
            trackEntry.Complete += (e) =>
            {
                attackDirection = direction;
                if (character.FacingDirection == FacingDirections.Left)
                    direction = new Vector3(-direction.x, direction.y);
                var pole = GameObject.Instantiate(Pole);
                pole.transform.position = PolePos.position;
                pole.character = this.character;
                pole.Damage = realDamage;
                pole.IsEnchanted = aiMove.IsEnchanted;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                pole.transform.rotation = Quaternion.Euler(0, 0, angle);

                // 魅惑攻击次数判断
                if (aiMove.IsEnchanted)
                {
                    if (attackCount > 0)
                        attackCount--;
                    else
                    {
                        LevelManager.Instance.EnchantedEnemys.Remove(zombieAnimation.zombieType, this.character);
                        character.Health.DoDamage(character.Health.maxHealth, DamageType.ZombieHurEachOther);
                    }
                }

                trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAfterAnimation, false);
                trackEntry.Complete += (e) =>
                {
                    audioSource = null;
                    trackEntry = null;
                    aiMove.SpeedRecovery();
                    isAttacking = false;
                    attackDirection = Vector3.zero;
                    lineRenderer.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
                    skeletonAnimation.AnimationState.ClearTrack(1);
                    if (aiMove.IsEnchanted)
                    {
                        aiMove.hurtFlash.BeEnchanted();
                    }
                };
            };
        };
    }

    protected override void Dead()
    {
        base.Dead();
        SetTrailAndColliderActive(false, false, AttackBoxColider);
        isAttacking = false;
        attackDirection = Vector3.zero;
        lineRenderer.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
    }

    private void SetTrailAndColliderActive(bool trailActive, bool coliderActive, BoxCollider2D collider)
    {
        foreach (var item in Trails)
        {
            item.SetActive(trailActive);
        }
        collider.enabled = coliderActive;
    }

    private void SpeedRecovery()
    {
        aiMove.SpeedRecovery();
    }

    public override void BeEnchanted(int attackCount, float percentageDamageAdd, int basicDamageAdd)
    {
        base.BeEnchanted(attackCount, percentageDamageAdd, basicDamageAdd);
        int waveIndex = LevelManager.Instance.IndexWave + 1;
        if (waveIndex < 4)
        {
            this.realDamage = Damage + basicDamageAdd;
        }
        else if (waveIndex < 9)
        {
            this.realDamage = (int)((Damage + 1.5f + basicDamageAdd) * (waveIndex / 4f));
        }
        else if (waveIndex < 13)
        {
            this.realDamage = (int)((Damage + 2.5f + basicDamageAdd) * (waveIndex / 3f));
        }
        else if (waveIndex < 17)
        {
            this.realDamage = (int)((Damage + 3.5f + basicDamageAdd) * (waveIndex / 1.5f));
        }
        else if (waveIndex < 21)
        {
            this.realDamage = (int)((Damage + 4.5f + basicDamageAdd) * waveIndex);
        }
        else
        {
            this.realDamage = (int)((Damage + 5.5f + basicDamageAdd) * waveIndex * 1.5f);
        }
        this.realDamage = (int)(realDamage * percentageDamageAdd);
    }
}
