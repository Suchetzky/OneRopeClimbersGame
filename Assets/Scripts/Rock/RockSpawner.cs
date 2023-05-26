using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockSpawner : MonoBehaviour
{
    
    public event System.Action OnRockSpawn;
    [SerializeField] private RockPool rockPool;
    [SerializeField] Transform player1;
    [SerializeField] Transform player2;
    
    private Camera mainCamera;
    private float cameraHeight;
    private float cameraWidth;
    
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 5f;
    
    private float curSpawnInterval;
    private float lastSpawnTime = 0f;
    private bool _timeIsup;

    private void Start()
    {
        mainCamera = Camera.main;
        cameraHeight = mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;
        
        curSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        
        Timer.OnTimerEnd.AddListener(OnTimerEnd);
    }

    private void Update()
    {
        if (_timeIsup || Time.time - lastSpawnTime > curSpawnInterval)
        {
            // Get a meteorite from the pool and spawn it
            GameObject rock = rockPool.GetRockFromPool();
            if (!rock)
                return;
            rock.transform.position = GetRandomSpawnPosition();
            rock.SetActive(true);

            if (OnRockSpawn != null)
                OnRockSpawn();
            
            StartCoroutine(CheckIfOutOfBounds(rock));

            lastSpawnTime = Time.time;
            curSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float min = Mathf.Min(player1.position.x, player2.position.x);
        float max = Mathf.Max(player1.position.x, player2.position.x);

        float x = Random.Range(min, max);

        // Tal change, the 10 is the diff from button cam to upper cam.
        Vector2 topCenterScreenPosition = new Vector2(Screen.width / 2f, Screen.height);

        //Camera camera = Camera.main;
        Vector3 topCenterWorldPosition = 
            mainCamera.ScreenToWorldPoint(new Vector3(topCenterScreenPosition.x, topCenterScreenPosition.y, mainCamera.nearClipPlane));

        // float y = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).y + 10 + 1;

        return new Vector3(x, topCenterWorldPosition.y + 1, 0f);
    }
    
    private IEnumerator CheckIfOutOfBounds(GameObject rock)
    {
        while (IsInCameraBounds(rock.transform.position))
            yield return null;
        rockPool.ReturnRockToPool(rock);
        yield return null;
    }

    private bool IsInCameraBounds(Vector3 position)
    {
        float minY = mainCamera.transform.position.y - cameraHeight - 1;
        float maxY = mainCamera.transform.position.y + cameraHeight + 1;

        return position.y > minY;
    }

    private void OnTimerEnd()
    {
        _timeIsup = true;
    }

    private void OnDestroy()
    {
        Timer.OnTimerEnd.RemoveListener(OnTimerEnd);
    }
}
