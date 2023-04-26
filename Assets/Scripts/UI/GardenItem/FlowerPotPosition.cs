using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlowerPotPosition : MonoBehaviour
{
    [Tooltip("花盆预制体")]
    public FlowerPotGardenItem flowerPot;
    [Tooltip("水花盆预制体")]
    public FlowerPotGardenItem waterFlowerPot;

    [Tooltip("泥土")]
    public GameObject earth;
    [Tooltip("是否有泥土覆盖")]
    public bool HaveEarth;

    /// <summary>
    /// 该位置上的花盆
    /// </summary>
    public FlowerPotGardenItem FlowerPot { get; set; }

    private PlantConent plantConent;
    private bool isShowPrice;
    private bool isMoving;
    private bool canPlace;
    private FlowerPotGardenItem TempItem;
    private FlowerPotGardenItem TempItemTarget;

    private bool isWaterPot;

    private void Start()
    {
        if (HaveEarth)
            earth = this.transform.GetChild(0).gameObject;
        plantConent = this.GetComponentInParent<PlantConent>();
        if (GardenManager.Instance.earth.Contains(this.gameObject.name))
        {
            earth.SetActive(false);
            HaveEarth = false;
        }
    }

    private void OnMouseDown()
    {
        if (GardenManager.Instance.IsShoveling)
            ShovelAwayEarth();
        if (GardenManager.Instance.IsSelling)
            SellPlant();
        if (GardenManager.Instance.IsMoving)
            Invoke("MovePlant", 0.1f);
    }

    private void OnMouseEnter()
    {
        if (GardenManager.Instance.IsSelling)
            isShowPrice = true;
    }

    private void OnMouseExit()
    {
        isShowPrice = false;
    }

    public void ShovelAwayEarth()
    {
        if (HaveEarth)
        {
            earth.SetActive(false);
            HaveEarth = false;
            GardenManager.Instance.MaxFlowerPotCount++;
            GardenManager.Instance.earth.Add(this.gameObject.name);
            plantConent.AddLayUpPos(this);
            GardenManager.Instance.IsShoveling = false;
            AudioManager.Instance.PlayEffectSoundByName("shovel");
            ShopManager.Instance.RemovePurchasePropByName("shovel");
        }
    }

    public void SellPlant()
    {
        if (FlowerPot == null)
            return;
        FlowerPot.Sell();
        if (FlowerPot.PlantAttribute == null || FlowerPot.PlantAttribute.plantCard.plantType == PlantType.None)
            return;
        plantConent.RemoveFlowerPot(this, isWaterPot);
        if (isWaterPot)
            GardenManager.Instance.WaterFlowerPotCount--;
        else
            GardenManager.Instance.FlowerPotCount--;
        Destroy(FlowerPot);
        FlowerPot = null;
        AudioManager.Instance.PlayEffectSoundByName("PlacePlant");
    }

    private void OnGUI()
    {
        if (isShowPrice && FlowerPot != null)
        {
            // gui坐标左下为0，0 右上为 screen.width, height
            var guiPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

            GUI.skin.label.fontSize = 25;
            GUI.skin.label.font = GameManager.Instance.HUDFont;
            GUI.Label(new Rect(guiPos.x, guiPos.y, Screen.width, Screen.height), "$" + FlowerPot.GetPrice());
        }
    }

    public void MovePlant()
    {
        if (FlowerPot == null)
            return;
        isMoving = true;
        TempItem = GameObject.Instantiate(FlowerPot, this.transform);
        TempItemTarget = GameObject.Instantiate(FlowerPot, this.transform);
        TempItemTarget.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        this.FlowerPot.GetComponent<Image>().color = Color.gray;
        if (this.FlowerPot.targetPlant != null)
        {
            this.FlowerPot.targetPlant.GetComponent<Image>().color = Color.gray;
            TempItemTarget.targetPlant.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
        TempItemTarget.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isMoving)
        {
            var worldPos = UIManager.Instance.UICamera.ScreenToWorldPoint(Input.mousePosition);
            TempItem.transform.position = new Vector3(worldPos.x, worldPos.y, TempItem.transform.position.z);
            var target = GetMouseFlowerPot();
            if (target)
            {
                if (!target.HaveEarth && target.FlowerPot == null)
                {
                    TempItemTarget.gameObject.SetActive(true);
                    TempItemTarget.transform.position = target.transform.position;
                    canPlace = true;
                }
                else
                {
                    TempItemTarget.gameObject.SetActive(false);
                    canPlace = false;
                }
            }
            else
            {
                TempItemTarget.gameObject.SetActive(false);
                canPlace = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                DestroyTemp();
                if (canPlace)
                {
                    var tempPos = this.transform.position;
                    this.transform.position = target.transform.position;
                    target.transform.position = tempPos;
                    AudioManager.Instance.PlayEffectSoundByName("MoveSuccess");
                }
            }

            if (Input.GetMouseButton(1))
                DestroyTemp();
        }
    }

    private void DestroyTemp()
    {
        GameObject.Destroy(TempItem.gameObject);
        GameObject.Destroy(TempItemTarget.gameObject);
        AudioManager.Instance.PlayEffectSoundByName("BtnGarden");
        this.FlowerPot.GetComponent<Image>().color = Color.white;
        if (this.FlowerPot.targetPlant != null)
            this.FlowerPot.targetPlant.GetComponent<Image>().color = Color.white;
        isMoving = false;
    }


    public FlowerPotPosition GetMouseFlowerPot()
    {
        GraphicRaycaster _raycaster = FindObjectOfType<GraphicRaycaster>();
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(eventData, results);
        FlowerPotPosition flowerPotPosition = null;
        foreach (var item in results)
        {
            var flowerPot = item.gameObject.GetComponent<FlowerPotPosition>();
            if (FlowerPot)
            {
                flowerPotPosition = flowerPot;
                break;
            }
        }
        return flowerPotPosition;
    }

    public void CreateFlowerPot()
    {
        FlowerPot = GameObject.Instantiate(flowerPot, this.transform);
        isWaterPot = false;
    }

    public void CreateWaterFlowerPot()
    {
        FlowerPot = GameObject.Instantiate(waterFlowerPot, this.transform);
        isWaterPot = true;
    }
}
