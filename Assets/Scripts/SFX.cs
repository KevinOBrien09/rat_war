using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] AudioSource source;

    public void Play(AudioClip clip_, float volume_ = .75f,float pitch_ = 1f)
    {
        source.volume = volume_;
        source.pitch = pitch_;
        source.clip = clip_;
        source.Play();
        Destroy(gameObject,clip_.length + clip_.length/4);
    }
}
