using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip[] walkSound;
    AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttackSound()
    {
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(attackSound);

    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);

    }

    public void PlayWalkSound()
    {
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(walkSound[Random.Range(0, walkSound.Length)]);
       
    }



}
