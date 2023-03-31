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
        /// ����ִ�У�ÿ֡ͨ��Character����
        /// </summary>
        public virtual void ProcessAbility()
        {

        }

        /// <summary>
        /// ���¶���
        /// </summary>
        public virtual void UpdateAnimator()
        {
            if (character == null || character.IsDead)
                return;
        }

        /// <summary>
        /// ����س�ʼ��
        /// </summary>
        public virtual void Reuse()
        {

        }
    }
}
