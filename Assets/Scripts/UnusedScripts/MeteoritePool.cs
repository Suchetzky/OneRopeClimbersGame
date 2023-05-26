using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteoritePool : MonoBehaviour
{
    public GameObject meteoritePrefab;
    public int poolSize = 10;
    public float spawnInterval = 1f;
    public float spawnRange = 5f;

    private Queue<GameObject> meteoritePool = new Queue<GameObject>();
    private float lastSpawnTime = 0f;
    
    private Camera mainCamera;
    private float cameraHeight;
    private float cameraWidth;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float meteorAngleRange;
    [SerializeField] private float minScaleFactor;
    [SerializeField] private float maxScaleFactor;

    private void Start()
    {
        mainCamera = Camera.main;
        cameraHeight = mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;
        
        // Populate the meteorite pool with game objects
        for (int i = 0; i < poolSize; i++)
        {
            GameObject meteorite = Instantiate(meteoritePrefab, transform.position, Quaternion.identity);
            meteorite.SetActive(false);
            meteoritePool.Enqueue(meteorite);
        }
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime > spawnInterval && meteoritePool.Count > 0)
        {
            // Get a meteorite from the pool and spawn it
            GameObject meteorite = meteoritePool.Dequeue();
            meteorite.SetActive(true);
            meteorite.transform.position = GetRandomSpawnPosition();
            float scaleFactor = Random.Range(minScaleFactor, maxScaleFactor);
            meteorite.transform.localScale = new Vector2(1, 1) * scaleFactor;
            SetMeteoriteMotion(meteorite);
            StartCoroutine(CheckIfOutOfBounds(meteorite));

            lastSpawnTime = Time.time;
        }
    }

    private IEnumerator CheckIfOutOfBounds(GameObject meteorite)
    {
        while (!IsInCameraBounds(meteorite.transform.position))
            yield return null;
        while (IsInCameraBounds(meteorite.transform.position))
            yield return null;
        meteorite.SetActive(false);
        meteoritePool.Enqueue(meteorite);
        yield return null;
    }

    private bool IsInCameraBounds(Vector3 position)
    {
        float minX = mainCamera.transform.position.x - cameraWidth;
        float maxX = mainCamera.transform.position.x + cameraWidth;
        float minY = mainCamera.transform.position.y - cameraHeight;
        float maxY = mainCamera.transform.position.y + cameraHeight;

        return position.x > minX && position.x < maxX && position.y > minY && position.y < maxY;
    }

    private void SetMeteoriteMotion(GameObject meteorite)
    {
        float angleToMiddle = GetAngleToCameraCenter(meteorite.transform.position);
        float moveSpeed = Random.Range(minSpeed, maxSpeed);
        meteorite.GetComponent<Meteorite>().SetSpeed(moveSpeed);
        angleToMiddle += 360;
        float angle1 = angleToMiddle - (meteorAngleRange / 2) + 360;
        float angle2 = angleToMiddle + (meteorAngleRange / 2) + 360;
        float randomAngle = Random.Range(Mathf.Min(angle1, angle2),Mathf.Max(angle1, angle2)) * Mathf.Deg2Rad;
        Vector2 moveDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
        meteorite.GetComponent<Rigidbody2D>().velocity = moveDirection * moveSpeed;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-cameraWidth, cameraWidth);
        float y = Random.Range(-cameraHeight, cameraHeight);

        // make sure the spawn position is outside the camera bounds
        if (Mathf.Abs(x) < cameraWidth && Mathf.Abs(y) < cameraHeight)
        {
            if (Mathf.Abs(x) < Mathf.Abs(y))
            {
                x = x < 0 ? -cameraWidth - 1f : cameraWidth + 1f;
            }
            else
            {
                y = y < 0 ? -cameraHeight - 1f : cameraHeight + 1f;
            }
        }

        return new Vector3(x, y, 0f);
    }
    
    public static float GetAngleToCameraCenter(Vector2 point)
    {
        // Get the position of the camera in world space
        Vector3 cameraPosition = Camera.main.transform.position;
        
        // Calculate the vector from the camera position to the point
        Vector2 cameraToPointVector = new Vector2(cameraPosition.x, cameraPosition.y) - point;
        
        // Calculate the angle between the camera-to-point vector and the x-axis
        return Mathf.Atan2(cameraToPointVector.y, cameraToPointVector.x) * Mathf.Rad2Deg;
    }
}
