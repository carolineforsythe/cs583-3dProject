using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3Win : MonoBehaviour
{
   public void replayLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
