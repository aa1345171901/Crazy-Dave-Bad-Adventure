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
        [Tooltip("�������ƽ���ٶȵı���")]
        public float runSpeedMultiple = 1.5f;

        [Tooltip("����������ʱ��")]
        public float MaximumRunningTime = 5;

        [Tooltip("���ָܻ�Ч�ʣ�ÿ����dealtime�ָ�1")]
        public float RunningRecoveryEfficiency = 10;

        [Space(10)]
        [Header("SoundEffect")]
        [Tooltip("������·������AudioSource")]
        public AudioSource WalkAudio;
        [Tooltip("��·Ч������")]
        public List<AudioClip> WalkClips;

        [Space(10)]
        [Header("Info")]
        [ReadOnly]
        public Vector2 vectorInput;

        private bool runKeyDown;
        private float speed; // ���ƶ����ٶ�
        private float runTimer; // ���Ա��ܵ�ʱ��
        private int recoveryTimer;  // �ָ���ʱ��
        private bool runCancel;  // ʱ����������Ϊtrue,�ٴΰ�����Ϊfalse;

        private readonly float defaultSpeed = 3f;

        private float finalMoveSpeed;

        // �Ƿ���ͨ�������ƶ�
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
            runTimer = MaximumRunningTime;
            SetRuntimer();
            recoveryTimer = 0;

            finalMoveSpeed = moveSpeed * (100 + GameManager.Instance.UserData.Speed) / 100;
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
            var key = InputManager.GetKey("Run");
            key.Down -= SetCancel;
        }

        private void SetCancel()
        {
            runCancel = false;
        }

        public override void ProcessAbility()
        {
            // canMoveΪ�Ƿ��յ���������յ���ʱ�������������
            if (canMove)
            {
                if (!character.IsDead && !GameManager.Instance.IsDaytime)
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

                    // �����˾Ͳ����ӻָ���ʱ��
                    if (speedMultiple == 1)
                        recoveryTimer ++;

                    if (recoveryTimer >= RunningRecoveryEfficiency && runTimer < MaximumRunningTime)
                    {
                        runTimer += Time.deltaTime;
                        SetRuntimer();
                        recoveryTimer = 0;
                    }

                    speed = Mathf.Max(Mathf.Abs(vectorInput.x), Mathf.Abs(vectorInput.y)) * (finalMoveSpeed * speedMultiple) / defaultSpeed;

                    if (vectorInput.x != 0 && vectorInput.y != 0)
                        vectorInput = vectorInput.normalized;
                    // ��Ҷ�ݷ���
                    float realMoveSpeed = character.FacingDirection == FacingDirections.Right ? finalMoveSpeed + GardenManager.Instance.Windspeed : finalMoveSpeed;
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
            float value = runTimer / MaximumRunningTime;
            GameManager.Instance.SetRunSlider(value);
        }

        public override void UpdateAnimator()
        {
            base.UpdateAnimator();
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
