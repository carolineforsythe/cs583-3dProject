using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityWaterSound2 : MonoBehaviour
{
    public AudioSource waterAudioSource;
    public float maxDistance = 20f;
    public float minDistance = 5f;
    public float maxVolume = 1f;

    private Transform playerTransform;
    private GameObject[] waterObjects;

    void Start()
    {
        // find the player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Monkey");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            Debug.Log("Player found: " + playerObj.name);
        }
        else
        {
            Debug.LogError("Player not found! Make sure your player has the 'Monkey' tag.");
        }

        // find all water objects
        waterObjects = GameObject.FindGameObjectsWithTag("Water");
        Debug.Log("Found " + waterObjects.Length + " water objects");

        if (waterObjects.Length == 0)
        {
            Debug.LogError("No water objects found! Make sure your water objects have the 'Water' tag.");
        }

        // setup audio source
        if (waterAudioSource == null)
        {
            waterAudioSource = GetComponent<AudioSource>();
        }

        if (waterAudioSource == null)
        {
            Debug.LogError("No AudioSource found on WaterNoise object!");
            return;
        }

        waterAudioSource.loop = true;
        waterAudioSource.spatialBlend = 0f;
        waterAudioSource.Play();
        Debug.Log("Water audio started playing");
    }

    void Update()
    {
        if (playerTransform == null || waterObjects.Length == 0)
            return;

        // find the closest water object
        float closestDistance = float.MaxValue;

        foreach (GameObject water in waterObjects)
        {
            if (water != null)
            {
                float distance = Vector3.Distance(playerTransform.position, water.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                }
            }
        }

        // calculate volume based on distance
        float volumeMultiplier = 0f;

        if (closestDistance <= minDistance)
        {
            volumeMultiplier = 1f;
        }
        else if (closestDistance >= maxDistance)
        {
            volumeMultiplier = 0f;
        }
        else
        {
            volumeMultiplier = 1f - ((closestDistance - minDistance) / (maxDistance - minDistance));
        }

        // apply volume with SFX settings
        float sfxVolume = SoundVolumeSFX.Instance != null ? SoundVolumeSFX.Instance.GetSFXVolume() : 1f;
        waterAudioSource.volume = volumeMultiplier * maxVolume * sfxVolume;

        // debug info 
        if (Time.frameCount % 60 == 0) // Log every 60 frames to avoid spam
        {
            Debug.Log($"Distance: {closestDistance:F1} | Volume Mult: {volumeMultiplier:F2} | Final Volume: {waterAudioSource.volume:F2}");
        }
    }
}
