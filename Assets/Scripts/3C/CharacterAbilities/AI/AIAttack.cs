using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/AI/Ability/AIAttack")]
    public class AIAttack : CharacterAbility
    {
        [Space(10)]
        [Tooltip("�ý�ʬ����ǰҡ����ٶȱ���")]
        public float RushSpeedMul = 2f;

        [Tooltip("�ᷢ�������ľ���")]
        public float AttackRange = 5f;

        [Tooltip("�˴ι����˺�")]
        public int Damage = 2;

        [Tooltip("���������ĸ���")]
        [Range(0, 1)]
        public float AttackProbability = 0.5f;
        [Tooltip("�������ж�һ�¹���")]
        public float AttackJudgmentTime = 1f;

        [Tooltip("�ý�ʬ����ǰҡ������")]
        public string AttackBeforeAnimation = "Attack_Before";

        [Tooltip("�ý�ʬ����������")]
        public string AttackAnimation = "Attack";

        [Tooltip("�����Ĵ�����")]
        public BoxCollider2D AttackBoxColider;
        [Tooltip("��������β")]
        public List<GameObject> Trails;

        private Transform Target;
        private AIMove aiMove;
        private TrackEntry trackEntry;
        private float timer;
        private Trigger2D attackTrigger;

        // �Ƿ��Ȼ��Ȼ�
        private int attackCount;
        private List<Health> healths = new List<Health>();  // ����ʱ��գ���ֹ��ɶ���˺�

        [ReadOnly]
        public int realDamage;
        [ReadOnly]
        public float realAttackRange;
        [ReadOnly]
        public float realAttackProbability;

        private AudioSource audioSource;

        protected override void Initialization()
        {
            base.Initialization();
            Target = GameManager.Instance.Player.transform;
            aiMove = character.FindAbility<AIMove>();
            attackTrigger = AttackBoxColider.GetComponent<Trigger2D>();
            Reuse();
        }

        public override void Reuse()
        {
            trackEntry = null;
            int waveIndex = LevelManager.Instance.IndexWave + 1;
            this.realAttackRange = AttackRange + waveIndex / 10f;
            realAttackProbability = AttackProbability + waveIndex * 0.01f;
            if (waveIndex < 4)
            {
                this.realDamage = Damage;
            }
            else if (waveIndex < 9)
            {
                this.realDamage = (int)((Damage + 1) * (waveIndex / 4f));
            }
            else if (waveIndex < 13)
            {
                this.realDamage = (int)((Damage + 2) * (waveIndex / 3f));
            }
            else if (waveIndex < 17)
            {
                this.realDamage = (int)((Damage + 3) * (waveIndex / 1.5f));
            }
            else if (waveIndex < 21)
            {
                this.realDamage = (Damage + 4) * waveIndex;
            }
            else
            {
                this.realDamage = (int)((Damage + 5) * waveIndex * 1.5f);
            }
            SetTrailAndColliderActive(false, false);
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
            if (character.State.AIStateType == AIStateType.Init || character.IsDead)
            {
                audioSource?.Stop();
                return;
            }

            // ��ɫ�ڹ������������Ҵ�������
            if (attackTrigger.IsTrigger && AttackBoxColider.enabled)
            {
                if (GameManager.Instance.IsEnd || (aiMove.IsEnchanted))
                {
                    var target = attackTrigger.Target.GetComponent<Character>();
                    if (target != null && !healths.Contains(target.Health))
                    {
                        target.Health.DoDamage(realDamage, DamageType.ZombieHurEachOther);
                        healths.Add(target.Health);
                    }
                }
                else if (attackTrigger.Target == GameManager.Instance.Player.gameObject)
                {
                    GameManager.Instance.DoDamage(realDamage);
                }
            }
            if (Time.time - timer < AttackJudgmentTime)
                return;
            timer = Time.time;
            if (trackEntry != null)
                return;
            // �жϴ�ʱ�Ƿ񹥻�
            float random = Random.Range(0, 1f);
            if (random > realAttackProbability)
                return;
            Attack();
        }

        private void Attack()
        {
            healths.Clear();
            float distance = Vector3.Distance(this.transform.position, Target.transform.position);
            if (distance < realAttackRange)
            {
                // ǰҡʱ���ѡ��ʬAudioSource,���û�ڲ����򲥷�
                audioSource = AudioManager.Instance.RandomPlayZombieSounds();

                // ����ǰҡ�����
                aiMove.MoveSpeed *= RushSpeedMul;
                controller.BoxCollider.enabled = false;
                SetTrailAndColliderActive(true, false);
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackBeforeAnimation, false);
                trackEntry.Complete += (e) =>
                {
                    // ����ǰҡ���˹����� �ٶ�Ϊ0
                    aiMove.MoveSpeed = 0;
                    controller.BoxCollider.enabled = true;
                    trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAnimation, false);
                    SetTrailAndColliderActive(true, true);
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
                        SetTrailAndColliderActive(false, false);
                        trackEntry = null;
                        skeletonAnimation.AnimationState.ClearTrack(1);
                        audioSource = null;
                    };
                };
                trackEntry.Dispose += (e) => { SetTrailAndColliderActive(false, false); };
            }
        }

        private void SetTrailAndColliderActive(bool trailActive, bool coliderActive)
        {
            foreach (var item in Trails)
            {
                item.SetActive(trailActive);
            }
            AttackBoxColider.enabled = coliderActive;
        }

        private void SpeedRecovery()
        {
            aiMove.SpeedRecovery();
        }

        public void BeEnchanted(int attackCount, float percentageDamageAdd, int basicDamageAdd)
        {
            this.attackCount = attackCount;
            int waveIndex = LevelManager.Instance.IndexWave + 1;
            realAttackProbability = 1;
            if (waveIndex < 4)
            {
                this.realDamage = Damage + basicDamageAdd;
            }
            else if (waveIndex < 9)
            {
                this.realDamage = (int)((Damage + 1 + basicDamageAdd) * (waveIndex / 4f));
            }
            else if (waveIndex < 13)
            {
                this.realDamage = (int)((Damage + 2 + basicDamageAdd) * (waveIndex / 3f));
            }
            else if (waveIndex < 17)
            {
                this.realDamage = (int)((Damage + 3 + basicDamageAdd) * (waveIndex / 1.5f));
            }
            else if (waveIndex < 21)
            {
                this.realDamage = (Damage + 4 + basicDamageAdd) * waveIndex;
            }
            else
            {
                this.realDamage = (int)((Damage + 5 + basicDamageAdd) * waveIndex * 1.5f);
            }
            this.realDamage = (int)(realDamage * percentageDamageAdd);
        }
    }
}
