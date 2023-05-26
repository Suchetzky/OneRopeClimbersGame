using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowSpawner : MonoBehaviour
{
public event System.Action OnRockSpawn;

    [SerializeField] private float minScaleFactor;
    [SerializeField] private float maxScaleFactor;
    
    
    private Camera mainCamera;
    private float cameraHeight;
    private float cameraWidth;
    
    public GameObject rockPrefab;
    public int poolSize = 10;
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 5f;

    private Queue<GameObject> rockPool = new Queue<GameObject>();
    private float lastSpawnTime = 0f;
    private float curSpawnInterval;
    private void Start()
    {
        mainCamera = Camera.main;
        cameraHeight = mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;

        curSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        
        // Populate the meteorite pool with game objects
        for (int i = 0; i < poolSize; i++)
        {
            GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
            rock.SetActive(false);
            rockPool.Enqueue(rock);
        }
    }
    
    private void Update()
    {
        
        if (Time.time - lastSpawnTime > curSpawnInterval && rockPool.Count > 0)
        {
            // Get a meteorite from the pool and spawn it
            GameObject rock = rockPool.Dequeue();
            rock.transform.position = GetRandomSpawnPosition();
            rock.SetActive(true);

            /*if (OnRockSpawn != null)
                OnRockSpawn();*/

            float scaleFactor = Random.Range(minScaleFactor, maxScaleFactor);
            rock.transform.localScale = Vector2.one * scaleFactor;
            //SetMeteoriteMotion(rock);
            StartCoroutine(CheckIfOutOfBounds(rock));

            lastSpawnTime = Time.time;
            curSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        float min = mainCamera.transform.position.x - cameraWidth / 2f;
        float max = mainCamera.transform.position.x + cameraWidth / 2f;

        float x = Random.Range(min, max);

        // Tal change, the 10 is the diff from button cam to upper cam.
        Vector2 topCenterScreenPosition = new Vector2(Screen.width / 2f, Screen.height);

        Camera camera = Camera.main;
        Vector3 topCenterWorldPosition = 
            camera.ScreenToWorldPoint(new Vector3(topCenterScreenPosition.x, topCenterScreenPosition.y, camera.nearClipPlane));

        // float y = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).y + 10 + 1;

        return new Vector3(x, topCenterWorldPosition.y + 1, 0f);
    }
    
    private IEnumerator CheckIfOutOfBounds(GameObject meteorite)
    {
        while (IsInCameraBounds(meteorite.transform.position))
            yield return null;
        meteorite.SetActive(false);
        rockPool.Enqueue(meteorite);
        yield return null;
    }

    private bool IsInCameraBounds(Vector3 position)
    {
        float minY = mainCamera.transform.position.y - cameraHeight - 1;
        float maxY = mainCamera.transform.position.y + cameraHeight + 1;

        return position.y > minY;
    }
}
