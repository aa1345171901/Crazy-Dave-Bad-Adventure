using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPage : MonoBehaviour
{
    public SkeletonGraphic Dave;
    public Transform goContent;
    public AchievementItem achievementItem;

    public void Return()
    {
        StartCoroutine(DelayPlay());
    }

    IEnumerator DelayPlay()
    {
        yield return new WaitForSeconds(0.75f);
        Dave.AnimationState.SetAnimation(0, "MainMenuIdel", true);
    }

    public void OnEnter()
    {
        if (goContent.childCount == 0)
        {
            foreach (var item in ConfManager.Instance.confMgr.achievement.items)
            {
                var newAchievementItem = GameObject.Instantiate(achievementItem, goContent);
                newAchievementItem.gameObject.SetActive(true);
                newAchievementItem.InitData(item);
            }
        }
    }
}
