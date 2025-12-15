using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityBananaSound3 : MonoBehaviour
{
    public AudioSource bananaAudioSource;
    public float maxDistance = 20f;
    public float minDistance = 5f;
    public float maxVolume = 1f;

    private Transform playerTransform;
    private GameObject[] bananaObjects;

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

        // find all banana objects
        bananaObjects = GameObject.FindGameObjectsWithTag("Banana");
        Debug.Log("Found " + bananaObjects.Length + " water objects");

        if (bananaObjects.Length == 0)
        {
            Debug.LogError("No water objects found! Make sure your water objects have the 'Water' tag.");
        }

        // setup audio source
        if (bananaAudioSource == null)
        {
            bananaAudioSource = GetComponent<AudioSource>();
        }

        if (bananaAudioSource == null)
        {
            Debug.LogError("No AudioSource found on WaterNoise object!");
            return;
        }

        bananaAudioSource.loop = true;
        bananaAudioSource.spatialBlend = 0f;
        bananaAudioSource.Play();
        Debug.Log("Water audio started playing");
    }

    void Update()
    {
        if (playerTransform == null || bananaObjects.Length == 0)
            return;

        // find the closest banana object
        float closestDistance = float.MaxValue;

        foreach (GameObject water in bananaObjects)
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
        bananaAudioSource.volume = volumeMultiplier * maxVolume * sfxVolume;

        // debug
        if (Time.frameCount % 60 == 0) // Log every 60 frames to avoid spam
        {
            Debug.Log($"Distance: {closestDistance:F1} | Volume Mult: {volumeMultiplier:F2} | Final Volume: {bananaAudioSource.volume:F2}");
        }
    }
}

