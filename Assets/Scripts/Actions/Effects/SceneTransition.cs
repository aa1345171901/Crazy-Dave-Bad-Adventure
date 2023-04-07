using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SceneTransition : MonoBehaviour
{
    [Tooltip("ȫ�ֹ�Դ")]
    public Light2D GlobalLight;

    [Tooltip("����ȫ�ֹ�Դǿ��")]
    public float DayTimeLightIntensity = 1;

    [Tooltip("ҹ����")]
    public List<SpriteRenderer> nightBg;

    [Tooltip("ҹ��ƹ�")]
    public List<Light2D> nightLights;

    [Tooltip("���챳��")]
    public List<SpriteRenderer> daytimeBg;

    private float defaultGlobalIntensity;

    public readonly float TransitionTime = 1f;   // ����ʱ��
    private readonly float GradientInterval = 0.1f;  // ������

    public void TransitionToDaytime()
    {
        defaultGlobalIntensity = GlobalLight.intensity;
        StartCoroutine(TransitionDayTime());
    }

    IEnumerator TransitionDayTime()
    {
        float len = TransitionTime / GradientInterval;
        foreach (var item in daytimeBg)
        {
            item.gameObject.SetActive(true);
        }
        for (int i = 1; i <= len; i++)
        {
            GlobalLight.intensity = defaultGlobalIntensity + i * (DayTimeLightIntensity - defaultGlobalIntensity) / len;
            foreach (var item in nightLights)
            {
                item.intensity = 1 - i * 1 / len;
            }

            foreach (var item in nightBg)
            {
                if (i == len)
                    item.gameObject.SetActive(false);
                else
                    item.color = new Color(item.color.a, item.color.g, item.color.b, 1 - i * 1 / len);
            }
            yield return new WaitForSeconds(GradientInterval);
        }
    }

    public void TransitionToNight()
    {
        StartCoroutine(TransitionNight());
    }

    IEnumerator TransitionNight()
    {
        float len = TransitionTime / GradientInterval;
        for (int i = 1; i <= len; i++)
        {
            GlobalLight.intensity = DayTimeLightIntensity - i * (DayTimeLightIntensity - defaultGlobalIntensity) / len;
            foreach (var item in nightLights)
            {
                item.intensity = i * 1 / len;
            }

            foreach (var item in nightBg)
            {
                if (i == 1)
                    item.gameObject.SetActive(true);
                else
                    item.color = new Color(item.color.a, item.color.g, item.color.b, i * 1 / len);
            }
            yield return new WaitForSeconds(GradientInterval);
        }

        foreach (var item in daytimeBg)
        {
            item.gameObject.SetActive(false);
        }
    }
}
