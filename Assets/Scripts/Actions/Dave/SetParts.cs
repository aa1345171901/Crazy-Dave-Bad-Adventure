using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class SetParts : MonoBehaviour
{
    [Tooltip("������λ��")]
    public Transform BrainPos;
    [Tooltip("����")]
    public SpriteRenderer Fire;
    [Tooltip("���")]
    public SpriteRenderer Fogmachine;
    [Tooltip("��˷�")]
    public SpriteRenderer Mic;
    [Tooltip("�ƹ��")]
    public SpriteRenderer Lights;
    [Tooltip("����")]
    public SpriteRenderer Speaker;

    [Tooltip("����")]
    public List<SpriteRenderer> LightItems;
    [Tooltip("��")]
    public ParticleSystem fog;

    private void Start()
    {
        GameManager.Instance.BrainPos = BrainPos;
        GameManager.Instance.Player.FaceTurn += (bool left)=>
        {
            fog.transform.rotation = Quaternion.Euler(0, left ? 180 : 0, 0);
        };
    }

    public void SetLayer(int layer)
    {
        Fire.sortingOrder = layer - 1;
        Lights.sortingOrder = layer;
        Mic.sortingOrder = layer + 1;
        Fogmachine.sortingOrder = layer - 1;
        Speaker.sortingOrder = layer + 2;
        foreach (var item in LightItems)
        {
            item.sortingOrder = layer + 2;
        }
        fog.GetComponent<ParticleSystemRenderer>().sortingOrder = layer + 3;
    }
}
