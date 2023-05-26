using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerJumpScript : MonoBehaviour
{
    public static event Action OnBothPlayersDead;
    
    private const int DiffY = 5;
    public GameObject playerOne;
    public GameObject playerTwo;

    private Rigidbody2D playerOneRigid;
    private Rigidbody2D playerTwoRigid;

    [SerializeField] ProceduralPlayerController playerOneController;
    [SerializeField] ProceduralPlayerController playerTwoController;

    private bool playerOneJump = false;
    private bool playerTwoJump = false;

    private float minGravity = 0;
    [SerializeField] float maxGravity = 10;

    private float minDrag = 0;
    private float maxDrag = 2;

    private bool oneInMountain = true;
    private bool twoInMountain = true;

    [SerializeField] private GameObject rightGoal;
    [SerializeField] private GameObject leftGoal;

    private bool isBothPlayersDead = false;
    private bool didWin = false;

    [SerializeField] private AudioSource playerOneJumpSound;
    [SerializeField] private AudioSource playerTwoJumpSound;
    void Start() 
    {
        MountainTopController.PlayersHitTop += OnPlayersWin;
        OnBothPlayersDead += BothPlayersFall;
        
        playerOneRigid = playerOne.GetComponent<Rigidbody2D>();
        playerTwoRigid = playerTwo.GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        playersInMountain();

        if (playerOneJump && playerTwoJump)
        {
            if (OnBothPlayersDead != null && !isBothPlayersDead && !didWin)
            {
                isBothPlayersDead = true;
                OnBothPlayersDead();
            }
        }
        else
        {
            //FirstPlayer Jump
            if (DidPlayer1Jump() && !playerTwoJump)
            {
                // Jump -> No Jump
                if (playerOneJump && oneInMountain)
                {
                    PlayerOneJumpToNoJump();
                }
                // No Jump -> Jump
                else
                {
                    playerOneJumpSound.Play();
                    PlayerOneNoJumpToJump();
                }
            }

            //player 2 jump or connect
            if (DidPlayer2Jump() && !playerOneJump)
            {
                // Jump -> No Jump
                if (playerTwoJump && twoInMountain)
                {
                    PlayerTwoJumpToNoJump();
                }
                // No Jump -> Jump
                else
                {
                    playerTwoJumpSound.Play();
                    PlayerTwoNoJumpToJump();
                }
            }
        }
    }

    private void OnDestroy()
    {
        MountainTopController.PlayersHitTop -= OnPlayersWin;
        OnBothPlayersDead -= BothPlayersFall;
    }

    public void PlayerOneJumpToNoJump()
    {
        playerOneJump = false;
        playerTwoRigid.bodyType = RigidbodyType2D.Dynamic;
        playerOneRigid.gravityScale = minGravity;
        playerOneRigid.velocity = Vector2.zero;
        playerOneRigid.drag = maxDrag;
        playerOneRigid.drag = maxDrag;

        playerOneRigid.GetComponentInParent<ProceduralPlayerController>().SetScribbleActive(false);
        
    }

    public void PlayerOneNoJumpToJump()
    {
        playerOneJump = true;
        playerTwoRigid.bodyType = RigidbodyType2D.Static;
        playerOneRigid.gravityScale = maxGravity;
        playerOneRigid.drag = minDrag;
        playerOneRigid.drag = minDrag;
        
        playerOneRigid.GetComponentInParent<ProceduralPlayerController>().SetScribbleActive(true);
    }

    public void PlayerTwoJumpToNoJump()
    {
        playerTwoJump = false;
        playerOneRigid.bodyType = RigidbodyType2D.Dynamic;
        playerTwoRigid.gravityScale = minGravity;
        playerTwoRigid.velocity = Vector2.zero;
        playerTwoRigid.drag = maxDrag;
        playerTwoRigid.angularDrag = maxDrag;
        
        playerTwoRigid.GetComponentInParent<ProceduralPlayerController>().SetScribbleActive(false);
    }

    public void PlayerTwoNoJumpToJump()
    {
        playerTwoJump = true;
        playerOneRigid.bodyType = RigidbodyType2D.Static;
        playerTwoRigid.gravityScale = maxGravity;
        playerTwoRigid.drag = minDrag;
        playerTwoRigid.angularDrag = minDrag;
        
        playerTwoRigid.GetComponentInParent<ProceduralPlayerController>().SetScribbleActive(true);
    }

    public bool PlayerOneJumpStateGetter()
    {
        return playerOneJump;
    }
    
    public bool PlayerTwoJumpStateGetter()
    {
        return playerTwoJump;
    }

    // checks both sides connection with mountain. make sure player inside mountain.
    void playersInMountain()
    {

        if ((Physics2D.Raycast(new Vector2(playerOne.transform.position.x,playerOne.transform.position.y+DiffY),
                new Vector2(rightGoal.transform.position.x,rightGoal.transform.position.y)).collider !=null)
            && Physics2D.Raycast(new Vector2(playerOne.transform.position.x,playerOne.transform.position.y+DiffY),
                new Vector2(leftGoal.transform.position.x,leftGoal.transform.position.y)).collider !=null)
        {
            oneInMountain = true;
        }
        else
        {
            oneInMountain = false;
        }
        
        if ((Physics2D.Raycast(new Vector2(playerTwo.transform.position.x,playerTwo.transform.position.y+DiffY),
                new Vector2(rightGoal.transform.position.x,rightGoal.transform.position.y)).collider !=null)
            && Physics2D.Raycast(new Vector2(playerTwo.transform.position.x,playerTwo.transform.position.y+DiffY),
                new Vector2(leftGoal.transform.position.x,leftGoal.transform.position.y)).collider !=null)
        {
            twoInMountain = true;
        }
        else
        {
            twoInMountain = false;
        }
    }

    private bool DidPlayer1Jump()
    {
        Gamepad gamePad = null;
        if (Gamepad.all.Count > 0)
        {
            gamePad = Gamepad.all[0];
        }

        return DidPlayerJump(KeyCode.G, gamePad);
    }

    private bool DidPlayer2Jump()
    {
        Gamepad gamePad = null;
        if (Gamepad.all.Count > 1)
        {
            gamePad = Gamepad.all[1];
        }

        return DidPlayerJump(KeyCode.L, gamePad);
    }

    private bool DidPlayerJump(KeyCode keyCode, Gamepad gamePad)
    {
        bool didKeyboardJump = Input.GetKeyDown(keyCode);

        bool didGamepadJump = false;
        if (gamePad != null)
        {
            didGamepadJump = gamePad.rightTrigger.wasReleasedThisFrame;
        }

        return didKeyboardJump || didGamepadJump;
    }

    private void OnPlayersWin()
    {
        didWin = true;
    }

    private void BothPlayersFall()
    {
        playerOneRigid.bodyType = RigidbodyType2D.Dynamic;
        playerTwoRigid.bodyType = RigidbodyType2D.Dynamic;

        playerOneRigid.gravityScale = maxGravity;
        playerTwoRigid.gravityScale = maxGravity;
    }
}