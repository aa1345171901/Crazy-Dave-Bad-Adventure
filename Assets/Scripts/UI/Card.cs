using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image Bg;
    public Image Plant;
    public Text Sun;
    public Button button;
    public RectTransform coolMask;

    public PlantAttribute PlantAttribute;
    public float coolTimer; // 冷却时间

    private bool isGarden;
    private int sun;
    private float lastTimer;  // 上次种植时间

    private readonly int LevelSunReduced = -25;

    private void Start()
    {
        GardenManager.Instance.SunChanged += () =>
        {
            if (!isGarden)
            {
                if (sun > GardenManager.Instance.Sun)
                {
                    this.Sun.color = Color.red;
                    this.button.enabled = false;
                }
                else
                {
                    this.Sun.color = Color.black;
                    this.button.enabled = true;
                }
                if (sun < 0)
                {
                    this.Sun.text = "+" + -sun;
                    this.Sun.color = Color.green;
                }
            }
        };
    }

    public void SetPlant(PlantAttribute plantAttribute, bool isGarden)
    {
        this.PlantAttribute = plantAttribute;

        Sprite bg = Resources.Load<Sprite>(plantAttribute.plantCard.plantBgImagePath);
        this.Bg.sprite = bg;

        Sprite plantImage = Resources.Load<Sprite>(plantAttribute.plantCard.plantImagePath);
        this.Plant.sprite = plantImage;

        this.PlantAttribute.SunChanged += UpdateSun;
        UpdateSun();

        this.isGarden = isGarden;

        Bg.enabled = true;
        Plant.enabled = true;
        Sun.enabled = true;
        button.enabled = true;
    }

    public void UnSetPlant()
    {
        this.PlantAttribute.SunChanged -= UpdateSun;
        Bg.enabled = false;
        Plant.enabled = false;
        Sun.enabled = false;
        button.enabled = false;
    }

    private void UpdateSun()
    {
        this.Sun.text = PlantAttribute.plantCard.defaultSun.ToString();
        sun = PlantAttribute.plantCard.defaultSun;
        this.Sun.color = Color.black;
        switch (PlantAttribute.plantCard.plantType)
        {
            case PlantType.CherryBomb:
            case PlantType.Jalapeno:
            case PlantType.DoomShroom:
                ReducedSun(7);
                break;
            case PlantType.IceShroom:
                ReducedSun(2);
                break;
            default:
                break;
        }
    }

    private void ReducedSun(int attribute)
    {
        for (int i = 0; i < PlantAttribute.attribute.Length; i++)
        {
            // 字段映射
            var fieldInfo = typeof(PlantAttribute).GetField("level" + (i + 1));
            if (PlantAttribute.attribute[i] == attribute)
            {
                sun = (int)fieldInfo.GetValue(PlantAttribute) * LevelSunReduced + PlantAttribute.plantCard.defaultSun;
                this.Sun.text = sun.ToString();
                if (sun < 0)
                {
                    this.Sun.text = "+" + -sun;
                    this.Sun.color = Color.green;
                }
                break;
            }
        }
    }

    public void OnClick()
    {
        if (!isGarden && Time.time - lastTimer > coolTimer)
        {
            var plantPrefab = GardenManager.Instance.PlantPrefabInfos.GetPlantInfo(PlantAttribute.plantCard.plantType).plant;
            if (plantPrefab != null)
            {
                var plant = GameObject.Instantiate(plantPrefab);
                plant.plantAttribute = PlantAttribute;
                var plantManual = plant.GetComponent<ManualPlant>();
                if (plantManual)
                    plantManual.InitPlant(this, sun);
            }
        }
    }

    private void Update()
    {
        if (Time.time - lastTimer < coolTimer)
        {
            float process = 1 - (Time.time - lastTimer) / coolTimer;
            coolMask.localScale = new Vector3(1, process, 1);
        }
    }

    public void PlacePlant()
    {
        lastTimer = Time.time;
    }
}
