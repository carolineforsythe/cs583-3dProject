using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playLevel1(){
        SceneManager.LoadScene("Level1");
    }

    public void playLevel2(){
        SceneManager.LoadScene("Level2");
    }

    public void playLevel3()
    {
        SceneManager.LoadScene("Level3");
    }


}
