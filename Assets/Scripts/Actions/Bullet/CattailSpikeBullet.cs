using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class CattailSpikeBullet : SpikeBullet
{
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 lastPos; // �����ӵ��ĽǶ�
    private Vector3 controlPoint;
    private float currentPercent;
    private float percentSpeed;
    private float angle;
    private Vector2 direction;

    private GameObject target;
    private bool bezierInit;

    private void Start()
    {
        InitBezier();
    }

    private void InitBezier()
    {
        lastPos = startPos = this.transform.position;
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
        direction = endPos - startPos;
        controlPoint = GetControlPoint(startPos, endPos);
        percentSpeed = 1 / (direction.magnitude / Speed / Time.deltaTime);
        currentPercent = 0;
        bezierInit = true;
        angle = this.transform.eulerAngles.z;
    }

    /// <summary>
    /// ��������0.1λ�ô�������ƶ��Ŀ��Ƶ�
    /// </summary>
    /// <param name="startPos">��ʼλ��</param>
    /// <param name="endPos">Ŀ��λ��</param>
    /// <returns>���������߿��Ƶ�</returns>
    private Vector3 GetControlPoint(Vector3 startPos, Vector3 endPos)
    {
        Vector3 m = Vector3.Lerp(startPos, endPos, 0.1f);
        Vector3 normal = Vector2.Perpendicular(startPos - endPos).normalized;

        float rd = Random.Range(-2, 2);
        float curveRatio = 0.3f;

        return m + (startPos - endPos).magnitude * curveRatio * rd * normal;
    }

    void Update()
    {
        if (currentPercent < 1)
        {
            currentPercent += percentSpeed;
            if (target != null && target.activeSelf)
                endPos = target.transform.position;
            this.transform.position = BezierUtils.BezierPoint(startPos, controlPoint, endPos, currentPercent);
            direction = (transform.position - lastPos).normalized;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
            lastPos = this.transform.position;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
