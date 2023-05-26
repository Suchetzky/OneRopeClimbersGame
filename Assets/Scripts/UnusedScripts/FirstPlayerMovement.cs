using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerMovement : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    public float frameRightBoundery;
    public float frameLeftBoundery;
    public float frameUpBoundery;
    public float frameDownBoundery;

    // Position
    private float curX;
    private float curY;

    // Movement
    private bool rotateRight = false;
    private bool rotateLeft = false;
    private bool up = false;
    private bool down = false;
    public float forceSize;

    private float hAxis;
    private float vAxis;

    private int speedMax = 5;
    public float rotateScale;
    private float curRotate = 0;

    private bool playerIdentity = false; // true 1, false 2.

    [SerializeField]
    private ClimbingInputManager inputManager;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        if (myRigidbody.CompareTag("Astronaut1"))
        {
            playerIdentity = true;
        }
    }

    void Update()
    {
        // reset speed.
        if (!CheckBoundaries())
        {
            myRigidbody.velocity = Vector2.zero;
        }

        // get press keys

        if (playerIdentity)
        {
            hAxis = inputManager.player1Horizontal;
            vAxis = inputManager.player1Vertical;
            rotateLeft = Input.GetKey("left");
            rotateRight = Input.GetKey("right");
            up = Input.GetKey("up");
            down = Input.GetKey("down");
        }
        else
        {
            hAxis = inputManager.player2Horizontal;
            vAxis = inputManager.player2Vertical;
            rotateLeft = Input.GetKey("a");
            rotateRight = Input.GetKey("d");
            up = Input.GetKey("w");
            down = Input.GetKey("s");
        }
    }

    private void FixedUpdate()
    {
        // Move with arrows.
        if (rotateLeft || hAxis > 0)
        {
            transform.Rotate(Vector3.back, -1 * rotateScale * Time.deltaTime);
        }

        if (rotateRight || hAxis < 0)
        {
            transform.Rotate(Vector3.back, rotateScale * Time.deltaTime);
        }

        if (up || vAxis > 0)
        {
            myRigidbody.AddRelativeForce(new Vector2(forceSize * -1, 0));
        }
        
        if (down || vAxis < 0)
        {
            myRigidbody.AddRelativeForce(new Vector2(forceSize, 0));
        }
        
    }

    // check if the object reach frame boundaries.
    bool CheckBoundaries()
    {
        curX = myRigidbody.transform.position.x;
        if (curX < frameLeftBoundery || curX > frameRightBoundery)
        {
            return false;
        }
        return true;
    }
}