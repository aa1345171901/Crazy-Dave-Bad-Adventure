using UnityEngine;
using UnityEngine.Events;

namespace TopDownPlate
{
    /// <summary>
    /// 控制角色的运动,物理相关操作
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    [AddComponentMenu("TopDownPlate/Character/Core/Controller")]
    public class CharacterController : MonoBehaviour
    {
        public Rigidbody2D Rigidbody { get; protected set; }
        public BoxCollider2D BoxCollider { get; protected set; }

        protected Vector3 colliderBottomCenterPosition;
        protected Vector3 colliderLeftCenterPosition;
        protected Vector3 colliderRightCenterPosition;
        protected Vector3 colliderTopCenterPosition;

        public Vector3 ColliderSize
        {
            get
            {
                return Vector3.Scale(transform.localScale, BoxCollider.size);
            }
        }

        public virtual Vector3 ColliderBottomPosition
        {
            get
            {
                colliderBottomCenterPosition.x = BoxCollider.bounds.center.x;
                colliderBottomCenterPosition.y = BoxCollider.bounds.min.y;
                colliderBottomCenterPosition.z = 0;
                return colliderBottomCenterPosition;
            }
        }

        public virtual Vector3 ColliderLeftPosition
        {
            get
            {
                colliderLeftCenterPosition.x = BoxCollider.bounds.min.x;
                colliderLeftCenterPosition.y = BoxCollider.bounds.center.y;
                colliderLeftCenterPosition.z = 0;
                return colliderLeftCenterPosition;
            }
        }

        public virtual Vector3 ColliderTopPosition
        {
            get
            {
                colliderTopCenterPosition.x = BoxCollider.bounds.center.x;
                colliderTopCenterPosition.y = BoxCollider.bounds.max.y;
                colliderTopCenterPosition.z = 0;
                return colliderTopCenterPosition;
            }
        }

        public virtual Vector3 ColliderRightPosition
        {
            get
            {
                colliderRightCenterPosition.x = BoxCollider.bounds.max.x;
                colliderRightCenterPosition.y = BoxCollider.bounds.center.y;
                colliderRightCenterPosition.z = 0;
                return colliderRightCenterPosition;
            }
        }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            BoxCollider = GetComponent<BoxCollider2D>();
        }
    }
}
