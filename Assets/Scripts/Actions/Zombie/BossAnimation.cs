using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    [SpineAnimation]
    public string DeadAnimation = "Dead";

    private Character character;

    private void Start()
    {
        character = this.transform.GetComponentInParent<Character>();
        character.Reuse();
        character.State.AIStateType = AIStateType.Run;
        character.Health.Reuse();
        LevelManager.Instance.Enemys.Add(ZombieType.Boss, character);
    }

    public void Dead()
    {
        character.SkeletonAnimation.ClearState();
        character.SkeletonAnimation.AnimationState.SetAnimation(0, DeadAnimation, false);
        LevelManager.Instance.CacheEnemys.Remove(ZombieType.Boss, character);
    }
}
