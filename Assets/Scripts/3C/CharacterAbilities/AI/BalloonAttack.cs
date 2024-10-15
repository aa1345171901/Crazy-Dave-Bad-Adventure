using Spine;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[AddComponentMenu("TopDownPlate/AI/Ability/Attack/Balloon Attack")]
public class BalloonAttack : AIAttack
{
    [Space(10)]
    [Tooltip("该僵尸攻击前摇冲刺速度倍率")]
    public float RushSpeedMul = 2f;

    [Tooltip("会发动攻击的距离")]
    public float AttackRange = 5f;

    [Tooltip("该僵尸攻击前摇动画名")]
    public string AttackBeforeAnimation = "Attack_Before";
    [Tooltip("该僵尸攻击动画名")]
    public string AttackAnimation = "Attack";
    [Tooltip("该僵尸前扑攻击结束动画名")]
    public string AttackAfterAnimation = "Attack_After";

    [Tooltip("攻击的触发器")]
    public BoxCollider2D AttackBoxColider;
    [Tooltip("攻击的拖尾")]
    public List<GameObject> Trails;

    private TrackEntry trackEntry;
    private float timer;
    private Trigger2D attackTrigger;
    private ZombieAnimation zombieAnimation;

    [ReadOnly]
    public bool realCanSwoop;

    [ReadOnly]
    public float realAttackRange;

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
        int waveIndex = LevelManager.Instance.IndexWave + 1;
        this.realAttackRange = AttackRange + waveIndex / 10f;
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
        if (character.State.AIStateType == AIStateType.Init || character.IsDead)
        {
            audioSource?.Stop();
            return;
        }
        JudgeTrigger(attackTrigger, AttackBoxColider);

        if (Time.time - timer < AttackJudgmentTime)
            return;
        timer = Time.time;
        if (trackEntry != null)
            return;
        // 判断此时是否攻击
        float random = Random.Range(0, 1f);
        if (random > realAttackProbability)
            return;

        Attack();
    }

    private void JudgeTrigger(Trigger2D trigger2D, BoxCollider2D boxCollider2D)
    {
        // 角色在攻击触发器中且触发器打开
        if (trigger2D.IsTrigger && boxCollider2D.enabled)
        {
            if (GameManager.Instance.IsEnd || aiMove.IsEnchanted)
            {
                var target = trigger2D.Target.GetComponent<Character>();
                if (target != null && !healths.Contains(target.Health))
                {
                    target.Health.DoDamage(realDamage, DamageType.ZombieHurEachOther);
                    healths.Add(target.Health);
                }
            }
            else if (trigger2D.Target == GameManager.Instance.Player.gameObject)
            {
                GameManager.Instance.DoDamage(realDamage);
            }
        }
    }

    private void Attack()
    {
        healths.Clear();
        float distance = aiMove.AIParameter.Distance;
        if (distance < realAttackRange)
        {
            // 前摇时随机选择僵尸AudioSource,如果没在播放则播放
            audioSource = AudioManager.Instance.RandomPlayZombieSounds();

            aiMove.isSwoop = true;
            SetTrailAndColliderActive(true, false, AttackBoxColider);
            trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackBeforeAnimation, false);
            trackEntry.Complete += (e) =>
            {
                aiMove.MoveSpeed *= RushSpeedMul;
                controller.BoxCollider.enabled = false;
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAnimation, false);
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
                            character.Health.DoDamage(character.Health.maxHealth, DamageType.Zombie);
                        }
                    }

                    trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAfterAnimation, false);
                    trackEntry.Complete += (e) =>
                    {
                        skeletonAnimation.AnimationState.ClearTrack(1);
                        trackEntry = null;
                        aiMove.SpeedRecovery();
                    };

                    aiMove.MoveSpeed = 0;
                    SetTrailAndColliderActive(false, false, AttackBoxColider);
                    audioSource = null;
                    aiMove.isSwoop = false;
                };
            };
        }
    }

    protected override void Dead()
    {
        base.Dead();
        SetTrailAndColliderActive(false, false, AttackBoxColider);
    }

    private void SetTrailAndColliderActive(bool trailActive, bool coliderActive, BoxCollider2D collider)
    {
        foreach (var item in Trails)
        {
            item.SetActive(trailActive);
        }
        collider.enabled = coliderActive;
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
