using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpWithCamera : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, cam.transform.position.y-2, transform.position.z);
    }
}
