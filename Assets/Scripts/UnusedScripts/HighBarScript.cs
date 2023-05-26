using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighBarScript : MonoBehaviour
{

    public float summit;
    public float bottom;
    public float barUp;
    public float barDown;

    private float defX;
    private float newY;
    private float defZ;
    
    // Start is called before the first frame update
    void Start()
    {
        defX = transform.position.x;
        newY = transform.position.y;
        defZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        barDown = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).y + 1;
        barUp = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).y + 9;
        newY = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).y + 5;
        newY = ((newY - bottom) / (summit - bottom)) * (barUp - barDown) + barDown; 
        if (newY < barUp)
        {
            transform.position = new Vector3(defX,newY,defZ);
        }
        
    }
}
