using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    public AudioClip attackSound;
    public AudioClip roarSound;
    public AudioClip deathSound;
    public AudioClip hordeSound;
    public AudioClip moanSound;
    AudioSource audioSource;


    private void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }
    public void PlayAttackSound()
    {
        audioSource.clip = attackSound;
        audioSource.volume = 0.7f;
        audioSource.Play();

    }

    public void PlayDeathSound()
    {
        StartCoroutine(DeathSounds());

    }

    public IEnumerator DeathSounds()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
        yield return new WaitForSeconds(1);
        PlayHordeSound();

    }

    public void PlayRoarSound()
    {
        audioSource.clip = roarSound;
        audioSource.volume = 0.5f;
        audioSource.Play();

    }

    public void PlayHordeSound()
    {
        audioSource.clip = roarSound;
        audioSource.volume = 0.6f;
        audioSource.Play();

    }
    public void PlayMoanSound()
    {
        audioSource.clip = moanSound;
        audioSource.Play();
    }
}
