using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class BossBall : MonoBehaviour
{
    public float speed = 10;
    public GameObject lineRenderer;
    public List<GameObject> RollGameObject;
    public int Damage;

    private float angle;
    private float timer;

    private float nextPosX;
    private Bounds bounds;

    private readonly float GoTimer = 1.33f;
    private readonly float LiveTime = 5f;

    private void Start()
    {
        Invoke("DelayDestroy", LiveTime);
        bounds = LevelManager.Instance.LevelBounds;
        nextPosX = bounds.max.x;
        this.transform.position = new Vector2(bounds.max.x + 3, this.transform.position.y);
    }

    private void DelayDestroy()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > GoTimer)
        {
            lineRenderer.SetActive(false);
            this.transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            angle += 1;
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
            if (transform.position.x < nextPosX)
            {
                int index = Random.Range(0, RollGameObject.Count);
                var go = GameObject.Instantiate(RollGameObject[index], GameManager.Instance.IceGroundContent);
                go.transform.position = new Vector3(nextPosX, transform.position.y - 0.2f);
                go.transform.localScale = Vector3.one * 2;
                nextPosX -= 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.Player.gameObject)
            GameManager.Instance.DoDamage(Damage);
    }
}
