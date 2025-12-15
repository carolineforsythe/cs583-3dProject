using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCountImages3 : MonoBehaviour
{
    public Image[] heartImages; // array of 3 hearts
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

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
        for (int i = 0; i < heartImages.Length; i++)
        {
            // hearts fill from left to right
            if (PlayerController3.numLivesLeft > i)
            {
                heartImages[i].sprite = fullHeartSprite;
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }
}

