using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSeedCard : MonoBehaviour
{
    public SpriteRenderer plant;
    public SpriteRenderer bg;
    public ManualPlantSeedCard manualPlant;

    public int plantType { get; set; }
    public Vector3 targetPos { get; set; }

    ManualPlantSeedCard nowPlantCard;
    Animator animator;

    bool isSelect;
    bool isMove = true;
    float time;
    float curTimer;
    Vector3 startPos;

    /// <summary>
    /// 存在时间
    /// </summary>
    readonly float lifeTime = 15f;

    private void Start()
    {
        var confCardItem = ConfManager.Instance.confMgr.plantCards.GetPlantCardByType(plantType);
        Sprite bg = Resources.Load<Sprite>(confCardItem.plantBgImagePath);
        this.bg.sprite = bg;

        Sprite plantImage = Resources.Load<Sprite>(confCardItem.plantImagePath);
        this.plant.sprite = plantImage;

        time = Random.Range(3, 5);
        startPos = this.transform.position;

        animator = GetComponent<Animator>();
        StartCoroutine(DelayDestroy());
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(lifeTime - 3);
        animator.enabled = true;
        yield return new WaitForSeconds(3);
        if (nowPlantCard != null )
            nowPlantCard.plantSeedCard = null;
        GameObject.Destroy(this.gameObject);
    }

    private void Update()
    {
        if (isSelect && Input.GetMouseButtonDown(1))
        {
            isSelect = false;

            plant.color = Color.white;
            bg.color = Color.white;
        }

        if (isMove)
        {
            curTimer += Time.deltaTime;
            var process = curTimer / time;
            this.transform.position = Vector3.Lerp(startPos, targetPos, process);

            int sortingOrder = (int)((-transform.position.y + 10) * 10);
            plant.sortingOrder = sortingOrder + 1;
            bg.sortingOrder = sortingOrder;
        }
    }

    private void OnMouseDown()
    {
        isSelect = true;
        isMove = false;

        plant.color = Color.gray;
        bg.color = Color.gray;

        nowPlantCard = GameObject.Instantiate(this.manualPlant);
        nowPlantCard.plantType = plantType;
        nowPlantCard.plantSeedCard = this;
    }

    public void PlaceSeed()
    {
        GameObject.Destroy(this.gameObject);
    }
}
