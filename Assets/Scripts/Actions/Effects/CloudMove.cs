using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    [Tooltip("云移动目标位置")]
    public Transform originPos;
    [Tooltip("云移动目标位置")]
    public Transform TargetPos;

    private float speed;
    private Vector3 direction;

    private void Start()
    {
        direction = TargetPos.position - originPos.position;
        speed = Random.Range(0.01f, 0.05f);
    }

    private void Update()
    {
        this.transform.Translate(direction * Time.deltaTime * speed);
        if ((TargetPos.position - this.transform.position).magnitude <= 1f)
        {
            this.transform.position = originPos.position;
            speed = Random.Range(0.01f, 0.05f);
        }
    }
}
