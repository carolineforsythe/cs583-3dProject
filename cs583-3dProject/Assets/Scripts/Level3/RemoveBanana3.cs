using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RemoveBanana3 : MonoBehaviour
{
    public static int numBananasToCollect = 4;
    public float deleteDistance = 2f;   // how close the player needs to be
    private Transform player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Monkey").transform;
        numBananasToCollect = 4;
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
                numBananasToCollect--;

                print(numBananasToCollect);

                if (numBananasToCollect == 0)
                {
                    SceneManager.LoadScene("Level3Win");


                }
            }
        }
    }
}