using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayAnim : MonoBehaviour
{
    public void Play(string name)
    {
        var anim = GetComponent<Animator>();
        anim.Play(name, 0, 0);
    }

    public void Play1(string name)
    {
        var anim = GetComponent<Animator>();
        anim.Play(name, 1, 0);
    }
}
