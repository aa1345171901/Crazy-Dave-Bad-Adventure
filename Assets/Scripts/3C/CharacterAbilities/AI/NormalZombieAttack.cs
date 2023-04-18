using Spine;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[AddComponentMenu("TopDownPlate/AI/Ability/Attack/Normal Attack")]
public class NormalZombieAttack : AIAttack
{
    [Space(10)]
    [Tooltip("�ý�ʬ����ǰҡ����ٶȱ���")]
    public float RushSpeedMul = 2f;

    [Tooltip("�ý�ʬ����ǰ���ٶȱ���")]
    public float SwoopSpeedMul = 4f;

    [Tooltip("�ᷢ�������ľ���")]
    public float AttackRange = 5f;

    [Tooltip("�ý�ʬ����ǰҡ������")]
    public string AttackBeforeAnimation = "Attack_Before";
    [Tooltip("�ý�ʬ����������")]
    public string AttackAnimation = "Attack";

    [Tooltip("�ý�ʬǰ�˹���ǰҡ������")]
    public string AttackSwoopBeforeAnimation = "AttackSwoop_Before";
    [Tooltip("�ý�ʬǰ�˹���������")]
    public string AttackSwoopAnimation = "AttackSwoop";
    [Tooltip("�ý�ʬǰ�˹�������������")]
    public string AttackSwoopAfterAnimation = "AttackSwoop_After";

    [Tooltip("�����Ĵ�����")]
    public BoxCollider2D AttackBoxColider;
    [Tooltip("ǰ�˹����Ĵ�����")]
    public BoxCollider2D AttackSwoopBoxColider;
    [Tooltip("��������β")]
    public List<GameObject> Trails;

    private TrackEntry trackEntry;
    private float timer;
    private Trigger2D attackTrigger;
    private Trigger2D attackSwoopTrigger;

    [ReadOnly]
    public float realAttackRange;

    protected override void Initialization()
    {
        base.Initialization();
        attackTrigger = AttackBoxColider.GetComponent<Trigger2D>();
        attackSwoopTrigger = AttackSwoopBoxColider.GetComponent<Trigger2D>();
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
        JudgeTrigger(attackSwoopTrigger, AttackSwoopBoxColider);

        if (Time.time - timer < AttackJudgmentTime)
            return;
        timer = Time.time;
        if (trackEntry != null)
            return;
        // �жϴ�ʱ�Ƿ񹥻�
        float random = Random.Range(0, 1f);
        if (random > realAttackProbability)
            return;

        // ���ѡ�񹥻���ʽ
        if (Random.Range(0, 2) == 0)
            Attack();
        else
            AttackSwoop();
    }

    private void JudgeTrigger(Trigger2D trigger2D, BoxCollider2D boxCollider2D)
    {
        // ��ɫ�ڹ������������Ҵ�������
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
        float distance = aiMove.direction.magnitude;
        if (distance < realAttackRange)
        {
            // ǰҡʱ���ѡ��ʬAudioSource,���û�ڲ����򲥷�
            audioSource = AudioManager.Instance.RandomPlayZombieSounds();

            // ����ǰҡ�����
            aiMove.MoveSpeed *= RushSpeedMul;
            controller.BoxCollider.enabled = false;
            SetTrailAndColliderActive(true, false, AttackBoxColider);
            trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackBeforeAnimation, false);
            trackEntry.Complete += (e) =>
            {
                // ����ǰҡ���˹����� �ٶ�Ϊ0
                aiMove.MoveSpeed = 0;
                controller.BoxCollider.enabled = true;
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAnimation, false);
                SetTrailAndColliderActive(true, true, AttackBoxColider);
                trackEntry.Complete += (e) =>
                {
                    // �Ȼ󹥻������ж�
                    if (aiMove.IsEnchanted)
                    {
                        if (attackCount > 0)
                            attackCount--;
                        else
                        {
                            LevelManager.Instance.EnchantedEnemys.Remove(this.character);
                            character.Health.DoDamage(character.Health.maxHealth, DamageType.Zombie);
                        }
                    }

                    // �����꽩ֱ0.2s�����ƶ�
                    Invoke("SpeedRecovery", 0.2f);
                    SetTrailAndColliderActive(false, false, AttackBoxColider);
                    trackEntry = null;
                    skeletonAnimation.AnimationState.ClearTrack(1);
                    audioSource = null;
                };
            };
            trackEntry.Dispose += (e) => { SetTrailAndColliderActive(false, false, AttackBoxColider); };
        }
    }

    private void AttackSwoop()
    {
        healths.Clear();
        float distance = aiMove.direction.magnitude;
        if (distance < realAttackRange)
        {
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
                SetTrailAndColliderActive(true, true, AttackSwoopBoxColider);
                trackEntry.Complete += (e) =>
                {
                    // �Ȼ󹥻������ж�
                    if (aiMove.IsEnchanted)
                    {
                        if (attackCount > 0)
                            attackCount--;
                        else
                        {
                            LevelManager.Instance.EnchantedEnemys.Remove(this.character);
                            character.Health.DoDamage(character.Health.maxHealth, DamageType.Zombie);
                        }
                    }
                    else
                    {
                        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackSwoopAfterAnimation, false);
                        trackEntry.Complete += (e) =>
                        {
                            trackEntry = null;
                            skeletonAnimation.AnimationState.ClearTrack(1);
                            aiMove.SpeedRecovery();
                        };
                    }
                    aiMove.MoveSpeed = 0;
                    SetTrailAndColliderActive(false, false, AttackSwoopBoxColider);
                    audioSource = null;
                    trackEntry = null;
                    aiMove.isSwoop = false;
                };
            };
            trackEntry.Dispose += (e) => { SetTrailAndColliderActive(false, false, AttackSwoopBoxColider); };
        }
    }

    private void SetSpeed()
    {
        aiMove.SpeedRecovery();
        aiMove.MoveSpeed *= SwoopSpeedMul;
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
