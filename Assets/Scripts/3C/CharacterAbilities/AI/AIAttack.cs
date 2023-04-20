using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    public class AIAttack : CharacterAbility
    {
        [Tooltip("此次攻击伤害")]
        public int Damage = 2;

        [Tooltip("发动攻击的概率")]
        [Range(0, 1)]
        public float AttackProbability = 0.14f;
        [Tooltip("多少秒判断一下攻击")]
        public float AttackJudgmentTime = 1f;

        protected Transform Target;
        protected AIMove aiMove;

        // 是否被魅惑菇魅惑
        protected int attackCount;
        protected List<Health> healths = new List<Health>();  // 攻击时清空，防止造成多次伤害

        [ReadOnly]
        public int realDamage;
        [ReadOnly]
        public float realAttackProbability;

        protected AudioSource audioSource;

        protected override void Initialization()
        {
            base.Initialization();
            Target = GameManager.Instance.Player.transform;
            aiMove = character.FindAbility<AIMove>();
            character.Health.DeadAction += Dead;
            Reuse();
        }

        public override void Reuse()
        {
            int waveIndex = LevelManager.Instance.IndexWave + 1;
            realAttackProbability = AttackProbability + waveIndex * 0.01f;
        }

        public virtual void BeEnchanted(int attackCount, float percentageDamageAdd, int basicDamageAdd)
        {
            this.attackCount = attackCount;
            realAttackProbability = 1;
        }

        protected virtual void Dead()
        {

        }
    }
}
