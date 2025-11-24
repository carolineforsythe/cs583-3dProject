using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SoundVolumeMusic : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;

    void Start()
    {
        // load saved values (default to 1 = max volume)
        float masterValue = PlayerPrefs.GetFloat("MasterExposed", 1f);

        masterSlider.value = masterValue;

        SetMasterVolume(masterValue);
    }

    public void SetMasterVolume(float sliderValue)
    {
        // convert slider value (0 to 1) to decibel range (-80dB to 0dB)
        float dB = Mathf.Lerp(-80f, 0f, sliderValue);
        audioMixer.SetFloat("MasterExposed", dB);
        PlayerPrefs.SetFloat("MasterExposed", sliderValue);
    }

}
