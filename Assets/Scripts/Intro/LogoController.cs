using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoController : MonoBehaviour
{
    public float delay = 2f;
    
    void Start()
    {
        StartCoroutine(StartLogoAnimation());
    }

    IEnumerator StartLogoAnimation()
    {
        yield return new WaitForSeconds(delay);
        
        GetComponent<Animator>().SetTrigger("Start");
    }
}
