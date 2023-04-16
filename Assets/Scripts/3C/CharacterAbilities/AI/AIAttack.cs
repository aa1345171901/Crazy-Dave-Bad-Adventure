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
        [Tooltip("该僵尸攻击前摇冲刺速度倍率")]
        public float RushSpeedMul = 2f;

        [Tooltip("会发动攻击的距离")]
        public float AttackRange = 5f;

        [Tooltip("此次攻击伤害")]
        public int Damage = 2;

        [Tooltip("发动攻击的概率")]
        [Range(0, 1)]
        public float AttackProbability = 0.5f;
        [Tooltip("多少秒判断一下攻击")]
        public float AttackJudgmentTime = 1f;

        [Tooltip("该僵尸攻击前摇动画名")]
        public string AttackBeforeAnimation = "Attack_Before";

        [Tooltip("该僵尸攻击动画名")]
        public string AttackAnimation = "Attack";

        [Tooltip("攻击的触发器")]
        public BoxCollider2D AttackBoxColider;
        [Tooltip("攻击的拖尾")]
        public List<GameObject> Trails;

        private Transform Target;
        private AIMove aiMove;
        private TrackEntry trackEntry;
        private float timer;
        private Trigger2D attackTrigger;

        // 是否被魅惑菇魅惑
        private int attackCount;
        private List<Health> healths = new List<Health>();  // 攻击时清空，防止造成多次伤害

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

            // 角色在攻击触发器中且触发器打开
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
            // 判断此时是否攻击
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
                // 前摇时随机选择僵尸AudioSource,如果没在播放则播放
                audioSource = AudioManager.Instance.RandomPlayZombieSounds();

                // 攻击前摇，冲刺
                aiMove.MoveSpeed *= RushSpeedMul;
                controller.BoxCollider.enabled = false;
                SetTrailAndColliderActive(true, false);
                trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackBeforeAnimation, false);
                trackEntry.Complete += (e) =>
                {
                    // 攻击前摇完了攻击， 速度为0
                    aiMove.MoveSpeed = 0;
                    controller.BoxCollider.enabled = true;
                    trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, AttackAnimation, false);
                    SetTrailAndColliderActive(true, true);
                    trackEntry.Complete += (e) =>
                    {
                        // 魅惑攻击次数判断
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

                        // 攻击完僵直0.2s设置移动
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
