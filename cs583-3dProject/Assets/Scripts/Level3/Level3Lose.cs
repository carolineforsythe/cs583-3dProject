using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3Lose : MonoBehaviour
{
    public void replayLevel3()
    {
        SceneManager.LoadScene("Level3"); // go back to start of level 33
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // go to main menu
    }
}
