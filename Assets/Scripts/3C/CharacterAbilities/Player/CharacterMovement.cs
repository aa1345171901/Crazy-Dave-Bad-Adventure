using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/Character/Ability/Movement")]
    public class CharacterMovement : CharacterAbility
    {
        [Space(10)]
        [Header("MoveParameter")]
        public float moveSpeed = 3.5f;

        [Space(10)]
        [Header("RunParameter")]
        [Tooltip("奔跑相对平常速度的倍数")]
        public float runSpeedMultiple = 1.5f;

        [Tooltip("奔跑最大持续时间")]
        public float MaximumRunningTime = 5;

        [Tooltip("奔跑恢复效率，每多少dealtime恢复1")]
        public float RunningRecoveryEfficiency = 10;

        [Space(10)]
        [Header("SoundEffect")]
        [Tooltip("播放走路声音的AudioSource")]
        public AudioSource WalkAudio;
        [Tooltip("走路效果声音")]
        public List<AudioClip> WalkClips;

        [Space(10)]
        [Header("Info")]
        [ReadOnly]
        public Vector2 vectorInput;

        private bool runKeyDown;
        private float speed; // 控制动画速度
        private float runTimer; // 可以奔跑的时间
        private float realMaxRunTime; // 可以奔跑的最大时间时间，加成后
        private int recoveryTimer;  // 恢复计时器
        private bool runCancel;  // 时间用完设置为true,再次按下设为false;

        private readonly float defaultSpeed = 3f;

        private float finalMoveSpeed;

        // 是否能通过输入移动
        public bool canMove { get; set; }

        protected override void Initialization()
        {
            base.Initialization();
            Reuse();
        }

        public override void Reuse()
        {
            base.Reuse();
            canMove = true;
            realMaxRunTime = MaximumRunningTime + SaveManager.Instance.externalGrowthData.GetGrowSumValueByKey("runTime");
            runTimer = realMaxRunTime;
            GameManager.Instance.SetRunSliderWidth(realMaxRunTime / MaximumRunningTime);
            SetRuntimer();
            recoveryTimer = 0;

            finalMoveSpeed = moveSpeed * (200 + GameManager.Instance.UserData.Speed) / 200;
        }

        private void OnEnable()
        {
            var key = InputManager.GetKey("Run");
            key.Down += SetCancel;
            AudioManager.Instance.AudioLists.Add(WalkAudio);
            WalkAudio.volume = AudioManager.Instance.EffectPlayer.volume;
        }

        private void OnDisable()
        {
            AudioManager.Instance.AudioLists.Remove(WalkAudio);
            var key = InputManager.GetKey("Run");
            key.Down -= SetCancel;
        }

        private void SetCancel()
        {
            runCancel = false;
        }

        public override void ProcessAbility()
        {
            // canMove为是否收到外界力，收到力时不进行输入控制
            if (canMove)
            {
                if (!character.IsDead && GameManager.Instance.PlayerEnable)
                {
                    vectorInput.x = InputManager.GetAxis("Movement");
                    vectorInput.y = InputManager.GetAxis("Vertical");
                    runKeyDown = InputManager.GetKeyDown("Run");

                    float speedMultiple = 1;
                    if (vectorInput.x != 0 || vectorInput.y != 0)
                    {
                        if (runKeyDown && !runCancel)
                        {
                            runTimer -= Time.deltaTime;
                            SetRuntimer();
                            speedMultiple = runSpeedMultiple;
                        }
                    }

                    if (runTimer <= 0)
                        runCancel = true;

                    // 奔跑了就不增加恢复计时器
                    if (speedMultiple == 1)
                        recoveryTimer ++;

                    if (recoveryTimer >= RunningRecoveryEfficiency && runTimer < realMaxRunTime)
                    {
                        var mul = (100f + SaveManager.Instance.externalGrowthData.GetGrowSumValueByKey("physicalRecovery")) / 100;
                        runTimer += Time.deltaTime * mul;
                        SetRuntimer();
                        recoveryTimer = 0;
                    }

                    speed = Mathf.Max(Mathf.Abs(vectorInput.x), Mathf.Abs(vectorInput.y)) * (finalMoveSpeed * speedMultiple) / defaultSpeed;

                    if (vectorInput.x != 0 && vectorInput.y != 0)
                        vectorInput = vectorInput.normalized;
                    // 三叶草风速
                    float realMoveSpeed = character.FacingDirection == FacingDirections.Right ? finalMoveSpeed + GardenManager.Instance.BloverEffect.Windspeed : finalMoveSpeed;
                    realMoveSpeed *= GameManager.Instance.DecelerationRatio;
                    float xSpeed = vectorInput.x * realMoveSpeed;
                    float ySpeed = vectorInput.y * finalMoveSpeed;
                    controller.Rigidbody.velocity = new Vector2(xSpeed, ySpeed) * speedMultiple;
                }
                else
                {
                    controller.Rigidbody.velocity = Vector2.zero;
                    speed = 0;
                }
            }

            if (vectorInput.x > 0)
            {
                character.FacingDirection = FacingDirections.Right;
            }
            else if (vectorInput.x < 0)
            {
                character.FacingDirection = FacingDirections.Left;
            }
        }

        private void SetRuntimer()
        {
            float value = runTimer / realMaxRunTime;
            GameManager.Instance.SetRunSlider(value);
        }

        public void EatFood()
        {
            runTimer = realMaxRunTime;
            SetRuntimer();
        }

        public override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (!character.FindAbility<CharacterDash>().isDashing)
            {
                if (speed == 0)
                {
                    character.CharacterAnimationState = "EnterIdel";
                    WalkAudio.Stop();
                }
                else
                {
                    character.CharacterAnimationState = "Run";
                    if (character.NowTrackEntry != null)
                    {
                        if (character.State.PlayerStateType == PlayerStateType.Attack && speed < 0.5f)
                            character.NowTrackEntry.TimeScale = 1;
                        else
                            character.NowTrackEntry.TimeScale = speed;
                    }

                    if (!WalkAudio.isPlaying)
                    {
                        int index = Random.Range(0, WalkClips.Count);
                        WalkAudio.clip = WalkClips[index];
                        WalkAudio.Play();
                    }
                    if (runKeyDown)
                    {
                        WalkAudio.pitch = runSpeedMultiple * finalMoveSpeed / moveSpeed;
                    }
                    else
                    {
                        WalkAudio.pitch = 1 * finalMoveSpeed / moveSpeed;
                    }
                }
            }
        }
    }
}
