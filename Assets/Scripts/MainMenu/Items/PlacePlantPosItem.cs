using System;
using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class PlacePlantPosItem : MonoBehaviour
{
    public Transform plantRoot;

    UIEventListener uiEventListener;
    ExternalGardenPanel externalGardenPanel;
    int plantType;
    int pos;

    private void Start()
    {
        uiEventListener = GetComponent<UIEventListener>();
        uiEventListener.onMouseEnter.AddListener(MouseEnter);
        uiEventListener.onMouseExit.AddListener(MouseExit);
        uiEventListener.onPointUp.AddListener(OnClick);
        uiEventListener.onPointDown.AddListener(OnDown);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            MouseExit();
    }

    public void InitData(int pos, int plantType, ExternalGardenPanel externalGardenPanel)
    {
        this.pos = pos;
        this.plantType = plantType;
        this.externalGardenPanel = externalGardenPanel;

        if (plantType != 0)
        {
            plantRoot.DestroyChild();
            var confCardItem = ConfManager.Instance.confMgr.plantIllustrations.GetPlantItemType(plantType);

            var plantGo = Resources.Load<GameObject>(confCardItem.prefabPath);
            var newGo = GameObject.Instantiate(plantGo, plantRoot);
            newGo.GetComponent<Image>().raycastTarget = false;
        }
    }

    void MouseEnter()
    {
        if (plantType == 0 && externalGardenPanel.selectSeed != 0 && plantRoot.childCount == 0)
        {
            var confCardItem = ConfManager.Instance.confMgr.plantIllustrations.GetPlantItemType(externalGardenPanel.selectSeed);

            var plantGo = Resources.Load<GameObject>(confCardItem.prefabPath);
            var newGo = GameObject.Instantiate(plantGo, plantRoot);
            var image = newGo.GetComponent<Image>();
            image.color = new Color(0.8f, 0.8f, 0.8f, 0.7f);
            image.raycastTarget = false;
        }
        if (externalGardenPanel.isShovel && plantRoot.childCount != 0)
        {
            var plant = plantRoot.GetChild(0);
            var image = plant.GetComponent<Image>();
            image.color = new Color(0.8f, 0.8f, 0.8f, 0.7f); 
        }
    }

    void MouseExit()
    {
        if (plantType == 0)
        {
            plantRoot.DestroyChild();
        }
        if (plantRoot.childCount != 0)
        {
            var plant = plantRoot.GetChild(0);
            var image = plant.GetComponent<Image>();
            image.color = Color.white;
        }
    }

    void OnDown()
    {
        externalGardenPanel.isItemDown = true;
    }

    void OnClick()
    {
        externalGardenPanel.isItemDown = false;
        // 铲掉
        if (externalGardenPanel.isShovel && plantType != 0)
        {
            externalGardenPanel.ShovelPlant(pos);
            plantType = 0;
            plantRoot.DestroyChild();
            AudioManager.Instance.PlayEffectSoundByName("shovel");
        }
        // 种植
        else if (plantType == 0 && externalGardenPanel.selectSeed != 0)
        {
            plantRoot.DestroyChild();
            plantType = externalGardenPanel.selectSeed;
            externalGardenPanel.PlacePlantSeed(pos);
            var confCardItem = ConfManager.Instance.confMgr.plantIllustrations.GetPlantItemType(plantType);

            var plantGo = Resources.Load<GameObject>(confCardItem.prefabPath);
            var newGo = GameObject.Instantiate(plantGo, plantRoot);
            newGo.GetComponent<Image>().raycastTarget = false;
            AudioManager.Instance.PlayEffectSoundByName("PlacePlant");
        }
    }
}
