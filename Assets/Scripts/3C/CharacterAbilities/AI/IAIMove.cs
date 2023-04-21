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

    [Tooltip("�ý�ʬ�ƶ�������")]
    public string moveAnimationName = "run_normal";

    public Transform HeadPos;
    public Transform BodyPos;

    public float MoveSpeed { get; set; } // ���õ�ǰ�ƶ��ٶ�

    [ReadOnly]
    public float realSpeed;  // ʵ�ʳ�ʼ�ٶ��ٶ�
    [ReadOnly]
    public bool canMove = false;  // ��ʼ��ʱ�����ƶ�
    [Tooltip("�ƶ�ʱ���������ǵľ���")]
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
