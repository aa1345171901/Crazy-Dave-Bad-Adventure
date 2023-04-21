using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class IceGround : MonoBehaviour
{
    [Tooltip("减速比例")]
    [Range(0, 1)]
    public float DecelerationRatio;

    [Tooltip("存在时间")]
    public float liveTime = 20;

    private void Start()
    {
        Invoke("DestroyDelay", liveTime);
    }

    private void DestroyDelay()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.Player.gameObject)
        {
            GameManager.Instance.DecelerationRatio = DecelerationRatio;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.Player.gameObject)
        {
            GameManager.Instance.DecelerationRatio = 1;
        }
    }
}
