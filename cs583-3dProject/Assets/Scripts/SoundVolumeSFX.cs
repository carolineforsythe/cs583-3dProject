using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundVolumeSFX : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sfxSlider;

    void Start()
    {
        // load saved values (default to 1 = max volume)
        float sfxValue = PlayerPrefs.GetFloat("SFXExposed", 1f);

        sfxSlider.value = sfxValue;

        SetSFXVolume(sfxValue);
    }


    public void SetSFXVolume(float sliderValue)
    {
        float dB = Mathf.Lerp(-80f, 0f, sliderValue);
        audioMixer.SetFloat("SFXExposed", dB);
        PlayerPrefs.SetFloat("SFXExposed", sliderValue);
    }

}
