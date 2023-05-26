using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] private List<Sprite> cloudSprites;

    private SpriteRenderer renderer;

    private float speed = 3f;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = cloudSprites[Random.Range(0, cloudSprites.Count)];

        if (mainCamera == null)
            mainCamera = Camera.main;

        speed = Random.Range(1f, 3f);
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!IsSpriteVisibleInView(spriteRenderer: renderer, camera: mainCamera))
        {
            Destroy(gameObject);
            return;
        }

        Vector3 delta = -transform.right.normalized * speed * Time.deltaTime;
        transform.position += delta;
    }

    private bool IsSpriteVisibleInView(SpriteRenderer spriteRenderer, Camera camera)
    {
        Bounds spriteBounds = spriteRenderer.bounds;
        Vector3[] spriteCorners = new Vector3[4]
        {
            spriteBounds.min,
            new Vector3(spriteBounds.min.x, spriteBounds.max.y),
            new Vector3(spriteBounds.max.x, spriteBounds.min.y),
            spriteBounds.max
        };

        foreach (Vector3 corner in spriteCorners)
        {
            Vector3 viewportPos = camera.WorldToViewportPoint(corner);
            if (viewportPos.x >= -0.5f && viewportPos.x <= 1.5f &&
                viewportPos.y >= -0.5f && viewportPos.y <= 1.5f)
            {
                return true;
            }
        }

        return false;
    }
}
