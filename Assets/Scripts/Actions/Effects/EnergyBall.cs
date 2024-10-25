using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public int addMaxHealthValue;
    public float speed;

    private Vector3 endPos;
    private Vector3 startPos;
    private Vector3 lastPos; // 计算子弹的角度
    private Vector3 controlPoint;
    private float currentPercent;
    private float percentSpeed;
    private float angle;
    private Vector2 direction;

    private bool isEnd;
    private bool isInit;
    AudioSource audioSource;

    private void InitBezier()
    {
        lastPos = startPos = this.transform.position;
        endPos = GameManager.Instance.Player.transform.position + Vector3.up;
        direction = endPos - startPos;
        controlPoint = GetControlPoint(startPos, endPos);
        percentSpeed = 1 / (direction.magnitude / speed);
        currentPercent = 0;
        angle = this.transform.eulerAngles.z;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
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

        float rd = Random.Range(-4, 4);
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
        if (currentPercent < 1)
        {
            currentPercent += percentSpeed * Time.deltaTime;
            endPos = GameManager.Instance.Player.transform.position + Vector3.up;
            this.transform.position = BezierUtils.BezierPoint(startPos, controlPoint, endPos, currentPercent);
            direction = (transform.position - lastPos).normalized;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
            lastPos = this.transform.position;
        }
        else if (!isEnd)
        {
            isEnd = true;
            GameManager.Instance.AddMaxHealth(addMaxHealthValue);
            audioSource.Play();
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            StartCoroutine(DelayDestroy());
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(1);
        GameObject.Destroy(gameObject);
    }
}
