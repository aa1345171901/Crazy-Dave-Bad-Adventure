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

    public void CreateWaterFlowerPot()
    {
        FlowerPot = GameObject.Instantiate(waterFlowerPot, this.transform);
    }
}
