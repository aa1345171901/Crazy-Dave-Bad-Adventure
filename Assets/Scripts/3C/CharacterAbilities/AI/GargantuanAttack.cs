using Spine;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("TopDownPlate/AI/Ability/Attack/Gargantuan Attack")]
public class GargantuanAttack : AIAttack
{
    [Space(10)]
    [Tooltip("该僵尸攻击前摇冲刺速度倍率")]
    public float RushSpeedMul = 2f;

    [Tooltip("该僵尸攻击前扑速度倍率")]
    public float SwoopSpeedMul = 4f;

    [Tooltip("冲刺攻击距离")]
    public float AttackRange = 3f;
    [Tooltip("冲刺前扑攻击距离")]
    public float AttackSwoopRange = 5f;
    [Tooltip("冲刺跳跃攻击距离，大于此值必定释放")]
    public float AttackFallRange = 7f;

    [Tooltip("该僵尸攻击前摇动画名")]
    public string AttackBeforeAnimation = "Attack_Before";   // 在文件夹中，需要加上文件夹
    [Tooltip("该僵尸攻击动画名")]
    public string AttackAnimation = "Attack";

    [Tooltip("该僵尸前扑攻击前摇动画名")]
    public string AttackSwoopBeforeAnimation = "AttackSwoop_Before";
    [Tooltip("该僵尸前扑攻击动画名")]
    public string AttackSwoopAnimation = "AttackSwoop";
    [Tooltip("该僵尸前扑攻击结束动画名")]
    public string AttackSwoopAfterAnimation = "AttackSwoop_After";

    [Tooltip("从天而降的攻击前摇")]
    public string AttackFallBeforeAnimation = "FallingSkyAttack_Before";
    [Tooltip("从天而降的攻击")]
    public string AttackFallAnimation = "FallingSkyAttack";
    [Tooltip("从天而降的攻击hou摇")]
    public string AttackFallAfterAnimation = "FallingSkyAttack_After";

    [Tooltip("攻击的触发器")]
    public BoxCollider2D AttackBoxColider;
    [Tooltip("前扑攻击的触发器")]
    public BoxCollider2D AttackSwoopBoxColider;
    [Tooltip("从天而降攻击的触发器")]
    public CircleCollider2D AttackFallBoxColider;
    [Tooltip("从天而降攻击需要隐藏的")]
    public List<BoxCollider2D> colliders;

    public GameObject crack;

    public UnityEvent FallAttack;

    private TrackEntry trackEntry;
    private float timer;
    private Trigger2D attackTrigger;
    private Trigger2D attackSwoopTrigger;
    private Trigger2D attackFallTrigger;

    private bool isRushAttack;
    private bool isFallAttack;

    protected override void Initialization()
    {
        base.Initialization();
        attackTrigger = AttackBoxColider.GetComponent<Trigger2D>();
        attackSwoopTrigger = AttackSwoopBoxColider.GetComponent<Trigger2D>();
        attackFallTrigger = AttackFallBoxColider.GetComponent<Trigger2D>();
        crack.SetActive(false);
    }

    public override void Reuse()
    {
        base.Reuse();
        isRushAttack = false;
        trackEntry = null;
        int waveIndex = LevelManager.Instance.IndexWave + 1;
        if (waveIndex < 4)
        {
            this.realDamage = Damage;
        }
        else if (waveIndex < 9)
        {
            this.realDamage = (int)((Damage + 2f) * (waveIndex / 4f));
        }
        else if (waveIndex < 13)
        {
            this.realDamage = (int)((Damage + 3f) * (waveIndex / 3f));
        }
        else if (waveIndex < 17)
        {
            this.realDamage = (int)((Damage + 4f) * (waveIndex / 1.5f));
        }
        else if (waveIndex < 21)
        {
            this.realDamage = (int)((Damage + 5f) * waveIndex);
        }
        else
        {
            this.realDamage = (int)((Damage + 6f) * waveIndex * 1.5f);
        }
        AttackBoxColider.enabled = false;
        AttackSwoopBoxColider.enabled = false;
        AttackFallBoxColider.enabled = false;
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
        JudgeTrigger(attackTrigger, AttackBoxColider);
        JudgeTrigger(attackSwoopTrigger, AttackSwoopBoxColider);
        JudgeTrigger(attackFallTrigger, AttackFallBoxColider);

        if (isRushAttack && aiMove.AIParameter.Distance < 0.4f && trackEntry != null)
        {
            isRushAttack = false;
            Attack();
        }

        if (Time.time - timer < AttackJudgmentTime)
            return;

        timer = Time.time;
        if (trackEntry != null)
            return;
        if (aiMove.AIParameter.Distance > AttackFallRange && !isFallAttack)
        {
            AttackFallingSky();
        }

        // 判断此时是否攻击
        float random = Random.Range(0, 1f);
        if (random > realAttackProbability)
            return;

        if (aiMove.AIParameter.Distance < AttackRange)
        {
            if (Random.Range(0, 2) == 0)
                Attack_Before();
            else
                AttackSwoop();
        }
        else if (aiMove.AIParameter.Distance < AttackSwoopRange)
        {
            AttackSwoop();
        }
    }

    private void JudgeTrigger(Trigger2D trigger2D, Collider2D boxCollider2D)
    {
        // 角色在攻击触发器中且触发器打开
        if (boxCollider2D.enabled && trigger2D.IsTrigger)
        {
            if (GameManager.Instance.IsEnd)
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
                GameManager.Instance.DoDamage(realDamage, ZombieType.Gargantuan);
                boxCollider2D.enabled = false;
            }
        }
    }

    private void Attack_Before()
    {
        healths.Clear();
        // 前摇时随机选择僵尸AudioSource,如果没在播放则播放
        audioSource = AudioManager.Instance.RandomPlayZombieSounds();
        isRushAttack = true;
        // 攻击前摇，冲刺
        aiMove.MoveSpeed *= RushSpeedMul;
        controller.BoxCollider.enabled = false;
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackBeforeAnimation, false);
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
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAnimation, false);
        AttackBoxColider.enabled = true;
        trackEntry.Complete += (e) =>
        {
            isRushAttack = false;
            // 攻击完僵直0.2s设置移动
            Invoke("SpeedRecovery", 0.2f);
            AttackBoxColider.enabled = false;
            trackEntry = null;
            skeletonAnimation.AnimationState.ClearTrack(1);
            audioSource = null;
        };
    }

    private void AttackSwoop()
    {
        healths.Clear();
        // 前摇时随机选择僵尸AudioSource,如果没在播放则播放
        audioSource = AudioManager.Instance.RandomPlayZombieSounds();
        // 攻击前摇，蓄力
        aiMove.MoveSpeed = 0;
        aiMove.isSwoop = true;
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackSwoopBeforeAnimation, false);
        Invoke("SetSpeed", trackEntry.AnimationTime / 2);
        trackEntry.Complete += (e) =>
        {
            controller.BoxCollider.enabled = false;
            trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackSwoopAnimation, false);
            AttackSwoopBoxColider.enabled = true;
            trackEntry.Complete += (e) =>
            {
                FallAttack?.Invoke();
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackSwoopAfterAnimation, false);
                trackEntry.Complete += (e) =>
                {
                    trackEntry = null;
                    skeletonAnimation.AnimationState.ClearTrack(1);
                    aiMove.SpeedRecovery();
                };
                aiMove.MoveSpeed = 0;
                AttackSwoopBoxColider.enabled = false;
                trackEntry = null;
                aiMove.isSwoop = false;
                audioSource = null;
            };
        };
    }

    private void AttackFallingSky()
    {
        healths.Clear();
        // 前摇时随机选择僵尸AudioSource,如果没在播放则播放
        audioSource = AudioManager.Instance.RandomPlayZombieSounds();
        // 攻击前摇，蓄力
        aiMove.canMove = false;
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackFallBeforeAnimation, false);
        trackEntry.Complete += (e) =>
        {
            isFallAttack = true;
            SetColliders(false);
            Vector3 pos = GameManager.Instance.Player.transform.position;
            pos.y -=1.25f;
            pos.x += 0.5f;
            this.transform.position = pos;
            trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackFallAnimation, false);
            trackEntry.Complete += (e) =>
            {
                FallAttack?.Invoke();
                AttackFallBoxColider.enabled = true;
                crack.SetActive(true);
                pos.y += 1.2f;
                pos.x += 1f;
                if (character.FacingDirection == FacingDirections.Right)
                {
                    pos.x -= 2f;
                }
                crack.transform.position = pos;
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackFallAfterAnimation, false);
                trackEntry.Complete += (e) =>
                {
                    skeletonAnimation.AnimationState.ClearTrack(1);
                    AttackFallBoxColider.enabled = false;
                    SetColliders(true);
                    aiMove.canMove = true;
                    SpeedRecovery();
                    isFallAttack = false;
                    audioSource = null;
                    crack.SetActive(false);
                    aiMove.AIParameter.Distance = (Target.position - this.transform.position).magnitude;
                    trackEntry = null;
                };
            };
        };
    }

    protected override void Dead()
    {
        base.Dead();
        AttackBoxColider.enabled = false;
        AttackSwoopBoxColider.enabled = false;
        AttackFallBoxColider.enabled = false;
    }

    private void SetColliders(bool enabled)
    {
        foreach (var item in colliders)
        {
            item.enabled = enabled;
        }
    }

    private void SetSpeed()
    {
        aiMove.SpeedRecovery();
        aiMove.MoveSpeed *= SwoopSpeedMul;
    }

    private void SpeedRecovery()
    {
        aiMove.SpeedRecovery();
    }
}
