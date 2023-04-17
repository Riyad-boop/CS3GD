using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip walkSound;
    AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttackSound()
    {
        audioSource.clip = attackSound;
        audioSource.volume = 0.7f;
        audioSource.Play();

    }

    public void PlayDeathSound()
    {
        audioSource.clip = deathSound;
        audioSource.Play();

    }



}
