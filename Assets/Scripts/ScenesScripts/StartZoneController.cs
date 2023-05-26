using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZoneController : MonoBehaviour
{
    public LevelLoader levelLoader;
    public Color activeColor;
    
    private int playersInside = 0;
    private float delay = 1f;
    private bool didGameStart = false;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playersInside == 0)
        {
            Color newColor = activeColor;
            newColor.a = 0.3f;
            spriteRenderer.color = newColor;
        }
        
        if (playersInside >= 1)
        {
            spriteRenderer.color = activeColor;
        }
        
        if (playersInside == 2 && !didGameStart)
        {
            didGameStart = true;
            levelLoader.LoadNextLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playersInside += 1;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playersInside -= 1;
    }
}
