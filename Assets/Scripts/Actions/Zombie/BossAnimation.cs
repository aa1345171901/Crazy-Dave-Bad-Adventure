using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    [SpineAnimation]
    public string DeadAnimation = "Dead";
    [SpineAnimation]
    public string DeadLoopAnimation = "Deading";

    private Character character;

    private void Start()
    {
        character = this.transform.GetComponentInParent<Character>();
        character.Reuse();
        character.State.AIStateType = AIStateType.Run;
        character.Health.Reuse();
        LevelManager.Instance.Enemys.Add(ZombieType.Boss, character);
        AudioManager.Instance.PlayBossMusic();
    }

    public void Dead()
    {
        character.SkeletonAnimation.ClearState();
        var track = character.SkeletonAnimation.AnimationState.SetAnimation(0, DeadAnimation, false);
        track.Complete += (e) =>
        {
            character.SkeletonAnimation.AnimationState.SetAnimation(0, DeadLoopAnimation, true);
        };
        LevelManager.Instance.CacheEnemys.Remove(ZombieType.Boss, character);
        GameManager.Instance.Victory();
    }
}
