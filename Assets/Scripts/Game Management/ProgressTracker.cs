using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    [SerializeField]
    private Transform mountain;
    private SpriteRenderer mountainSpriteRenderer;

    [SerializeField] private Transform bottomMountailSegment;
    [SerializeField] private Transform topMountailSegment;
    
    [SerializeField]
    private Transform player1;

    [SerializeField]
    private Transform player2;
    
    public float percentClimbed { get; private set; }

    private float mountainStart;
    private float mountainHeight;

    private void Start()
    {
        mountainSpriteRenderer = mountain.GetComponent<SpriteRenderer>();

        mountainStart = bottomMountailSegment.position.y -
                                bottomMountailSegment.GetComponent<SpriteRenderer>().bounds.extents.y;
        float mountainTopY = topMountailSegment.position.y +
                               topMountailSegment.GetComponent<SpriteRenderer>().bounds.extents.y;

        mountainHeight = mountainTopY - mountainStart;
    }

    private void Update()
    {
        //float mountainHeight = mountainSpriteRenderer.size.y * mountain.localScale.y*2.6f;
        float player1Y = player1.position.y;
        float player2Y = player2.position.y;
        float lowestY = Mathf.Min(player1Y, player2Y);
        float playersHeight = lowestY - mountainStart;
        
        percentClimbed = lowestY / mountainHeight;
    }
}
