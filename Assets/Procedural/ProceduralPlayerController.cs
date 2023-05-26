using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ProceduralPlayerController : MonoBehaviour
{
    [SerializeField] private Transform centerBone;
    [SerializeField] private ClimbingInputManager inputManager;

    [SerializeField] private int keySet = 0;

    [SerializeField] private float regularSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float jumpDuration = .2f;

    [HideInInspector] public bool shouldAnimateLimbs = true;

    [SerializeField] private GameObject scribbleObject;
    [SerializeField] private AudioSource climbingSound;

    private float speed = 3f;

    private void Awake()
    {
        if (keySet == 0)
        {
            inputManager.OnPlayer1Jump += OnJump;
        }
     
    }

    void Update()
    {
        HandleMovement();
    }

    private void OnDestroy()
    {
        inputManager.OnPlayer1Jump -= OnJump;
    }

    public void SetScribbleActive(bool isActive)
    {
        scribbleObject.SetActive(isActive);
    }

    private void OnJump()
    {
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        shouldAnimateLimbs = false;

        Vector3 currentPos = transform.position;
        Vector3 forward = centerBone.right.normalized;
        Vector3 jumpTargetPos = transform.position + forward * 2;

        float jumpTime = 0f;

        while(jumpTime < jumpDuration)
        {
            transform.position = Vector3.Slerp(currentPos, jumpTargetPos, jumpTime / jumpDuration);
            jumpTime += Time.deltaTime;
            yield return null;
        }

        shouldAnimateLimbs = true;
    }

    private void HandleMovement()
    {
        float vInput = keySet == 0 ? inputManager.player1Vertical : inputManager.player2Vertical;
        float hInput = keySet == 0 ? inputManager.player1Horizontal : inputManager.player2Horizontal;

        if (vInput != 0)
        {
            Vector3 forward = centerBone.right.normalized * vInput * speed * Time.deltaTime;
            centerBone.position += forward;
        }


        if (hInput != 0)
        {
            Vector3 degrees = new Vector3(0f, 0f, -hInput * rotateSpeed * Time.deltaTime);
            centerBone.Rotate(degrees);
        }

        if (vInput == 0 && hInput == 0)
            climbingSound.Pause();
        else if (!climbingSound.isPlaying)
            climbingSound.UnPause();
    }
}
