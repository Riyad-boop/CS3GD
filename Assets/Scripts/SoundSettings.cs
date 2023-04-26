using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField]
    private Slider soundSlider;
    [SerializeField]
    private AudioMixer masterMixer;

    // Start is called before the first frame update
    void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));

    }

    public void SetVolume(float volume)
    {
        //prevent setting volume to 0 
        if(volume < 1)
        {
            volume = 0.001f;
        }

        SetSlider(volume);
        //save volume into playerprefs
        PlayerPrefs.SetFloat("SavedMasterVolume", volume);

        //set the volume of audio mixer
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume / 100) * 20f);


    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    public void SetSlider(float volume)
    {
        soundSlider.value = volume;
    }
}
