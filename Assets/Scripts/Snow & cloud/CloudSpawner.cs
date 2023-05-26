using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private float minSpawnInterval = 5f;
    [SerializeField] private float maxSpawnInterval = 10f;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private void SpawnCloud()
    {
        Vector3 spawnPos = CalculateSpawnPosition();

        Instantiate(cloudPrefab, spawnPos, Quaternion.identity, transform);
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);

            SpawnCloud();
        }
    }

    private Vector3 CalculateSpawnPosition()
    {
        float vPos = Random.Range(.5f, 1f);
        Vector3 viewportPosition = new Vector3(1, vPos, mainCamera.nearClipPlane);
        Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(viewportPosition);
        spawnPosition.z = -1f;

        // Adjust the spawn position to take the sprite size into account
        SpriteRenderer spriteRenderer = cloudPrefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spawnPosition.x += spriteRenderer.bounds.extents.x;
        }
        else
        {
            Debug.LogWarning("[ERROR]: could'nt find sprite renderer, sprite size won't be taken into account");
        }

        return spawnPosition;
    }
}
