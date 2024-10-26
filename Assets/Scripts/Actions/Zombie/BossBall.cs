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
    private Vector2 direction;

    private readonly float GoTimer = 1.33f;
    private readonly float LiveTime = 5f;

    public bool isLeft;

    private void Start()
    {
        Invoke("DelayDestroy", LiveTime);
        bounds = LevelManager.Instance.LevelBounds;
        float startX = isLeft ? bounds.min.x : bounds.max.x + 3;
        this.transform.position = new Vector2(startX, this.transform.position.y);
        direction = isLeft ? Vector2.right : Vector2.left;
        nextPosX = isLeft ? bounds.min.x : bounds.max.x;
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
            this.transform.Translate(direction * speed * Time.deltaTime, Space.World);
            angle += 1;
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
            if (transform.position.x < nextPosX && !isLeft)
            {
                CreateGround();
            }

            if (transform.position.x > nextPosX && isLeft)
            {
                CreateGround();
            }
        }
    }

    private void CreateGround()
    {
        if (nextPosX > LevelManager.Instance.LevelBounds.max.x)
            return;
        int index = Random.Range(0, RollGameObject.Count);
        var go = GameObject.Instantiate(RollGameObject[index], GameManager.Instance.IceGroundContent);
        go.transform.position = new Vector3(nextPosX, transform.position.y - 0.2f);
        go.transform.localScale = Vector3.one * 2;
        if (isLeft)
            nextPosX++;
        else
            nextPosX --;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.Player.gameObject)
            GameManager.Instance.DoDamage(Damage, ZombieType.Boss);
    }
}
