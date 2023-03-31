using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TopDownPlate
{
    public enum CharacterTypes { Player, AI }

    public enum FacingDirections { Right, Left }

    /// <summary>
    /// 控制角色的各项能力执行以及动画系统
    /// </summary>
    [AddComponentMenu("TopDownPlate/Character/Core/Character")]
    public class Character : MonoBehaviour
    {
        [Header("Animator")]
        [Tooltip("指定播放的Animator,为null不播放动画")]
        public SkeletonAnimation SkeletonAnimation;

        [Space(10)]
        [Header("CharacterParameter")]
        public CharacterTypes CharacterType = CharacterTypes.Player;

        [Space(10)]
        [Header("Event")]
        public UnityEvent<int> SetLayerEvent;
        public Action<bool> FaceTurn;

        [Space(10)]
        [Header("Info")]
        [ReadOnly]
        [SerializeField] CharacterState state;

        public CharacterState State { get { return state; } }

        public int LayerOrder { get; set; }

        protected CharacterAbility[] characterAbilities;
        protected CharacterController controller;
        protected bool abilitiesHasInit = false;

        private Transform Model;
        protected FacingDirections facingDirection = FacingDirections.Right;
        public FacingDirections FacingDirection
        {
            get
            {
                return facingDirection;
            }
            set
            {
                if (facingDirection != value)
                {
                    facingDirection = value;
                    Model.transform.localScale = Vector3.Scale(new Vector3(-1, 1, 1), Model.transform.localScale);
                    FaceTurn?.Invoke(facingDirection == FacingDirections.Left);
                }
            }
        }
        protected bool isDead = false;
        public bool IsDead
        {
            get
            {
                return isDead;
            }
            set
            {
                if (isDead != value)
                {
                    isDead = value;
                    if (isDead)
                    {
                        if (CharacterType == CharacterTypes.Player)
                        {
                            CharacterAnimationState = "Dead";
                            GameManager.Instance.IsEnd = true;
                            this.controller.BoxCollider.enabled = false;
                        }
                    }
                    else
                    {
                         // 僵尸对象池重新利用
                        previousAnimationState = "";
                    }
                }
            }
        }

        public bool CanProcessAbility { get; set; } = true;

        public Health Health { get; private set; }

        public string CharacterAnimationState;
        private string previousAnimationState;
        public Spine.TrackEntry NowTrackEntry;

        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            if (SkeletonAnimation == null)
                SkeletonAnimation = this.gameObject.GetComponentInChildren<SkeletonAnimation>();
            AbilitiesInit();
            controller = this.gameObject.GetComponent<CharacterController>();
            Health = this.GetComponent<Health>();

            if (CharacterType == CharacterTypes.Player)
            {
                SkeletonAnimation.AnimationState.SetAnimation(0, "Idel", true);
                CharacterAnimationState = previousAnimationState = "Idel";
                state = new PlayerState();
            }
            else
            {
                state = new AIState();
            }
            Model = this.transform.Find("Model");
            SetLayer();
        }

        public void Reuse()
        {
            this.IsDead = false;
            foreach (var item in characterAbilities)
            {
                item.Reuse();
            }
        }

        protected virtual void AbilitiesInit()
        {
            if (abilitiesHasInit)
            {
                return;
            }
            CacheAbilities();
        }

        public virtual void CacheAbilities()
        {
            characterAbilities = this.gameObject.GetComponents<CharacterAbility>();
            abilitiesHasInit = true;
        }

        public T FindAbility<T>() where T : CharacterAbility
        {
            AbilitiesInit();
            foreach (var item in characterAbilities)
            {
                if (item is T characterAbility)
                    return characterAbility;
            }
            return null;
        }


        private void Update()
        {
            if (Time.timeScale != 0f && CanProcessAbility)
            {
                ProcessAbilities();
                SetLayer();
            }
            UpdateAnimators();
        }

        public void SetLayer()
        {
            // 使下面的挡住上面的需要将y取负号，所有值大于0需要加个值,layer只能设置为int，所以*10使精度变高再取整

            int y = (int)((-this.transform.position.y + 10) * 10);
            if (y != LayerOrder)
            {
                SkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = LayerOrder = y;
                SetLayerEvent?.Invoke(y);
            }
        }

        protected virtual void ProcessAbilities()
        {
            foreach (CharacterAbility ability in characterAbilities)
            {
                if (ability.enabled)
                {
                    ability.ProcessAbility();
                }
            }
        }

        protected virtual void UpdateAnimators()
        {
            if (IsDead)
            {
                if (previousAnimationState != CharacterAnimationState && CharacterType == CharacterTypes.Player)
                {
                    SkeletonAnimation.AnimationState.SetAnimation(0, CharacterAnimationState, false);
                    previousAnimationState = CharacterAnimationState;
                }
                return;
            }
            foreach (var item in characterAbilities)
            {
                item.UpdateAnimator();
            }

            if (!string.IsNullOrEmpty(CharacterAnimationState))
            {
                if (previousAnimationState != CharacterAnimationState)
                {
                    if (CharacterAnimationState == "EnterIdel")
                    {
                        var enterIdel = SkeletonAnimation.AnimationState.SetAnimation(0, CharacterAnimationState, false);
                        SkeletonAnimation.AnimationState.AddAnimation(0, "Idel", true, 0);   // 在播放完EnterIdel之后进入Idel动画
                    }
                    else
                    {
                        NowTrackEntry = SkeletonAnimation.AnimationState.SetAnimation(0, CharacterAnimationState, true);
                    }
                    previousAnimationState = CharacterAnimationState;
                }
            }
        }
    }
}
