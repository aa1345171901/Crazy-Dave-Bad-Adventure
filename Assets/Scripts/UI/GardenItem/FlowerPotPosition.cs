using TopDownPlate;
using UnityEngine;

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

    private void Start()
    {
        if (HaveEarth)
            earth = this.transform.GetChild(0).gameObject;
        plantConent = this.GetComponentInParent<PlantConent>();
        if (GardenManager.Instance.earth.Contains(this.gameObject.name))
        {
            earth.SetActive(false);
            plantConent.AddLayUpPos(this);
            HaveEarth = false;
        }
    }

    private void OnMouseDown()
    {
        if (GardenManager.Instance.IsShoveling)
            ShovelAwayEarth();
        if (GardenManager.Instance.IsSelling)
            SellPlant();
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
        FlowerPot.Sell();
        plantConent.RemoveFlowerPot(this);
        GardenManager.Instance.FlowerPotCount--;
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

    public void CreateFlowerPot()
    {
        FlowerPot = GameObject.Instantiate(flowerPot, this.transform);
    }

    public void CreateWaterFlowerPot()
    {
        FlowerPot = GameObject.Instantiate(waterFlowerPot, this.transform);
    }
}
