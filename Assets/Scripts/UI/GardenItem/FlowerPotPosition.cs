using TopDownPlate;
using UnityEngine;

public class FlowerPotPosition : MonoBehaviour
{
    [Tooltip("����Ԥ����")]
    public FlowerPotGardenItem flowerPot;
    [Tooltip("ˮ����Ԥ����")]
    public FlowerPotGardenItem waterFlowerPot;

    [Tooltip("����")]
    public GameObject earth;
    [Tooltip("�Ƿ�����������")]
    public bool HaveEarth;

    /// <summary>
    /// ��λ���ϵĻ���
    /// </summary>
    public FlowerPotGardenItem FlowerPot { get; set; }

    private PlantConent plantConent;

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

    public void CreateFlowerPot()
    {
        FlowerPot = GameObject.Instantiate(flowerPot, this.transform);
    }

    public void CreateWaterFlowerPot()
    {
        FlowerPot = GameObject.Instantiate(waterFlowerPot, this.transform);
    }
}
