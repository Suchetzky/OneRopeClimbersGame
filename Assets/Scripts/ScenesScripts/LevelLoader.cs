using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime =1f;
    private static readonly int Start = Animator.StringToHash("Start");

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelnIndex)
    {
        transition.SetTrigger(Start);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(levelnIndex);
    }
}
