using System;
using TopDownPlate;
using UnityEngine;

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

public class IAIMove : CharacterAbility
{
    [Space(10)]
    [Header("MoveParameter")]
    public float moveSpeed = 2f;

    [Tooltip("该僵尸移动动画名")]
    public string moveAnimationName = "run_normal";

    public Transform HeadPos;
    public Transform BodyPos;

    public float MoveSpeed { get; set; } // 设置当前移动速度

    [ReadOnly]
    public float realSpeed;  // 实际初始速度速度
    [ReadOnly]
    public bool canMove = false;  // 初始化时不能移动
    [Tooltip("移动时计算与主角的距离")]
    [ReadOnly]
    public AIParameter AIParameter;

    protected override void Initialization()
    {
        base.Initialization();
        AIParameter.HeadPos = HeadPos;
        AIParameter.BodyPos = BodyPos;
        AIParameter.Distance = float.MaxValue;
    }

    public override void Reuse()
    {
        base.Reuse();
        AIParameter.Distance = float.MaxValue;
    }
}
