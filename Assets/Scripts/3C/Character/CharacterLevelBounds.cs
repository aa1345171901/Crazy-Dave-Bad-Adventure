using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/Character/Core/CharacterLevelBounds")]
    public class CharacterLevelBounds : MonoBehaviour
    {
		public enum BoundsBehavior
		{
			Nothing,
			Constrain,
			Kill
		}

		[Tooltip("�����ϱ�Ե�����¼�")]
		public BoundsBehavior Top = BoundsBehavior.Constrain;
		[Tooltip("�����±�Ե�����¼�")]
		public BoundsBehavior Bottom = BoundsBehavior.Kill;
		[Tooltip("�������Ե�����¼�")]
		public BoundsBehavior Left = BoundsBehavior.Constrain;
		[Tooltip("�����ұ�Ե�����¼�")]
		public BoundsBehavior Right = BoundsBehavior.Constrain;

		protected Bounds bounds;
		protected CharacterController controller;
		protected Character character;
		protected BoxCollider2D boxCollider;
		protected Vector2 constrainedPosition;

		public virtual void Start()
		{
			character = this.gameObject.GetComponent<Character>();
			controller = this.gameObject.GetComponent<CharacterController>();
			boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
			if (LevelManager.HasInstance)
			{
				bounds = LevelManager.Instance.LevelBounds;
			}
		}

		public virtual void LateUpdate()
		{
			if (character.IsDead || (!LevelManager.HasInstance))
				return;
			Physics2D.SyncTransforms();
			bounds = LevelManager.Instance.LevelBounds;

			if (bounds.size != Vector3.zero)
			{
				if ((Top != BoundsBehavior.Nothing) && (controller.ColliderTopPosition.y > bounds.max.y))
				{
					constrainedPosition.x = transform.position.x;
					constrainedPosition.y = bounds.max.y - controller.ColliderSize.y / 2;
					ApplyBoundsBehavior(Top, constrainedPosition);
				}

				if ((Bottom != BoundsBehavior.Nothing) && (controller.ColliderBottomPosition.y < bounds.min.y))
				{
					constrainedPosition.x = transform.position.x;
					constrainedPosition.y = bounds.min.y + controller.ColliderSize.y / 2;
					ApplyBoundsBehavior(Bottom, constrainedPosition);
				}

				if ((Right != BoundsBehavior.Nothing) && (controller.ColliderRightPosition.x > bounds.max.x))
				{
					constrainedPosition.x = bounds.max.x - controller.ColliderSize.x / 2;
					constrainedPosition.y = transform.position.y;
					ApplyBoundsBehavior(Right, constrainedPosition);
				}

				if ((Left != BoundsBehavior.Nothing) && (controller.ColliderLeftPosition.x < bounds.min.x))
				{
					constrainedPosition.x = bounds.min.x + controller.ColliderSize.x / 2;
					constrainedPosition.y = transform.position.y;
					ApplyBoundsBehavior(Left, constrainedPosition);
				}
			}
		}

		protected virtual void ApplyBoundsBehavior(BoundsBehavior behavior, Vector2 constrainedPosition)
		{
			if ((character == null)
				 || (!LevelManager.HasInstance))
			{
				return;
			}

			if (behavior == BoundsBehavior.Kill)
			{
				if (character.CharacterType == CharacterTypes.Player)
				{
					character.Health.DoDamage(1);
				}
				else
				{
					//
				}
				return;
			}

			if (behavior == BoundsBehavior.Constrain)
			{
				transform.position = constrainedPosition;
				Physics2D.SyncTransforms();
				return;
			}
		}
	}
}
