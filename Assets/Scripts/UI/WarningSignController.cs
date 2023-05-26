using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningSignController : MonoBehaviour
{
    public Canvas canvas;
    public Transform targetTranform;

    private RectTransform rectTransform;
    private Image image;

    public float flashDuration = 0.1f;
    public int flashCount = 3;
    
    private RectTransform canvasRectTransform;
    private Camera mainCamera;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        canvasRectTransform = canvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPoint = mainCamera.WorldToScreenPoint(targetTranform.position);
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            screenPoint, 
            canvas.worldCamera,
            out Vector2 localPoint);

        float topMargin = 60;
        float canvasHeight = canvasRectTransform.rect.height;
        float adjustedY = canvasHeight / 2 - topMargin;

        rectTransform.localPosition = new Vector3(localPoint.x, adjustedY, 0f);
    }

    public void StartFlashing()
    {
        StartCoroutine(FlashSprite());
    }

    IEnumerator FlashSprite()
    {
        Color originalColor = image.color;

        for (int i = 0; i < flashCount; i++)
        {
            image.color = Color.clear;
            yield return new WaitForSeconds(flashDuration / 2f);
            image.color = originalColor;
            yield return new WaitForSeconds(flashDuration / 2f);
        }
        
        Destroy(gameObject);
    }
}
