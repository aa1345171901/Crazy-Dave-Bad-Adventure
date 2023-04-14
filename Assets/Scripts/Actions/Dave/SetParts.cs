using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class SetParts : MonoBehaviour
{
    [Tooltip("死后脑位置")]
    public Transform BrainPos;
    [Tooltip("火焰")]
    public SpriteRenderer Fire;
    [Tooltip("雾机")]
    public SpriteRenderer Fogmachine;
    [Tooltip("麦克风")]
    public SpriteRenderer Mic;
    [Tooltip("灯光机")]
    public SpriteRenderer Lights;
    [Tooltip("音响")]
    public SpriteRenderer Speaker;
    [Tooltip("南瓜前")]
    public SpriteRenderer PumpkinHeadBefore;
    [Tooltip("南瓜后")]
    public SpriteRenderer PumpkinHeadBack;

    [Tooltip("光线")]
    public List<SpriteRenderer> LightItems;
    [Tooltip("雾")]
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
