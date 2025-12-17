using System.Collections;
using System.Collections.Generic;using UnityEngine;

public class BirdHit : MonoBehaviour
{
    public string monkeyTag = "Monkey";
    public string loseSceneName = "Level3Lose";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(monkeyTag)) return;

        PlayerController3 player = other.GetComponent<PlayerController3>();
        if (player != null)
        {
            player.LoseLifeAndRespawn();
        }

        Destroy(gameObject); 
    }
}

