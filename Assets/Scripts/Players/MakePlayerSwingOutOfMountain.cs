using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePlayerSwingOutOfMountain : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    [SerializeField] private PlayerJumpScript playerJump;

    private int DefultLayer = 0;
    private int playerSwing = 10;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (playerJump.PlayerOneJumpStateGetter())
        {
            SetLayerRecursively(player1,playerSwing);
        }
        else
        {
            SetLayerRecursively(player1,DefultLayer);
        }
        
        if (playerJump.PlayerTwoJumpStateGetter())
        {
            SetLayerRecursively(player2,playerSwing);
        }
        else
        {
            SetLayerRecursively(player2,DefultLayer);
        }
    }


    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
    
    
}