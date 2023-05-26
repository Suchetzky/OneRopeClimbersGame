using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    private void Start()
    {
        animator = GetComponent<Animator>();

        PlayerJumpScript.OnBothPlayersDead += HandleBothPlayersDead;
    }

    private void OnDestroy()
    {
        PlayerJumpScript.OnBothPlayersDead -= HandleBothPlayersDead;
    }

    private void Update()
    {
        if (DidRestart())
        {
            StartCoroutine(PerformEndTransition());
        }
    }

    private void HandleBothPlayersDead()
    {
        StartCoroutine(FallThenEnd());
        //animator.SetTrigger("Start");
    }

    private IEnumerator FallThenEnd()
    {
        //virtualCamera.Follow = Instantiate(virtualCamera.Follow);
       //virtualCamera.LookAt = Instantiate(virtualCamera.LookAt);
       targetGroup.m_Targets = Array.Empty<CinemachineTargetGroup.Target>();
       yield return new WaitForSeconds(.5f);
       animator.SetTrigger("Start");
    }

    private IEnumerator PerformEndTransition()
    {
        animator.SetTrigger("End");

        yield return new WaitForSeconds(.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private bool DidRestart()
    {
        bool didKeyboardRestart = Input.GetKeyDown(KeyCode.Space);

        ReadOnlyArray<Gamepad> gamepads = Gamepad.all;

        bool didPlayer1GamepadRestart = false;
        if (gamepads.Count > 0)
        {
            didPlayer1GamepadRestart = gamepads[0].buttonSouth.wasReleasedThisFrame;
        }

        bool didPlayer2GamepadRestart = false;
        if (gamepads.Count > 1)
        {
            didPlayer2GamepadRestart = gamepads[1].buttonSouth.wasReleasedThisFrame;
        }

        return didKeyboardRestart || didPlayer1GamepadRestart || didPlayer2GamepadRestart;
    }
}
