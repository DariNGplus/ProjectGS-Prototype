using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingAfterImage : MonoBehaviour
{
    public ParticleSystem _deathParticles;
    private Player _player;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = GameObject.Find("GameManager").GetComponentInChildren<AudioManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void ToggleInvuln() 
    {
        if (!_player.IsDead)
        _player.IsInvul = false;
    }

    public void TriggerDeathParticles() 
    {
        _deathParticles.Play();
        _audioManager.SFXSource.PlayOneShot(_audioManager.PlayerDeath, 1);
    }

    public void DestroyUponFade() 
    {
        Destroy(gameObject);
    }
}
