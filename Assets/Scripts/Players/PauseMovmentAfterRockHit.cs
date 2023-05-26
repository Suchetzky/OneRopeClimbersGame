using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMovmentAfterRockHit : MonoBehaviour
{
    [SerializeField] private int SecondsToStopFalling = 1;
    public PlayerJumpScript playerJump;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    // private void OnCollisionEnter2D(Collision2D col)
    // {
    //     
    //     if (col.transform.CompareTag("Rock"))
    //     {
    //         
    //         StartCoroutine(StopMovment());
    //     }
    // }
    //
    // private IEnumerator StopMovment()
    // {
    //     yield return new WaitForSeconds(SecondsToStopFalling);
    //     gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //     
    // }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.transform.CompareTag("Rock"))
        {
            
            if (gameObject.transform.CompareTag("Player1"))
            {
                playerJump.PlayerOneNoJumpToJump();
            }
            if (gameObject.transform.CompareTag("Player2"))
            {
                playerJump.PlayerTwoNoJumpToJump();
            }
        }
    }
    
}
