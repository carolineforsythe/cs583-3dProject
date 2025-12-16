using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BananaCountImages3 : MonoBehaviour
{
    public Image[] bananaImages; // array of 4 bananas
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
            if (i < RemoveBanana3.numBananasToCollect)
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

