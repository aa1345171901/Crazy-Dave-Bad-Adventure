using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/Character/Ability/Dash")]
    public class CharacterDash : CharacterAbility
    {
        [Space(10)]
        [Header("DashParameter")]
        public float dashSpeed = 6;
        [Space(10)]
        public float defaultDashCoolTime = 10;
        [Space(10)]
        public int defaultDashCount = 1;

        [Space(10)]
        [Header("SoundEffect")]
        [Tooltip("冲刺音效")]
        public AudioSource DashAudio;

        public bool isDashing { get; private set; }
        private bool isDashKeyDown;

        private int dashCount;
        private int maxDashCount;

        private float dashCoolTime;

        private float finalDashSpeed;
        private float lastDashTime;

        private readonly float dashAnimLen = 1.33f;
        private readonly float speed = 3; // 控制动画速度

        protected override void Initialization()
        {
            base.Initialization();
            Reuse();
        }

        public override void Reuse()
        {
            base.Reuse();
            finalDashSpeed = dashSpeed * (100 + GameManager.Instance.UserData.Speed) / 100;
            dashCount = defaultDashCount + SaveManager.Instance.externalGrowthData.GetGrowSumValueByKey("dashTime");
            maxDashCount = dashCount;
            var mulCool = defaultDashCoolTime * (SaveManager.Instance.externalGrowthData.GetGrowSumValueByKey("dashRecovery")) / 100;
            dashCoolTime = defaultDashCoolTime - mulCool;
            lastDashTime = -dashCoolTime;
        }

        private void OnEnable()
        {
            AudioManager.Instance.AudioLists.Add(DashAudio);
            DashAudio.volume = AudioManager.Instance.EffectPlayer.volume;
        }

        private void OnDisable()
        {
        }

        private void OnDashDown()
        {
            if (!isDashing && dashCount > 0)
                isDashKeyDown = true;
        }

        public override void ProcessAbility()
        {
            if (!character.IsDead && GameManager.Instance.PlayerEnable)
            {
                if (Time.time - lastDashTime > dashCoolTime)
                {
                    if (dashCount < maxDashCount)
                    {
                        lastDashTime = Time.time;
                        dashCount++;
                    }
                }
                GameManager.Instance.SetDashSlider(maxDashCount, (Time.time - lastDashTime) / dashCoolTime, dashCount);

                var keyDown = InputManager.GetKeyDown("Dash");
                if (keyDown)
                    OnDashDown();

                if (isDashKeyDown)
                {
                    if (dashCount == maxDashCount)
                        lastDashTime = Time.time;
                    dashCount--;
                    isDashKeyDown = false;
                    isDashing = true;
                    if (!DashAudio.isPlaying)
                    {
                        DashAudio.Play();
                    }
                    StartCoroutine(Dash());
                }                
            }
        }

        IEnumerator Dash()
        {
            Vector2 vectorInput;
            vectorInput.x = InputManager.GetAxis("Movement");
            vectorInput.y = InputManager.GetAxis("Vertical");

            if (vectorInput.x != 0 && vectorInput.y != 0)
                vectorInput = vectorInput.normalized;
            if (vectorInput.x == 0 && vectorInput.y == 0)
                vectorInput = new Vector2(character.FacingDirection == FacingDirections.Right ? 1 : -1, 0);
            var velocity = vectorInput * finalDashSpeed;

            controller.Rigidbody.velocity = velocity;
            character.FindAbility<CharacterMovement>().canMove = false;
            character.Health.isInvincible = true;
            yield return new WaitForSeconds(dashAnimLen / speed);
            character.FindAbility<CharacterMovement>().canMove = true;
            character.Health.isInvincible = false;
            isDashing = false;
        }

        public override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (isDashing)
            {
                character.CharacterAnimationState = "RollingSprint";
                character.NowTrackEntry.TimeScale = speed;
            }
        }
    }
}
