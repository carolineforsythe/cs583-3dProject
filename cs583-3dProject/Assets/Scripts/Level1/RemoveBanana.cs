using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RemoveBanana : MonoBehaviour
{
    public static int numBananasCollected = 0;
    public float deleteDistance = 2f;   // how close the player needs to be
    private Transform player;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Monkey").transform;
        numBananasCollected = 0;
    }

    void Update()
    {
        // find all bananas currently in the scene
        GameObject[] bananas = GameObject.FindGameObjectsWithTag("Banana");

        foreach (GameObject banana in bananas)
        {
            float dist = Vector3.Distance(player.position, banana.transform.position);

            if (dist <= deleteDistance)
            {
                Destroy(banana);
                numBananasCollected++;

                print(numBananasCollected);

                if(numBananasCollected == 3)
                {
                    SceneManager.LoadScene("Level1Win");


                }
            }
        }
    }
}