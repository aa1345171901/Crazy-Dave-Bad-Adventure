using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class GravebusterSeed : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject Grave;

    private void Start()
    {
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
        audioSource.Play();
        int sortingOrder = (int)((-transform.position.y + 10) * 10);
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = sortingOrder;
        Grave.transform.localPosition = new Vector3(0, -0.844f, 0);
    }

    private void Update()
    {
        this.transform.Translate(Vector3.down * 0.6f * Time.deltaTime);
        Grave.transform.Translate(Vector3.up * 0.6f * Time.deltaTime);
    }
}
