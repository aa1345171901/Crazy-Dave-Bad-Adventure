using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropFall : MonoBehaviour
{
    private float height;
    private float time;
    private float curTime;
    private Vector3 offsetSpeed;

    private float LiveTime = 2;

    private void Start()
    {
        Vector3 offset = new Vector3(Random.Range(0, 0.01f), Random.Range(-0.01f, 0.01f), 0);
        height = Random.Range(0.015f, 0.012f);
        time = Random.Range(0.4f, 0.6f);
        offsetSpeed = offset / time;
        Invoke("DestroyProp", LiveTime);
    }

    private void DestroyProp()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (curTime < time)
        {
            curTime += Time.deltaTime;
            Vector3 newPos = this.transform.position + offsetSpeed * curTime;
            newPos.y += Mathf.Cos(curTime / time * Mathf.PI - Mathf.PI / 2) * height;
            newPos.z = offsetSpeed.z;
            transform.position = newPos;
        }
    }
}
