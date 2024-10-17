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
    [Tooltip("�Ϲ�ǰ")]
    public SpriteRenderer PumpkinHeadBefore;
    [Tooltip("�ϹϺ�")]
    public SpriteRenderer PumpkinHeadBack;

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
        Fire.sortingOrder = layer - 2;
        PumpkinHeadBack.sortingOrder = layer - 1;
        Lights.sortingOrder = layer;
        PumpkinHeadBefore.sortingOrder = layer + 1;
        Mic.sortingOrder = layer + 2;
        Fogmachine.sortingOrder = layer - 1;
        Speaker.sortingOrder = layer + 3;
        foreach (var item in LightItems)
        {
            item.sortingOrder = layer + 3;
        }
        fog.GetComponent<ParticleSystemRenderer>().sortingOrder = layer + 4;
    }
}
