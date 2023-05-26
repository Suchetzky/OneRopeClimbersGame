using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SnowOpacity : MonoBehaviour
{
    [SerializeField] private float minOpacity;
    [SerializeField] private float maxOpacity;

    private void OnEnable()
    {
        Color newColor = new Color(Color.white.r, Color.white.g, Color.white.b, Random.Range(minOpacity, maxOpacity));
        GetComponent<SpriteRenderer>().color = newColor;
    }
}
