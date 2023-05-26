using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomParallax : MonoBehaviour
{
    [SerializeField] private ProgressTracker progressTracker;

    [SerializeField] private float parallax;

    private int direction;

    private float travelAmount;
    private float startingHeight;

    private void Start()
    {
        direction = Math.Sign(parallax);
        travelAmount = Camera.main.orthographicSize * Mathf.Abs(parallax) + Mathf.Abs(transform.localPosition.y);
        startingHeight = transform.localPosition.y;
    }

    private void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x,
            startingHeight + travelAmount * progressTracker.percentClimbed * direction, transform.localPosition.z);
    }
}
