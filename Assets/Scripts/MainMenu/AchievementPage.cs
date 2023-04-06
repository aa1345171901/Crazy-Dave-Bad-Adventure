using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPage : MonoBehaviour
{
    public SkeletonGraphic Dave;

    public void Return()
    {
        this.gameObject.SetActive(false);
        Dave.AnimationState.SetAnimation(0, "MainMenuIdel", true);
    }
}
