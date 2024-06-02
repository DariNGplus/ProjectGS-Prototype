using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioSource SFXSource;

    public AudioClip BGMusic;

    public AudioClip PlayerDeath;
    public AudioClip PlayerHit;
    public AudioClip PlayerShoot;
    public AudioClip PlayerDash;

    public AudioClip EnemyHit;
    public AudioClip EnemyDie;

    private float _slowmoPitchFactor = 0.5f;

    void Start()
    {
        MusicSource.clip = BGMusic;
        MusicSource.Play();
    }

    public void ToggleSlowmoPitch(bool state) 
    {
        if (state)
        {
            MusicSource.pitch = _slowmoPitchFactor;
            SFXSource.pitch = _slowmoPitchFactor;
        }
        else 
        {
            MusicSource.pitch = 1;
            SFXSource.pitch = 1;
        }
    }

    void Update()
    {
        
    }
}
