using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioItem : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
        AudioManager.Instance.AudioLists.Add(audioSource);
    }

    private void OnEnable()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
