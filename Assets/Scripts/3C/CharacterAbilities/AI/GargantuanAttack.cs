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
    [Tooltip("�ý�ʬ����ǰҡ����ٶȱ���")]
    public float RushSpeedMul = 2f;

    [Tooltip("�ý�ʬ����ǰ���ٶȱ���")]
    public float SwoopSpeedMul = 4f;

    [Tooltip("��̹�������")]
    public float AttackRange = 3f;
    [Tooltip("���ǰ�˹�������")]
    public float AttackSwoopRange = 5f;
    [Tooltip("�����Ծ�������룬���ڴ�ֵ�ض��ͷ�")]
    public float AttackFallRange = 7f;

    [Tooltip("�ý�ʬ����ǰҡ������")]
    public string AttackBeforeAnimation = "Attack_Before";   // ���ļ����У���Ҫ�����ļ���
    [Tooltip("�ý�ʬ����������")]
    public string AttackAnimation = "Attack";

    [Tooltip("�ý�ʬǰ�˹���ǰҡ������")]
    public string AttackSwoopBeforeAnimation = "AttackSwoop_Before";
    [Tooltip("�ý�ʬǰ�˹���������")]
    public string AttackSwoopAnimation = "AttackSwoop";
    [Tooltip("�ý�ʬǰ�˹�������������")]
    public string AttackSwoopAfterAnimation = "AttackSwoop_After";

    [Tooltip("��������Ĺ���ǰҡ")]
    public string AttackFallBeforeAnimation = "FallingSkyAttack_Before";
    [Tooltip("��������Ĺ���")]
    public string AttackFallAnimation = "FallingSkyAttack";
    [Tooltip("��������Ĺ���houҡ")]
    public string AttackFallAfterAnimation = "FallingSkyAttack_After";

    [Tooltip("�����Ĵ�����")]
    public BoxCollider2D AttackBoxColider;
    [Tooltip("ǰ�˹����Ĵ�����")]
    public BoxCollider2D AttackSwoopBoxColider;
    [Tooltip("������������Ĵ�����")]
    public CircleCollider2D AttackFallBoxColider;
    [Tooltip("�������������Ҫ���ص�")]
    public List<BoxCollider2D> colliders;

    public GameObject crack;
    private GameObject crackGo;

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
        crackGo = GameObject.Instantiate(crack);
        crackGo.SetActive(false);
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
        AttackBoxColider.enabled = false;
        AttackSwoopBoxColider.enabled = false;
        AttackFallBoxColider.enabled = false;
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
        JudgeTrigger(attackSwoopTrigger, AttackSwoopBoxColider);
        JudgeTrigger(attackFallTrigger, AttackFallBoxColider);

        if (isRushAttack && aiMove.AIParameter.Distance < 0.4f && trackEntry != null)
        {
            isRushAttack = false;
            Attack();
        }

        if (Time.time - timer < AttackJudgmentTime)
            return;

        if (aiMove.AIParameter.Distance > AttackFallRange && !isFallAttack)
        {
            AttackFallingSky();
        }

        timer = Time.time;
        if (trackEntry != null)
            return;
        // �жϴ�ʱ�Ƿ񹥻�
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
        // ��ɫ�ڹ������������Ҵ�������
        if (boxCollider2D.enabled && trigger2D.IsTrigger)
        {
            if (GameManager.Instance.IsEnd)
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
                boxCollider2D.enabled = false;
            }
        }
    }

    private void Attack_Before()
    {
        float distance = aiMove.AIParameter.Distance;
        // ǰҡʱ���ѡ��ʬAudioSource,���û�ڲ����򲥷�
        audioSource = AudioManager.Instance.RandomPlayZombieSounds();
        isRushAttack = true;
        // ����ǰҡ�����
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
        // ����ǰҡ���˹����� �ٶ�Ϊ0
        aiMove.MoveSpeed = 0;
        controller.BoxCollider.enabled = true;
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAnimation, false);
        AttackBoxColider.enabled = true;
        trackEntry.Complete += (e) =>
        {
            isRushAttack = false;
            // �����꽩ֱ0.2s�����ƶ�
            Invoke("SpeedRecovery", 0.2f);
            AttackBoxColider.enabled = false;
            trackEntry = null;
            skeletonAnimation.AnimationState.ClearTrack(1);
            audioSource = null;
        };
    }

    private void AttackSwoop()
    {
        float distance = aiMove.AIParameter.Distance;
        // ǰҡʱ���ѡ��ʬAudioSource,���û�ڲ����򲥷�
        audioSource = AudioManager.Instance.RandomPlayZombieSounds();
        // ����ǰҡ������
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
        float distance = aiMove.AIParameter.Distance;
        // ǰҡʱ���ѡ��ʬAudioSource,���û�ڲ����򲥷�
        audioSource = AudioManager.Instance.RandomPlayZombieSounds();
        // ����ǰҡ������
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
                crackGo.SetActive(true);
                pos.y += 0.6f;
                pos.x -= 0.6f;
                crackGo.transform.position = pos;
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackFallAfterAnimation, false);
                trackEntry.Complete += (e) =>
                {
                    skeletonAnimation.AnimationState.ClearTrack(1);
                    AttackFallBoxColider.enabled = false;
                    SetColliders(true);
                    aiMove.canMove = true;
                    isFallAttack = false;
                    audioSource = null;
                    crackGo.SetActive(false);
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
        SetColliders(true);
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
