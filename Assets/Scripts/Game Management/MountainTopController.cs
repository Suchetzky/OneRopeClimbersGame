using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MountainTopController : MonoBehaviour
{
    public static event Action PlayersHitTop;
    
    private int playersOnTop = 0;
    private bool didPlayersReachTop = false;

    private void Update()
    {
        if (!didPlayersReachTop && playersOnTop >= 2)
        {
            didPlayersReachTop = true;
            PlayersHitTop?.Invoke();
        }

        if (didPlayersReachTop)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var controller = other.GetComponentInParent<ProceduralPlayerController>();
        if (controller != null)
        {
            playersOnTop += 1;
            Debug.Log("[TEST]: player entered top!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var controller = other.GetComponentInParent<ProceduralPlayerController>();
        if (controller != null)
        {
            playersOnTop -= 1;
            Debug.Log("[TEST]: player left top!");
        }
    }
}
