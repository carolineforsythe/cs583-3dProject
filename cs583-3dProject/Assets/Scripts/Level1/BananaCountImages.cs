using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BananaCountImages : MonoBehaviour
{
    public Image[] bananaImages; // array of 3 hearts
    public Sprite bananaSprite;

    void Start()
    {
        UpdateHearts();
    }

    void Update()
    {
        // update hearts to reflect current lives
        UpdateHearts();
    }

    void UpdateHearts()
    {
        //loop through all hearts and update based on lives remaining
        for (int i = 0; i < bananaImages.Length; i++)
        {
            // hearts fill from left to right
            if (i < RemoveBanana.numBananasToCollect)
            {
                bananaImages[i].sprite = bananaSprite;
                bananaImages[i].enabled = true;

            }
            else
            { 
                bananaImages[i].enabled = false;
            }
        }
    }
}
