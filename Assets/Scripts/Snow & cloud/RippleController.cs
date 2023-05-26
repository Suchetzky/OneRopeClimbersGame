using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleController : MonoBehaviour
{
    private Animator animator;
    
    private static readonly int Offset = Animator.StringToHash("Offset");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat(Offset, Random.Range(0.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
