using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level1Lose : MonoBehaviour
{
    public void replayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
