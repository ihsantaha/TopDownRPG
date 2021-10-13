using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyEditor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    int spriteIndex;
    public Sprite[] skySprites;


    void Start()
    {
        spriteIndex = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = skySprites[spriteIndex];
        

        // Coroutine
    }

    // Update is called once per frame
    void Update()
    {
        //InvokeRepeating("changeTimeOfDay", 1f, 1f);
    }

    void changeTimeOfDay()
    {  
        // Upgrade to use fading

        if (spriteIndex <= skySprites.Length)
        {
            spriteIndex++;
        } else
        {
            spriteIndex = 0;
        }
        spriteRenderer.sprite = skySprites[spriteIndex];
    }
}
