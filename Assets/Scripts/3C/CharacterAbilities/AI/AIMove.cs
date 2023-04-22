using System;
using System.Collections;
using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/AI/Ability/AIMove")]
    public class AIMove : IAIMove
    {
        [Tooltip("僵尸受攻击被击退的时间")]
        public float RepulsiveTime = 0.2f;

        [Tooltip("被魅惑变颜色")]
        public HurtFlash hurtFlash;

        [Tooltip("被寒冰菇冻住后的冰块")]
        public GameObject Ice;
        [Tooltip("是否能被高坚果嘲讽")]
        public bool canTaunt = true;

        public float RepulsiveForce { get; set; }

        protected Transform Target;

        /// <summary>
        /// 是否被魅惑
        /// </summary>
        public bool IsEnchanted { get; set; }
        protected Character target;  // 被魅惑随机攻击的目标

        private float decelerationPercentage = 1; // 减速百分比
        private float decelerationTime;  // 减速时间
        private float decelerationTimer;  // 减速时刻
        private float finalMoveSpeed;

        [ReadOnly]
        public ZombieAnimation zombieAnimation;

        [ReadOnly]
        public bool isSwoop;  // 正在飞扑,不改变移动方向
        [ReadOnly]
        public Vector3 direction;

        protected override void Initialization()
        {
            base.Initialization();
            Target = GameManager.Instance.Player.transform;
            SetRealSpeed();
            zombieAnimation = GetComponentInChildren<ZombieAnimation>();
        }

        public override void Reuse()
        {
            base.Reuse();
            if (Ice != null)
                Ice.SetActive(false);
            canMove = true;
            isSwoop = false;
            if (IsEnchanted)
            {
                hurtFlash.BeResume();
                this.gameObject.layer = LayerMask.NameToLayer("Zombie");
            }
            IsEnchanted = false;
            SetRealSpeed();
            SpeedRecovery();
        }

        protected void SetRealSpeed()
        {
            int waveIndex = LevelManager.Instance.IndexWave + 1;
            if (waveIndex < 5)
                realSpeed = MoveSpeed = moveSpeed;
            else
                realSpeed = MoveSpeed = moveSpeed * ((waveIndex - 4) * 2 + 100) / 100;
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
                if (IsEnchanted && (target == null || target.IsDead))
                {
                    int index = UnityEngine.Random.Range(0, LevelManager.Instance.Enemys.Count);
                    var targetList = LevelManager.Instance.Enemys[index].Zombies;
                    if (targetList.Count > 0)
                    {
                        int i = UnityEngine.Random.Range(0, targetList.Count);
                        target = targetList[i];
                    }
                }
                if (!isSwoop)
                {
                    if (IsEnchanted && target != null)
                        direction = target.transform.position - this.transform.position;
                    else
                    {
                        if (GardenManager.Instance.TallNuts.Count > 0 && canTaunt)
                        {
                            int index = UnityEngine.Random.Range(0, GardenManager.Instance.TallNuts.Count);
                            direction = GardenManager.Instance.TallNuts[index].transform.position - this.transform.position;
                        }
                        else
                            direction = Target.position - this.transform.position;
                    }
                }

                AIParameter.Distance = (Target.position - this.transform.position).magnitude;
                if (AIParameter.Distance > 0.5f)
                {
                    direction = new Vector3(direction.x, direction.y, transform.position.z).normalized;
                    character.FacingDirection = direction.x < 0 ? FacingDirections.Right : FacingDirections.Left;
                    AIParameter.IsPlayerRight = character.FacingDirection == FacingDirections.Right ? true : false ;
                    //this.transform.Translate(direction * moveSpeed * Time.deltaTime);

                    // 三叶草风阻
                    finalMoveSpeed = character.FacingDirection == FacingDirections.Right ? MoveSpeed - GardenManager.Instance.BloverEffect.Windage : MoveSpeed;

                    if (decelerationPercentage != 1 && Time.time - decelerationTimer > decelerationTime)
                    {
                        decelerationPercentage = 1;
                        hurtFlash.BeResume();
                        if (Ice != null)
                            Ice.SetActive(false);
                        if (IsEnchanted)
                            hurtFlash.BeEnchanted();
                    }
                    // 减速
                    finalMoveSpeed *= decelerationPercentage;
                    controller.Rigidbody.velocity = direction * finalMoveSpeed;
                }
            }
            else
            {
                controller.Rigidbody.velocity = Vector2.zero;
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
                        character.NowTrackEntry.TimeScale = finalMoveSpeed;
                }
            }
        }

        /// <summary>
        /// 设置击退
        /// </summary>
        public void SetRepulsiveForce(float force)
        {
            canMove = false;
            switch (zombieAnimation.zombieType)
            {
                case ZombieType.Normal:
                case ZombieType.Flag:
                    RepulsiveForce = force;
                    break;
                case ZombieType.Cone:
                case ZombieType.Bucket:
                    RepulsiveForce = force / 2;
                    break;
                case ZombieType.Screendoor:
                    RepulsiveForce = force / 4;
                    break;
                default:
                    break;
            }
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

        public void BeEnchanted(int attackCount, float percentageDamageAdd, int basicDamageAdd)
        {
            this.IsEnchanted = true;
            hurtFlash.BeEnchanted();
            LevelManager.Instance.Enemys.Remove(zombieAnimation.zombieType, this.character);
            LevelManager.Instance.EnchantedEnemys.Add(zombieAnimation.zombieType, this.character);
            character.FindAbility<AIAttack>().BeEnchanted(attackCount, percentageDamageAdd, basicDamageAdd);
            this.gameObject.layer = LayerMask.NameToLayer("ZombieEnchanted");
        }

        public void BeDecelerated(float decelerationPercentage, float decelerationTime)
        {
            if (Ice == null)
                return;
            if (decelerationPercentage < this.decelerationPercentage)
                return;
            if (decelerationPercentage == 1)
            {
                Ice.SetActive(true);
                Ice.GetComponent<SpriteRenderer>().sortingOrder = character.LayerOrder + 1;
            }
            this.decelerationPercentage = 1 - decelerationPercentage;
            this.decelerationTime = decelerationTime;
            decelerationTimer = Time.time;
            hurtFlash.BeDecelerated();
        }
    }
}
