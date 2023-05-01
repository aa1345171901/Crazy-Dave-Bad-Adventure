using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.UI;

public class GardenPropPage : MonoBehaviour
{
    public GameObject Shovel;
    public GameObject Sell;
    public GameObject Move;

    public Text ShovelText;
    private int shovelCount; 

    public List<Collider2D> collider2Ds;

    private Vector3 shovelStartPos;
    private Vector3 sellStartPos;
    private Vector3 moveStartPos;

    private bool isInit;
    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        foreach (var item in collider2Ds)
        {
            item.enabled = false;
        }
        boxCollider2D = GetComponent<BoxCollider2D>();
        Invoke("Init", 0.1f);
    }

    private void Init()
    {
        shovelStartPos = Shovel.transform.position;
        sellStartPos = Sell.transform.position;
        moveStartPos = Move.transform.position;
        isInit = true;
    }

    public void ShovelClick()
    {
        if (shovelCount <= 0)
        {
            AudioManager.Instance.PlayEffectSoundByName("NoSun");
            return;
        }
        if (GardenManager.Instance.IsShoveling)
        {
            AudioManager.Instance.PlayEffectSoundByName("BtnGarden");
            GardenManager.Instance.IsShoveling = false;
        }
        else
        {
            AudioManager.Instance.PlayEffectSoundByName("shovel");
            GardenManager.Instance.IsShoveling = true;
        }
    }

    public void SellClick()
    {
        AudioManager.Instance.PlayEffectSoundByName("BtnGarden");
        if (GardenManager.Instance.IsSelling)
            GardenManager.Instance.IsSelling = false;
        else
            GardenManager.Instance.IsSelling = true;
    }

    public void MoveClick()
    {
        AudioManager.Instance.PlayEffectSoundByName("BtnGarden");
        if (GardenManager.Instance.IsMoving)
            GardenManager.Instance.IsMoving = false;
        else
            GardenManager.Instance.IsMoving = true;
    }

    private void Update()
    {
        if (!isInit)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            GardenManager.Instance.IsSelling = GardenManager.Instance.IsMoving = GardenManager.Instance.IsShoveling = false;
            AudioManager.Instance.PlayEffectSoundByName("BtnGarden");
        }

        SetPos(Shovel, GardenManager.Instance.IsShoveling, shovelStartPos);
        SetPos(Sell, GardenManager.Instance.IsSelling, sellStartPos);
        SetPos(Move, GardenManager.Instance.IsMoving, moveStartPos);

        if (GardenManager.Instance.IsSelling || GardenManager.Instance.IsMoving || GardenManager.Instance.IsShoveling)
        {
            foreach (var item in collider2Ds)
            {
                item.enabled = true;
            }
        }
        else
        {
            foreach (var item in collider2Ds)
            {
                item.enabled = false;
            }
            shovelCount = ShopManager.Instance.PurchasePropCount("shovel");
            ShovelText.text = shovelCount.ToString();
        }

        if (!GardenManager.Instance.IsSelling && !GardenManager.Instance.IsMoving && !GardenManager.Instance.IsShoveling)
        {
            boxCollider2D.enabled = false;
        }
        else
        {
            boxCollider2D.enabled = true;
        }
    }

    private void SetPos(GameObject prop, bool ing, Vector3 startPos)
    {
        if (ing)
        {
            var worldPos = UIManager.Instance.UICamera.ScreenToWorldPoint(Input.mousePosition);
            prop.transform.position = new Vector3(worldPos.x, worldPos.y, prop.transform.position.z);
        }
        else
        {
            prop.transform.position = startPos;
        }
    }
}
