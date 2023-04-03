using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[Serializable]
public class PlantAttribute
{
    public PlantCard plantCard;
    public bool isCultivate;

    // Ϊ�˷����ԣ������ɵ�ʱ��˳��ȡֵ������ÿ��ֲ�����ӵ����Զ�����ͬ��һ��ȡ��������������������������������,����valueΪ�ȼ����ȼ�������������������ɾ���ֲ�������
    public int value1;
    public int value2;
    public int value3;
    public int maxLevel = 10;

    public PlantAttribute(PlantCard plantCard)
    {
        this.plantCard = plantCard;
    }
}

public class FlowerPotGardenItem : MonoBehaviour
{
    public PlantAttribute PlantAttribute = null;

    private PlantCultivationPage plantCultivationPage;
    private bool isPageOpen;
    private GameObject seeding;
    private GameObject taegetPlantPrefab;
    private GameObject taegetPlant;

    private Animator animator;

    private Camera UICamera;
    private RectTransform rectTransform;
    private RectTransform plantPageRectTransform;

    private void Start()
    {
        UICamera = UIManager.Instance.UICamera;
        rectTransform = this.GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isPageOpen && plantCultivationPage != null && Input.GetMouseButtonDown(0) 
            && !BoundsUtils.GetAnchorLeftRect(UICamera, plantPageRectTransform).Contains(Input.mousePosition) 
            && !BoundsUtils.GetSceneRect(UICamera, rectTransform).Contains(Input.mousePosition) && plantCultivationPage.gameObject.activeSelf)
        {
            plantCultivationPage.gameObject.SetActive(false);
            AudioManager.Instance.PlayEffectSoundByName("pageExpansion", UnityEngine.Random.Range(0.8f, 1f));
            isPageOpen = false;
        }
    }

    public void SetPlant(GameObject taegetPlant, PlantCard plantCard, PlantCultivationPage plantCultivationPage)
    {
        plantPageRectTransform = plantCultivationPage.GetComponent<RectTransform>();
        this.plantCultivationPage = plantCultivationPage;
        this.taegetPlantPrefab = taegetPlant;
        this.PlantAttribute = new PlantAttribute(plantCard);
        seeding = GameObject.Instantiate(GardenManager.Instance.SeedingPrefab, this.transform);
        GardenManager.Instance.PlantAttributes.Add(this.PlantAttribute);
    }

    private void OnMouseDown()
    {
        if (!isPageOpen && plantCultivationPage != null)
        {
            isPageOpen = true;
            AudioManager.Instance.PlayEffectSoundByName("pageExpansion", UnityEngine.Random.Range(1f, 1.2f));
            plantCultivationPage.gameObject.SetActive(true);
            plantCultivationPage.SetPlantAttribute(this);
            plantCultivationPage.transform.position = this.transform.position;
            Rect bounds = BoundsUtils.GetAnchorLeftRect(UICamera, plantPageRectTransform);
            if (bounds.xMax > Screen.width)
            {
                var pos = plantCultivationPage.transform.position;
                // 100 Ϊcanvasÿ��λ����
                pos.x = plantCultivationPage.transform.position.x - bounds.width / 100;
                plantCultivationPage.transform.position = pos;
            }
        }
    }

    public void CultivatePlant()
    {
        if (!PlantAttribute.isCultivate)
        {
            PlantAttribute.isCultivate = true;
            GameObject.Destroy(seeding);
            taegetPlant = GameObject.Instantiate(taegetPlantPrefab, this.transform);
            plantCultivationPage.SetPlantAttribute(this);
            UpdateSunPrice();
        }
    }

    public void UpdateSunPrice()
    {
        plantCultivationPage.UpdateSunPrice();
    }
}
