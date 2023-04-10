using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class CattailSpikeBullet : SpikeBullet
{
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 lastPos; // 计算子弹的角度
    private Vector3 controlPoint;
    private float currentPercent;
    private float percentSpeed;
    private float angle;
    private Vector2 direction;

    private GameObject target;
    private bool isEnd;
    private bool isInit;

    private void InitBezier()
    {
        lastPos = startPos = this.transform.position;
        if (target == null)
        {
            var enemys = LevelManager.Instance.Enemys;
            if (enemys.Count > 0)
            {
                int randomIndex = Random.Range(0, enemys.Count);
                target = enemys[randomIndex].gameObject;
                endPos = target.transform.position;
            }
            else
            {
                endPos = startPos.normalized * 100;
            }
        }
        else
        {
            endPos = target.transform.position;
        }
        direction = endPos - startPos;
        controlPoint = GetControlPoint(startPos, endPos);
        percentSpeed = 1 / (direction.magnitude / Speed / Time.deltaTime);
        currentPercent = 0;
        angle = this.transform.eulerAngles.z;
    }

    /// <summary>
    /// 返回两点0.1位置垂线随机移动的控制点
    /// </summary>
    /// <param name="startPos">起始位置</param>
    /// <param name="endPos">目标位置</param>
    /// <returns>贝塞尔曲线控制点</returns>
    private Vector3 GetControlPoint(Vector3 startPos, Vector3 endPos)
    {
        Vector3 m = Vector3.Lerp(startPos, endPos, 0.1f);
        Vector3 normal = Vector2.Perpendicular(startPos - endPos).normalized;

        float rd = Random.Range(-2, 2);
        float curveRatio = 0.3f;

        return m + (startPos - endPos).magnitude * curveRatio * rd * normal;
    }

    private void Update()
    {
        if (!isInit)
        {
            InitBezier();
            isInit = true;
        }
        if (isEnd)
        {
            transform.Translate(direction * Speed * Time.deltaTime);
        }
        else
        {
            if (currentPercent < 1)
            {
                currentPercent += percentSpeed;
                // 目标还在则一直跟踪，否则就按当前方向直线运动
                if (target != null && target.activeSelf)
                    endPos = target.transform.position;
                else
                    isEnd = true;
                this.transform.position = BezierUtils.BezierPoint(startPos, controlPoint, endPos, currentPercent);
                direction = (transform.position - lastPos).normalized;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                this.transform.rotation = Quaternion.Euler(0, 0, angle);
                lastPos = this.transform.position;
            }
            else
            {
                if (target.activeSelf)
                    InitBezier();
            }
        }
    }
}
