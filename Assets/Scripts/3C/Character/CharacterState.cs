using System;
using UnityEngine;

namespace TopDownPlate
{
    public enum PlayerStateType
    {
        Idel = 0,
        Run,
        Attack,
    }

    public enum AIStateType
    {
        None = 0,
        Init,
        Run,
        Attack,
    }

    [Serializable]
    public class PlayerState : CharacterState
    {
        
    }

    [Serializable]
    public class AIState : CharacterState
    {
       
    }

    [Serializable]
    public class CharacterState
    {
        public PlayerStateType PlayerStateType;
        public AIStateType AIStateType;
    }
}
