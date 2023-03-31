using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class AudioEffect : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip impact;
    public AudioClip throwOut;
    public AudioClip potReturn;

    public void ImpactAudioPlay(bool isDreariment)
    {
        if (isDreariment)
            audioSource.pitch = Random.Range(0.8f, 1);
        else
            audioSource.pitch = Random.Range(1, 1.4f);
        audioSource.PlayOneShot(impact);
        // Ä¬ÈÏimpact´ó¸Å0.3f s
        PotReturnPlay();
    }

    public void ThrowOutPlay()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(throwOut);
    }

    public void PotReturnPlay()
    {
        audioSource.clip = potReturn;
        audioSource.pitch = 2;
        audioSource.Play();
    }

    private void Start()
    {
        AudioManager.Instance.AudioLists.Add(audioSource);
        audioSource.volume = AudioManager.Instance.EffectPlayer.volume;
    }
}
