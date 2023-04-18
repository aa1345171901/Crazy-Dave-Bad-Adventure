using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/Character/Ability/Attack")]
    public class CharacterAttack : CharacterAbility
    {
        [Tooltip("攻击伤害")]
        public int Damage = 5;

        [Tooltip("攻击范围，范围内自动攻击")]
        public float AttackRange = 2;

        [Tooltip("攻击速度")]
        public float AttackSpeed = 10;

        [Tooltip("击退的力")]
        public float RepulsiveForce = 2;

        [Tooltip("武器的骨骼,平底锅")]
        public SkeletonUtilityBone WeaponPot;

        [Tooltip("攻击动画名,右边")]
        public string AttackRightAnimation = "AttackRight";

        [Tooltip("攻击动画名，左边")]
        public string AttackLeftAnimation = "AttackLeft";

        [Tooltip("获取锅的动画")]
        public string GetPotAnimation = "GetPot";

        private GameObject Pot;
        private AudioEffect potAudio;
        public Character target;
        private Vector3 direction;
        private bool isInFlight;
        private bool isAttacking;
        private AIMove targetAIMove;
        private Transform targetPos;

        private Vector3 startPos;
        private Vector3 endPos;
        private Vector3 controlPoint;
        private float angle;
        private float currentPercent;
        private float percentSpeed;
        private bool bezierInit = false;

        private int finalDamage;
        private int finalAttackRecovery;
        public float finalAttackSpped;
        private float finalRepulsiveForce;
        private float finalRange;
        private int finalCriticalHitRate;

        private float planternAttackSpeed;
        /// <summary>
        /// 路灯附近提供的攻速
        /// </summary>
        public float PlanternAttackSpeed
        {
            get
            {
                return planternAttackSpeed;
            }
            set
            {
                if (value == 0)
                    finalAttackSpped = finalAttackSpped / planternAttackSpeed;
                planternAttackSpeed = value;
                if (planternAttackSpeed != 0)
                    finalAttackSpped = finalAttackSpped * planternAttackSpeed;
            }
        }

        private float planternDamage;
        /// <summary>
        /// 路灯附近提升的伤害
        /// </summary>
        public float PlanternDamage
        {
            get
            {
                return planternDamage;
            }
            set
            {
                if (value == 0)
                    finalDamage = (int)(finalDamage / planternDamage);
                planternDamage = value;
                if (planternDamage != 0)
                    finalDamage = (int)(finalDamage * planternDamage);
            }
        }

        private float iceShroomAttackSpeed;
        /// <summary>
        /// 寒冰菇提供的攻速
        /// </summary>
        public float IceShroomAttackSpeed
        {
            get
            {
                return iceShroomAttackSpeed;
            }
            set
            {
                if (value == 0)
                    finalAttackSpped = finalAttackSpped / iceShroomAttackSpeed;
                iceShroomAttackSpeed = value;
                if (iceShroomAttackSpeed != 0)
                    finalAttackSpped = finalAttackSpped * iceShroomAttackSpeed;
            }
        }

        protected override void Initialization()
        {
            base.Initialization();
            Pot = GameManager.Instance.Pot;
            potAudio = Pot.GetComponent<AudioEffect>();
            Reuse();
        }

        public override void Reuse()
        {
            base.Reuse();
            var userData = GameManager.Instance.UserData;
            finalDamage = Mathf.RoundToInt((userData.Power + Damage) * (100f + userData.PercentageDamage) / 100);
            finalRepulsiveForce = RepulsiveForce + (userData.Power / 10f);
            finalAttackSpped = AttackSpeed * (100 + userData.AttackSpeed) / 100;
            finalRange = AttackRange * (100 + userData.Range) / 100;
            finalCriticalHitRate = userData.CriticalHitRate;
            finalAttackRecovery = userData.Adrenaline;
            finalDamage = (int)(finalDamage * GardenManager.Instance.GravebusterDamage);

            int spinaciaCount = ShopManager.Instance.PurchasePropCount("Spinacia");
            spinaciaCount = spinaciaCount > 4 ? 4 : spinaciaCount;
            GameManager.Instance.ZombieFlyDamage = spinaciaCount * finalDamage / 4;
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
            if (character.IsDead)
                return;

            // 攻击直线位移
            if (isInFlight)
            {
                direction = targetPos.position - Pot.transform.position;
                if (direction.magnitude < 0.25f)
                {
                    if (GameManager.Instance.IsZombieShock)
                        targetAIMove.SetRepulsiveForce(finalRepulsiveForce);
                    bool isCriticalHitRate = Random.Range(0, 101) < finalCriticalHitRate;
                    if (isCriticalHitRate)
                        target.Health.DoDamage(Mathf.RoundToInt(finalDamage * 1.5f), DamageType.Pot, true);
                    else
                        target.Health.DoDamage(finalDamage, DamageType.Pot);

                    bool isAttackRecovery = Random.Range(0, 101) < finalAttackRecovery;
                    if (isAttackRecovery)
                    {
                        int recoveryHp = finalAttackRecovery / 10;
                        GameManager.Instance.AddHP(recoveryHp == 0 ? 1 : recoveryHp);
                    }
                    InitBezier();
                    isInFlight = false;
                    GameManager.Instance.pumpkinHead.Falling(Pot.transform.position);
                }
                else
                {
                    direction = new Vector3(direction.x, direction.y, transform.position.z).normalized;
                    int y = (int)((-Pot.transform.position.y + 10) * 10);
                    Pot.GetComponent<SpriteRenderer>().sortingOrder = y;
                    Pot.transform.Translate(direction * finalAttackSpped * Time.deltaTime, Space.World);
                }
            }

            // 回程使用贝塞尔曲线
            if (bezierInit)
            {
                BezierMove();
            }

            if (!isAttacking)
            {
                // 从集合中寻找位置最近的敌人
                target = LevelManager.Instance.GetRecentlyEnemy(out bool isRight, finalRange);
                character.State.PlayerStateType = PlayerStateType.Attack;
                if (target != null)
                {
                    targetAIMove = target.FindAbility<AIMove>();
                    isAttacking = true;
                    Attack(isRight);
                }
            }
        }

        private void BezierMove()
        {
            if (currentPercent < 1)
            {
                currentPercent += percentSpeed;
                endPos = WeaponPot.transform.position;
                Pot.transform.position = BezierUtils.BezierPoint(startPos, controlPoint, endPos, currentPercent);
                angle += Random.Range(7, 14);
                Pot.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                isAttacking = false;
                Pot.SetActive(false);
                bezierInit = false;
                character.SkeletonAnimation.AnimationState.SetAnimation(1, GetPotAnimation, false);
                character.State.PlayerStateType = PlayerStateType.Idel;
            }
        }

        private void InitBezier()
        {
            startPos = Pot.transform.position;
            endPos = WeaponPot.transform.position;
            direction = endPos - startPos;
            controlPoint = GetControlPoint(startPos, endPos);
            percentSpeed = 1 / (direction.magnitude / finalAttackSpped / Time.deltaTime);
            currentPercent = 0;
            bezierInit = true;
            potAudio.ImpactAudioPlay(targetAIMove.AIParameter.attackPos == AIParameter.AttackPos.Body, targetAIMove.GetComponentInChildren<ZombieAnimation>().zombieType);
        }

        /// <summary>
        /// 返回两点0.1位置垂线随机移动的控制点
        /// </summary>
        /// <param name="startPos">起始位置</param>
        /// <param name="endPos">目标位置</param>
        /// <returns>贝塞尔曲线控制点</returns>
        private Vector3 GetControlPoint(Vector3 startPos, Vector3 endPos)
        {
            Vector3 m = Vector3.Lerp(startPos, endPos, 0.1f);
            Vector3 normal = Vector2.Perpendicular(startPos - endPos).normalized;
            float rd;
            if (direction.x < 0)
                rd = Random.Range(2, 4);
            else
                rd = Random.Range(-2, -4);
            float curveRatio = 0.3f;

            return m + direction.magnitude * curveRatio * rd * normal;
        }

        private void Attack(bool isRight)
        {
            TrackEntry entry;
            if (isRight)
            {
                if (character.FacingDirection == FacingDirections.Left)
                    entry = character.SkeletonAnimation.AnimationState.SetAnimation(1, AttackRightAnimation, false);
                else
                    entry = character.SkeletonAnimation.AnimationState.SetAnimation(1, AttackLeftAnimation, false);
            }
            else
            {
                if (character.FacingDirection == FacingDirections.Left)
                    entry = character.SkeletonAnimation.AnimationState.SetAnimation(1, AttackLeftAnimation, false);
                else
                    entry = character.SkeletonAnimation.AnimationState.SetAnimation(1, AttackRightAnimation, false);
            }
            entry.TimeScale = finalAttackSpped / 10;
            entry.Complete += (e) =>
            {
                character.SkeletonAnimation.AnimationState.ClearTrack(1);
                Pot.transform.SetPositionAndRotation(WeaponPot.transform.position, WeaponPot.transform.rotation);
                Pot.SetActive(true);
                potAudio.ThrowOutPlay();
                angle = WeaponPot.transform.rotation.z;
                isInFlight = true;
                int index = Random.Range(0, 8);
                targetPos = targetAIMove.AIParameter.HeadPos.transform;
                targetAIMove.AIParameter.attackPos = AIParameter.AttackPos.Head;
                if (index <= 3)
                {
                    targetPos = targetAIMove.AIParameter.BodyPos.transform;
                    targetAIMove.AIParameter.attackPos = AIParameter.AttackPos.Body;
                }
            };

        }
    }
}
