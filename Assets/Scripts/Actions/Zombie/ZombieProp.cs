using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ZombieProp : MonoBehaviour
{
    public ZombieType zombieType;
    public Character character;

    [Tooltip("���𶯻�1")]
    [SpineAnimation]
    public string WornAnimation1;
    [Tooltip("���𶯻�2")]
    [SpineAnimation]
    public string WornAnimation2;
    [Tooltip("���ߵ��䶯��")]
    [SpineAnimation]
    public string PropLostAnimation;

    public PropFall fallProp;

    private int animIndex; // ��һ�����õĶ����� �����ظ����ö���

    public void Injure()
    {
        int maxHp = character.Health.maxHealth;
        int hp = character.Health.health;
        if (hp <= maxHp / 4)
        {
            if (animIndex != 1)
            {
                animIndex = 1;
                character.SkeletonAnimation.AnimationState.SetAnimation(2, PropLostAnimation, false);
                if (fallProp != null)
                {
                    var prop = GameObject.Instantiate(fallProp);
                    fallProp.character = character;
                    prop.transform.position = this.transform.position + new Vector3(0, 1, 0);
                    prop.GetComponent<SpriteRenderer>().sortingOrder = character.LayerOrder + 1;
                }
            }
        }
        else if (hp <= maxHp / 2)
        {
            if (animIndex != 2)
            {
                animIndex = 2;
                character.SkeletonAnimation.AnimationState.SetAnimation(2, WornAnimation2, false);
            }
        }
        else if (hp <= maxHp * 3 / 4)
        {
            if (animIndex != 3)
            {
                character.SkeletonAnimation.AnimationState.SetAnimation(2, WornAnimation1, false);
                animIndex = 3;
            }
        }
    }
}
