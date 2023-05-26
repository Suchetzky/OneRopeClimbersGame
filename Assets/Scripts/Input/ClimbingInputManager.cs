using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbingInputManager : MonoBehaviour
{
    // this change is for dafna!

    public event Action OnPlayer1Jump;

    public float player1Horizontal;
    public float player1Vertical;

    public float player2Horizontal;
    public float player2Vertical;

    public bool isGamepadsEnabled;

    private PlayerInput playerInput;

    private int gamepadCount;

    private const int PLAYER_SWING = 10;
    [SerializeField] private GameObject player1Bone;
    private int player1Layer;
    [SerializeField] private GameObject player2Bone;
    private int player2Layer;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        gamepadCount = Gamepad.all.Count;
        if (gamepadCount <= 0 && isGamepadsEnabled)
        {
            isGamepadsEnabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        Vector2? player1Movement = GetPlayer1Movement();

        player1Layer = player1Bone.layer;
        player2Layer = player2Bone.layer;
        
        if (player1Movement.HasValue && player2Layer != PLAYER_SWING)
        {
            Vector2 value = player1Movement.Value;
            player1Horizontal = value.x;
            player1Vertical = value.y;
        }
        else
        {
            player1Horizontal = 0;
            player1Vertical = 0;
        }

        Vector2? player2Movement = GetPlayer2Movement();

        if (player2Movement.HasValue && player1Layer != PLAYER_SWING)
        {
            Vector2 value = player2Movement.Value;
            player2Horizontal = value.x;
            player2Vertical = value.y;
        }
        else
        {
            player2Horizontal = 0;
            player2Vertical = 0;
        }
    }

    private Vector2 GetPlayer2Movement()
    {
        Gamepad gamepad = null;
        if (Gamepad.all.Count > 1)
        {
            gamepad = Gamepad.all[1];
        }

        return GetPlayerMovement("ArrowsMove", gamepad);
    }

    private Vector2 GetPlayer1Movement()
    {
        Gamepad gamepad = null;
        if (Gamepad.all.Count > 0)
        {
            gamepad = Gamepad.all[0];
        }

        return GetPlayerMovement("Move", gamepad);
    }
    
    private Vector2 GetPlayerMovement(string set, Gamepad gamepad)
    {
        Vector2 keyboardMovement = playerInput.actions[set].ReadValue<Vector2>();

        Vector2 stickMovement = Vector2.zero;
        Vector2 dpadMovement = Vector2.zero;
        if (gamepad != null)
        {
            stickMovement = gamepad.leftStick.ReadValue();
            dpadMovement = gamepad.dpad.ReadValue();
        }

        float gamepadMovementH = stickMovement.x + dpadMovement.x;
        float hMovement = keyboardMovement.x + gamepadMovementH;

        float gamepadMovementV = stickMovement.y + dpadMovement.y;
        float vMovement = keyboardMovement.y + gamepadMovementV;

        return new Vector2(hMovement, vMovement);
    }

    private void HandleJump()
    {
        
    }
}
