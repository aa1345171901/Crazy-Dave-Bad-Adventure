using Spine.Unity;
using UnityEngine;

namespace TopDownPlate
{
    public class CharacterAbility : MonoBehaviour
    {
        protected Character character;
        protected CharacterController controller;
        protected SkeletonAnimation skeletonAnimation;

        private void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            character = this.GetComponent<Character>();
            controller = this.GetComponent<CharacterController>();
            if (character != null)
            {
                skeletonAnimation = character.SkeletonAnimation;
            }
        }

        /// <summary>
        /// 能力执行，每帧通过Character调用
        /// </summary>
        public virtual void ProcessAbility()
        {

        }

        /// <summary>
        /// 更新动画
        /// </summary>
        public virtual void UpdateAnimator()
        {
            if (character == null || character.IsDead)
                return;
        }

        /// <summary>
        /// 对象池初始化
        /// </summary>
        public virtual void Reuse()
        {

        }
    }
}
