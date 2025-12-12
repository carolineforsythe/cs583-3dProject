using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundVolumeSFX : MonoBehaviour
{

    // be able to access from different scenes
    public static SoundVolumeSFX Instance; 

    public AudioMixer audioMixer;
    public Slider sfxSlider;

    private float currentSFXValue = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // load saved values (default to 1 = max volume)
        float sfxValue = PlayerPrefs.GetFloat("SFXExposed", 1f);
        currentSFXValue = sfxValue;

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxValue;
        }

        SetSFXVolume(sfxValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        currentSFXValue = sliderValue;
        float dB = Mathf.Lerp(-60f, 60f, sliderValue);
        audioMixer.SetFloat("SFXExposed", dB);
        PlayerPrefs.SetFloat("SFXExposed", sliderValue);
    }

    // get current SFX volume (0 to 1)
    public float GetSFXVolume()
    {
        return currentSFXValue;
    }
}



