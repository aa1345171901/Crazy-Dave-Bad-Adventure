using System;
using System.Collections;
using UnityEngine;

namespace TopDownPlate
{
    [Serializable]
    public class AIParameter
    {
        public enum AttackPos
        {
            Head,
            Body,
        }

        public float Distance;
        public bool IsPlayerRight;
        public Transform HeadPos;
        public Transform BodyPos;

        public AttackPos attackPos;
    }

    [AddComponentMenu("TopDownPlate/AI/Ability/AIMove")]
    public class AIMove : CharacterAbility
    {
        [Space(10)]
        [Header("MoveParameter")]
        public float moveSpeed = 2f;

        [Tooltip("该僵尸移动动画名")]
        public string moveAnimationName = "run_normal";

        [Tooltip("僵尸受攻击被击退的时间")]
        public float RepulsiveTime = 0.2f;

        public Transform HeadPos;
        public Transform BodyPos;

        private Transform Target;

        [ReadOnly]
        public bool canMove = false;  // 初始化时不能移动

        [Tooltip("移动时计算与主角的距离")]
        [ReadOnly]
        public AIParameter AIParameter;

        public float MoveSpeed { get; set; }
        public float RepulsiveForce { get; set; }

        private Vector3 direction;
        public float realSpeed;

        protected override void Initialization()
        {
            base.Initialization();
            Target = GameManager.Instance.Player.transform;
            SetRealSpeed();
            AIParameter.HeadPos = HeadPos;
            AIParameter.BodyPos = BodyPos;
            AIParameter.Distance = float.MaxValue;
        }

        public override void Reuse()
        {
            canMove = true;
            AIParameter.Distance = float.MaxValue;
            SetRealSpeed();
            SpeedRecovery();
        }

        private void SetRealSpeed()
        {
            int waveIndex = LevelManager.Instance.IndexWave + 1;
            if (waveIndex < 5)
                realSpeed = MoveSpeed = moveSpeed;
            else
                realSpeed = MoveSpeed = moveSpeed * ((waveIndex - 4) * 3 + 100) / 100;
        }

        public override void ProcessAbility()
        {
            if (character.IsDead)
            {
                controller.Rigidbody.velocity = Vector2.zero;
                return;
            }
            if (canMove)
            {
                direction = Target.position - this.transform.position;
                AIParameter.Distance = direction.magnitude;
                if (AIParameter.Distance > 0.5f)
                {
                    direction = new Vector3(direction.x, direction.y, transform.position.z).normalized;
                    character.FacingDirection = direction.x < 0 ? FacingDirections.Right : FacingDirections.Left;
                    AIParameter.IsPlayerRight = character.FacingDirection == FacingDirections.Right ? true : false ;
                    //this.transform.Translate(direction * moveSpeed * Time.deltaTime);

                    // 三叶草风阻
                    float finalMoveSpeed = character.FacingDirection == FacingDirections.Right ? MoveSpeed - GardenManager.Instance.Windage : MoveSpeed;
                    controller.Rigidbody.velocity = direction * finalMoveSpeed;
                }
            }
        }

        public override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (character.IsDead)
            {
                if (character.NowTrackEntry != null)
                    character.NowTrackEntry.TimeScale = 1;
            }
            else
            {
                if (canMove)
                {
                    character.CharacterAnimationState = moveAnimationName;
                    if (character.NowTrackEntry != null)
                        character.NowTrackEntry.TimeScale = MoveSpeed;
                }
            }
        }

        /// <summary>
        /// 设置击退
        /// </summary>
        public void SetRepulsiveForce(float force)
        {
            canMove = false;
            RepulsiveForce = force;
            StartCoroutine(Repulsive());
        }

        IEnumerator Repulsive()
        {
            controller.Rigidbody.AddForce((this.transform.position - Target.position).normalized * RepulsiveForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(RepulsiveTime);
            canMove = true;
        }

        public void SpeedRecovery()
        {
            MoveSpeed = realSpeed;
        }

        public void SetBrainPos()
        {
            Target = GameManager.Instance.BrainPos;
        }
    }
}
