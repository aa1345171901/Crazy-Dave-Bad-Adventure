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
        [Tooltip("�ý�ʬ����ǰҡ����ٶ�")]
        public float RushSpeed = 4f;

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
                if (GameManager.Instance.IsEnd)
                {
                    var target = attackTrigger.Target.GetComponent<Character>();
                    if (target != null)
                        target.Health.DoDamage(Damage, DamageType.ZombieHurEachOther);
                }
                else
                {
                    GameManager.Instance.DoDamage(Damage);
                }
            }
            if (Time.time - timer < AttackJudgmentTime)
                return;
            timer = Time.time;
            if (trackEntry != null)
                return;
            // �жϴ�ʱ�Ƿ񹥻�
            float random = Random.Range(0, 1f);
            if (random > AttackProbability)
                return;
            Attack();
        }

        private void Attack()
        {
            float distance = Vector3.Distance(this.transform.position, Target.transform.position);
            if (distance < AttackRange)
            {
                // ǰҡʱ���ѡ��ʬAudioSource,���û�ڲ����򲥷�
                audioSource = AudioManager.Instance.RandomPlayZombieSounds();

                // ����ǰҡ�����
                aiMove.MoveSpeed = RushSpeed;
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
    }
}
