using UnityEngine;

public class FlowerPotPosition : MonoBehaviour
{
    [Tooltip("花盆预制体")]
    public FlowerPotGardenItem flowerPot;

    [Tooltip("泥土")]
    public GameObject earth;
    [Tooltip("是否有泥土覆盖")]
    public bool HaveEarth;

    /// <summary>
    /// 该位置上的花盆
    /// </summary>
    public FlowerPotGardenItem FlowerPot { get; set; }

    private void Start()
    {
        if (HaveEarth)
            earth = this.transform.GetChild(0).gameObject;
    }

    public void ShovelAwayEarth()
    {
        if (HaveEarth)
        {
            earth.SetActive(false);
            HaveEarth = false;
            GardenManager.Instance.MaxFlowerPotCount++;
        }
    }

    public void CreateFlowerPot()
    {
        FlowerPot = GameObject.Instantiate(flowerPot, this.transform);
    }
}
