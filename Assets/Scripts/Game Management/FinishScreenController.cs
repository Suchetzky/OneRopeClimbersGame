using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FinishScreenController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioSource;
    
    void Start()
    {
        MountainTopController.PlayersHitTop += OnPlayersHitTop;
    }

    private void OnDestroy()
    {
        MountainTopController.PlayersHitTop -= OnPlayersHitTop;
    }

    private void OnPlayersHitTop()
    {
        audioSource.Stop();
        videoPlayer.Play();
    }
}
