using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    public AudioClip attackSound;
    public AudioClip roarSound;
    public AudioClip deathSound;
    public AudioClip moanSound;
    public AudioClip[] growlSound;
    public AudioClip[] walkSound;
    AudioSource audioSource;


    private void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }
    public void PlayAttackSound()
    {
        audioSource.volume = 0.25f;
        audioSource.PlayOneShot(attackSound);

    }

    public void PlayDeathSound()
    {
        StartCoroutine(DeathSounds());

    }

    public IEnumerator DeathSounds()
    {
        audioSource.PlayOneShot(deathSound);
        yield return new WaitForSeconds(1);
        PlayMoanSound();


    }

    public void PlayRoarSound()
    {
        audioSource.volume = 0.45f;
        audioSource.PlayOneShot(roarSound);

    }

    public void PlayMoanSound()
    {
        audioSource.PlayOneShot(moanSound);
    }

    public void PlayWalkSound()
    {
        audioSource.volume = 0.15f;
        audioSource.PlayOneShot(walkSound[Random.Range(0, walkSound.Length)]);
       
    }

    public void PlayGrowlSound()
    {
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(growlSound[Random.Range(0, growlSound.Length)]);

    }
}
