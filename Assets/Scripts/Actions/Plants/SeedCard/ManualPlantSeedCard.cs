using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ManualPlantSeedCard : MonoBehaviour
{
    [Tooltip("在手上时一直跟着鼠标的图片")]
    public SpriteRenderer image;
    [Tooltip("播放植物动画的图片")]
    public SpriteRenderer plant;

    /// <summary>
    /// 是否还在手上
    /// </summary>
    public bool IsManual { get; protected set; } = true;

    public int plantType { get; set; }

    public PlantSeedCard plantSeedCard { get; set; }

    bool canPlace;

    private void Start()
    {
        var plantCard = ConfManager.Instance.confMgr.plantCards.GetPlantCardByType(plantType);
        Sprite plantImage = Resources.Load<Sprite>(plantCard.plantImagePath);
        image.sprite = plantImage;
        plant.sprite = plantImage;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsManual && canPlace)
        {
            PlacePlant();
        }
        // 鼠标右键取消
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1) && IsManual)
        {
            GameObject.Destroy(this.gameObject);
        }

#if UNITY_ANDROID
        if (IsManual)
        {
            bool isPlace = true;
            Vector2 touchPos = Vector2.zero;
            if (Input.touchCount > 0)
            {
                var touches = Input.touches;
                for (int i = 0; i < touches.Length; i++)
                {
                    if (GetBounds().Contains(touches[i].position))
                    {
                        isPlace = false;
                        touchPos = touches[i].position;
                        break;
                    }
                }
            }

            if (isPlace)
                PlacePlant();
            else
            {
                var targetPos = Camera.main.ScreenToWorldPoint(touchPos);
                float x = targetPos.x - targetPos.x % 0.5f;
                // 0.5 刚好站在格子上
                float y = targetPos.y - targetPos.y % 0.5f;
                this.transform.position = new Vector3(x, y, 0);
                int sortingOrder = (int)((-y + 10) * 10);
                plant.sortingOrder = sortingOrder;
                image.sortingOrder = sortingOrder;
                image.transform.position = new Vector3(targetPos.x, targetPos.y, 0);
            }
        }
#else
        if (IsManual)
        {
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = targetPos.x - targetPos.x % 0.5f;
            // 0.5 刚好站在格子上
            float y = targetPos.y - targetPos.y % 0.5f;
            this.transform.position = new Vector3(x, y, 0);
            int sortingOrder = (int)((-y + 10) * 10);
            plant.sortingOrder = sortingOrder;
            image.sortingOrder = sortingOrder;
            image.transform.position = new Vector3(targetPos.x, targetPos.y, 0);

            var levelBounds = LevelManager.Instance.LevelBounds;
            // 如果在右半部分则面向左
            this.transform.localScale = new Vector3(x <= levelBounds.center.x ? 1 : -1, 1, 1);
        }

        if (JudgePlace())
        {
            canPlace = true;
            plant.enabled = true;
        }
        else
        {
            canPlace = false;
            plant.enabled = false;
        }
#endif
}

private bool JudgePlace()
{
    var plantSeedCards = GameManager.Instance.plantSeedCard;
    foreach (var item in plantSeedCards)
    {
        if (item.transform.position.x == this.transform.position.x && item.transform.position.y == this.transform.position.y)
            return false;
    }
    return true;
}

private Rect GetBounds()
    {
        var screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        screenPos.x -= 500 / 2;
        screenPos.y -= 500 / 2;
        return new Rect(screenPos, new Vector2(500, 500));
    }


    protected virtual void PlacePlant()
    {
        AchievementManager.Instance.SetAchievementType9(plantType);
        AudioManager.Instance.PlayEffectSoundByName("PlacePlant");
        var info = GardenManager.Instance.PlantPrefabInfos.GetPlantInfo((PlantType)plantType);
        var plantCard = ConfManager.Instance.confMgr.plantCards.GetPlantCardByType(plantType);
        if (info != null)
        {
            var plant = info.plant;
            var newPlant = GameObject.Instantiate(plant);
            newPlant.plantAttribute = new PlantAttribute(plantCard);
            newPlant.plantAttribute.CultivatePlant(true);

            newPlant.transform.position = this.transform.position;
            newPlant.Reuse(false);

            var levelBounds = LevelManager.Instance.LevelBounds;
            // 如果在右半部分则面向左
            if (newPlant.transform.position.x > levelBounds.center.x)
                newPlant.FacingDirections = FacingDirections.Left;
            else
                newPlant.FacingDirections = FacingDirections.Right;
            int sortingOrder = (int)((-newPlant.transform.position.y + 10) * 10);
            var newSpriteRenderer = newPlant.GetComponent<SpriteRenderer>();
            if (newSpriteRenderer != null)
                newSpriteRenderer.sortingOrder = sortingOrder;

            // 墓碑加入 然后当前位置播放动画
            if (plantCard.plantType == PlantType.Gravebuster)
            {
                GardenManager.Instance.Gravebusters.Add(plant as Gravebuster);
                var go = Resources.Load<GameObject>("Prefabs/Plants/PlantSeedCard/Gravebuster");
                var newGo = GameObject.Instantiate(go);
                newGo.transform.position = this.transform.position;
            }
            // 三叶草只能朝右
            else if (plantCard.plantType == PlantType.Blover)
            {
                newPlant.FacingDirections = FacingDirections.Right;
            }

            GameManager.Instance.plantSeedCard.Add(newPlant);
        }
        else
        {
            void Create(string name)
            {
                var go = Resources.Load<GameObject>("Prefabs/Plants/PlantSeedCard/" + name);
                AudioManager.Instance.PlayEffectSoundByName("Eat");
                var newGo = GameObject.Instantiate(go, GameManager.Instance.Player.transform);
                newGo.transform.position = GameManager.Instance.Player.transform.position + Vector3.up;
                GameManager.Instance.Player.FindAbility<CharacterMovement>().EatFood();
                GameManager.Instance.Player.FindAbility<CharacterDash>().EatFood();
            }
            // 大蒜和咖啡豆吃了恢复满奔跑和翻滚体力
            if (plantCard.plantType == PlantType.CoffeeBean)
            {
                Create("CoffeeBean");
            }
            else if (plantCard.plantType == PlantType.Gralic)
            {
                Create("Gralic");
            }
            // 南瓜判断是否还有南瓜复活，没有则加
            else if (plantCard.plantType == PlantType.PumpkinHead)
            {
                GameManager.Instance.pumpkinHead.HasPumpkinHead = true;
                GameManager.Instance.pumpkinHead.gameObject.SetActive(true);
            }
        }

        plantSeedCard?.PlaceSeed();
        GameObject.Destroy(this.gameObject);
    }
}
