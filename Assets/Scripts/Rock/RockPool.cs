using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPool : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas;
    public GameObject rockPrefab;
    public int poolSize = 10;


    private Queue<GameObject> rockPool = new Queue<GameObject>();


    private void Start()
    {
        /*mainCamera = Camera.main;
        cameraHeight = mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;

        curSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);*/
        
        // Populate the meteorite pool with game objects
        for (int i = 0; i < poolSize; i++)
        {
            GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
            rock.transform.parent = transform;
            rock.SetActive(false);
            rock.GetComponent<Rock>().targetCanvas = targetCanvas;
            rockPool.Enqueue(rock);
        }
    }
    
    private void Update()
    {
        

    }

    public GameObject GetRockFromPool()
    {
        if (rockPool.Count <= 0)
            return null;
        return rockPool.Dequeue();
    }

    public void ReturnRockToPool(GameObject rock)
    {
        rock.SetActive(false);
        rockPool.Enqueue(rock);
    }
    

    



}
