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

    public PlantAttribute(PlantCard plantCard)
    {
        this.plantCard = plantCard;
    }
}

public class FlowerPotGardenItem : MonoBehaviour
{
    public PlantAttribute PlantAttribute = null;

    private PlantCultivationPage plantCultivationPage;
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
        plantPageRectTransform = plantCultivationPage.GetComponent<RectTransform>();
        rectTransform = this.GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !BoundsUtils.GetAnchorLeftRect(UICamera, plantPageRectTransform).Contains(Input.mousePosition) 
            && !BoundsUtils.GetSceneRect(UICamera, rectTransform).Contains(Input.mousePosition) && plantCultivationPage.gameObject.activeSelf)
        {
            plantCultivationPage.gameObject.SetActive(false);
            AudioManager.Instance.PlayEffectSoundByName("pageExpansion", UnityEngine.Random.Range(0.8f, 1.2f));
        }
    }

    public void SetPlant(GameObject taegetPlant, PlantCard plantCard, PlantCultivationPage plantCultivationPage)
    {
        this.plantCultivationPage = plantCultivationPage;
        this.taegetPlantPrefab = taegetPlant;
        this.PlantAttribute = new PlantAttribute(plantCard);
        seeding = GameObject.Instantiate(GardenManager.Instance.SeedingPrefab, this.transform);
        GardenManager.Instance.PlantAttributes.Add(this.PlantAttribute);
    }

    private void OnMouseDown()
    {
        if (PlantAttribute != null)
        {
            AudioManager.Instance.PlayEffectSoundByName("pageExpansion", UnityEngine.Random.Range(0.8f, 1.2f));
            plantCultivationPage.gameObject.SetActive(true);
            plantCultivationPage.SetPlantAttribute(this);
            plantCultivationPage.transform.position = this.transform.position;
            Rect bounds = BoundsUtils.GetAnchorLeftRect(UICamera, plantPageRectTransform);
            if (bounds.xMax > Screen.width)
            {
                plantCultivationPage.transform.position = new Vector3(Screen.width - bounds.width, bounds.y);
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
