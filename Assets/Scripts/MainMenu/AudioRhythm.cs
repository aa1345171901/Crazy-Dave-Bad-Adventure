using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;
using UnityEngine.Events;

public class AudioRhythm : MonoBehaviour
{
    public Vector3 offset;

    private Vector3 targetPos;
    private Vector3 origin;
    private AudioSource musicAudioSource;

    private float[] spectrum = new float[64];

    private void Start()
    {
        origin = this.transform.position;
    }

    private void Update()
    {
        if (musicAudioSource == null)
        {
            musicAudioSource = AudioManager.Instance.BackmusicPlayer;
            return;
        }
        if (musicAudioSource.volume > 0)
        {
            musicAudioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
            float maxSpectrum = GetMaxByArray.GetMaxSpectrum(spectrum) / musicAudioSource.volume;
            if (maxSpectrum > 0.05f)
            {
                targetPos = origin + maxSpectrum * offset;
            }
            if (targetPos != Vector3.zero)
            {
                transform.Translate((targetPos - transform.position) * 6 * Time.deltaTime);
                if ((targetPos - transform.position).magnitude < 0.1f)
                {
                    targetPos = Vector3.zero;
                }
            }
            else
            {
                transform.Translate((origin - transform.position) * 3 * Time.deltaTime);
            }
        }
    }
}
