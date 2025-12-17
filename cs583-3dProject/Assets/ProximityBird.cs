using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ProximityBird : MonoBehaviour
{
    public AudioSource birdAudioSource;

    public float maxDistance = 30f;
    public float minDistance = 8f;
    public float maxVolume = 1f;

    private Transform playerTransform;

    void Start()
    {
       
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

    
        if (birdAudioSource == null)
        {
            birdAudioSource = GetComponent<AudioSource>();
        }

        if (birdAudioSource == null)
        {
            Debug.LogError("No AudioSource found on BirdAmbienceManager object!");
            return;
        }

        birdAudioSource.loop = true;
        birdAudioSource.spatialBlend = 0f; 
        birdAudioSource.Play();
        Debug.Log("Bird ambience audio started playing");
    }

    void Update()
    {
        if (playerTransform == null) return;

        // find closest bird from the active list
        float closestDistance = float.MaxValue;

        for (int i = BirdProximitySound.ActiveBirds.Count - 1; i >= 0; i--)
        {
            Transform bird = BirdProximitySound.ActiveBirds[i];
            if (bird == null)
            {
                BirdProximitySound.ActiveBirds.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(playerTransform.position, bird.position);
            if (distance < closestDistance)
                closestDistance = distance;
        }

        // if no birds exist, mute
        if (closestDistance == float.MaxValue)
        {
            birdAudioSource.volume = 0f;
            return;
        }

        // calculate volume multiplier based on distance
        float volumeMultiplier;

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
        birdAudioSource.volume = volumeMultiplier * maxVolume * sfxVolume;

        // debug every ~60 frames
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"Bird Distance: {closestDistance:F1} | Volume Mult: {volumeMultiplier:F2} | Final Volume: {birdAudioSource.volume:F2}");
        }
    }
}

