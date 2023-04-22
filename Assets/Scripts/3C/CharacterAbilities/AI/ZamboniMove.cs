using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[AddComponentMenu("TopDownPlate/AI/Ability/ZamboniMove")]
public class ZamboniMove : IAIMove
{
    [Tooltip("冰面预制体")]
    public List<GameObject> IceGround;

    private Bounds levelBounds;
    private float addY;
    private float nextIcePosX;  // 下一块冰位置

    protected override void Initialization()
    {
        base.Initialization();
        SetRealSpeed();
    }

    public override void Reuse()
    {
        base.Reuse();
        // X到底了随机向上或向下
        addY = Random.Range(0, 2) == 0 ? addY = 0.5f : addY = -0.5f;
        canMove = true;
        SetRealSpeed();
    }

    protected void SetRealSpeed()
    {
        int waveIndex = LevelManager.Instance.IndexWave + 1;
        if (waveIndex < 5)
            realSpeed = MoveSpeed = moveSpeed;
        else
            realSpeed = MoveSpeed = moveSpeed * ((waveIndex - 4) * 2 + 100) / 100;

        levelBounds = LevelManager.Instance.LevelBounds;
        // 如果在右半部分则面向左
        if (transform.position.x > levelBounds.center.x)
        {
            character.FacingDirection = FacingDirections.Right;
            nextIcePosX = transform.position.x - transform.position.x % 0.5f - 0.5f;
        }
        else
        {
            character.FacingDirection = FacingDirections.Left;
            nextIcePosX = transform.position.x - transform.position.x % 0.5f + 0.5f;
        }
    }

    public override void ProcessAbility()
    {
        if (character.IsDead)
        {
            controller.Rigidbody.velocity = Vector2.zero;
            return;
        }
        if (canMove)
        {
            if (transform.position.x > levelBounds.max.x)
            {
                // 右为默认方向，相对于僵尸的左边，因为僵尸默认面朝左
                character.FacingDirection = FacingDirections.Right;
                nextIcePosX = transform.position.x - transform.position.x % 0.5f - 0.5f;
                float posY = transform.position.y + addY;
                if (posY > levelBounds.max.y || posY < levelBounds.min.y)
                {
                    addY = -addY;
                    posY = transform.position.y + addY;
                }
                transform.position = new Vector3(levelBounds.max.x, posY, 0);
            }
            if (transform.position.x < levelBounds.min.x)
            {
                // 右为默认方向，相对于僵尸的左边，因为僵尸默认面朝左
                character.FacingDirection = FacingDirections.Left;
                nextIcePosX = transform.position.x - transform.position.x % 0.5f + 0.5f;
                float posY = transform.position.y + addY;
                if (posY > levelBounds.max.y || posY < levelBounds.min.y)
                {
                    addY = -addY;
                    posY = transform.position.y + addY;
                }
                transform.position = new Vector3(levelBounds.min.x, posY, 0);
            }
            var direction = character.FacingDirection == FacingDirections.Right ? Vector2.left : Vector2.right;
            controller.Rigidbody.velocity = direction * MoveSpeed;
            AIParameter.Distance = (GameManager.Instance.Player.transform.position - this.transform.position).magnitude;
            if (character.FacingDirection == FacingDirections.Right)
            {
                if (transform.position.x < nextIcePosX)
                {
                    CreateIce();
                    nextIcePosX -= 0.5f;
                }
            }
            else
            {
                if (transform.position.x > nextIcePosX)
                {
                    CreateIce();
                    nextIcePosX += 0.5f;
                }
            }
        }
        else
        {
            controller.Rigidbody.velocity = Vector2.zero;
        }
    }

    private void CreateIce()
    {
        int index = Random.Range(0, IceGround.Count);
        var ice = GameObject.Instantiate(IceGround[index], GameManager.Instance.IceGroundContent);
        ice.transform.position = new Vector3(nextIcePosX, transform.position.y + 0.5f);
    }

    public override void UpdateAnimator()
    {
        base.UpdateAnimator();
        if (character.IsDead)
        {
            if (character.NowTrackEntry != null)
                character.NowTrackEntry.TimeScale = 1;
        }
        else
        {
            if (canMove)
            {
                character.CharacterAnimationState = moveAnimationName;
                if (character.NowTrackEntry != null)
                    character.NowTrackEntry.TimeScale = MoveSpeed;
            }
        }
    }
}
