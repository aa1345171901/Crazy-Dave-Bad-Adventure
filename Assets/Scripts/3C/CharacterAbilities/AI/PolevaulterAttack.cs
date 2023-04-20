using Spine;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[AddComponentMenu("TopDownPlate/AI/Ability/Attack/Polevaulter Attack")]
public class PolevaulterAttack : AIAttack
{
    [Space(10)]
    [Tooltip("�ᷢ����ս�����ľ��룬����ʱʹ��Զ��")]
    public float AttackRange = 3f;

    [Tooltip("�ý�ʬԶ�̹���ǰҡ������")]
    public string AttackBeforeAnimation = "Attack_Before";
    [Tooltip("�ý�ʬԶ�̹���������")]
    public string AttackAnimation = "Attack";
    [Tooltip("�ý�ʬԶ�̺�ҡ������")]
    public string AttackAfterAnimation = "Attack_After";

    [Tooltip("�ý�ʬ��ս����ǰҡ������")]
    public string AttackMeleeBeforeAnimation = "MeleeAttack_Before";
    [Tooltip("�ý�ʬ��ս����������")]
    public string AttackMeleeAnimation = "MeleeAttack";

    [Tooltip("��ǹԤ����")]
    public ZombiePole Pole;
    [Tooltip("��ǹ����λ��")]
    public Transform PolePos;
    [Tooltip("Զ�̹���������")]
    public LineRenderer lineRenderer;

    [Tooltip("�����Ĵ�����")]
    public BoxCollider2D AttackBoxColider;
    [Tooltip("��������β")]
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
        if (isAttacking && !aiMove.IsEnchanted)
        {
            if (character.FacingDirection == FacingDirections.Left)
                direction = new Vector3(-direction.x, direction.y);
            if (attackDirection != Vector3.zero)
                lineRenderer.SetPositions(new Vector3[2] { Vector3.zero, attackDirection * 10 });
            else
                lineRenderer.SetPositions(new Vector3[2] { Vector3.zero, direction * 10 });
        }
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
        // �жϴ�ʱ�Ƿ񹥻�
        float random = Random.Range(0, 1f);
        float nowAttackProbability = aiMove.AIParameter.Distance < AttackRange ? realAttackProbability : realAttackProbability * 2;
        if (random > nowAttackProbability)
            return;
        if (aiMove.AIParameter.Distance < AttackRange)
        {
            MelleAttack();
        }
        else
        {
            Attack();
        }
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

    private void MelleAttack()
    {
        isAttacking = true;
        healths.Clear();
        // ǰҡʱ���ѡ��ʬAudioSource,���û�ڲ����򲥷�
        audioSource = AudioManager.Instance.RandomPlayZombieSounds();

        // ����ǰҡ�����
        aiMove.MoveSpeed *= 2;
        controller.BoxCollider.enabled = false;
        SetTrailAndColliderActive(true, false, AttackBoxColider);
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackMeleeBeforeAnimation, false);
        trackEntry.Complete += (e) =>
        {
            // ����ǰҡ���˹����� �ٶ�Ϊ0
            aiMove.MoveSpeed = 0;
            controller.BoxCollider.enabled = true;
            trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackMeleeAnimation, false);
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
                        LevelManager.Instance.EnchantedEnemys.Remove(zombieAnimation.zombieType, this.character);
                        character.Health.DoDamage(character.Health.maxHealth, DamageType.Zombie);
                    }
                }

                // �����꽩ֱ0.2s�����ƶ�
                Invoke("SpeedRecovery", 0.2f);
                SetTrailAndColliderActive(false, false, AttackBoxColider);
                trackEntry = null;
                skeletonAnimation.AnimationState.ClearTrack(1);
                audioSource = null;
                isAttacking = false;
            };
        };
    }

    private void Attack()
    {
        isAttacking = true;
        audioSource = AudioManager.Instance.RandomPlayZombieSounds();

        // ����ǰҡ������ʱ������
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
                // �Ȼ󹥻������ж�
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
                else
                {
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
                    };
                }
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
