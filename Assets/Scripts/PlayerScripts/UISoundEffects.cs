using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundEffects : MonoBehaviour
{
    [SerializeField] public AudioClip click;

    public void PlayClick()
    {
        AudioManager.instance.PlayTrack(click, startingVolume: 1, channel: 1, loop: false);
    }
}
