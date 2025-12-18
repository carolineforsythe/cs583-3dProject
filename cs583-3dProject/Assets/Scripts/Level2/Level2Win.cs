using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Win : MonoBehaviour
{
    public void replayLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
