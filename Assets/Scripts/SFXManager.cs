using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] GameObject SFXGO;
    public bool musicPlaying;
    [SerializeField] AudioSource Music;
    [SerializeField] AudioClip Song;
    [SerializeField]  AudioSource Fight;
    public bool inFight;
    public void Awake()
    {
        if(instance == null)
        {instance = this;}
        else
        {Destroy(gameObject);}
        
    }
    
    public void Play(AudioClip clip, float volume = .75f,float pitch = 1f)
    {
        GameObject sfx = Instantiate(SFXGO);
        sfx.GetComponent<SFX>().Play(clip,volume,pitch);
    }

    public void UnitFight()
    {
        // if(!inFight)
        // {
        //     inFight = true;
            
        //     Fight.pitch = Random.Range(1.75f,2f);
        //     Fight.DOFade(.23f,.1f);
        // }
       

    }

    public void UnFadeFight()
    {
        // inFight = false;
        // Fight.DOFade(0,0f);
    }

    public void PlayMusic()
    {
       
        if(!musicPlaying)
        {
            Music.clip = Song;
            Music.volume = 0.09f;
            Music.Play();
            musicPlaying = true;
        }
       

    }
}
