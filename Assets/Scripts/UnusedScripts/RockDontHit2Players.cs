using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDontHit2Players : MonoBehaviour
{
    private PolygonCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player1") || col.transform.CompareTag("Player2"))
        {
            collider.enabled = false;
        }
    }
}
