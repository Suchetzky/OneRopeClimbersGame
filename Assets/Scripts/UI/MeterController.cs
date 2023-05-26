using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterController : MonoBehaviour
{
    [SerializeField]
    private RectTransform frame;

    [SerializeField]
    private RectTransform arrow;

    [SerializeField] private ProgressTracker progressTracker;

    public float percentage;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //float bottomPoint = frame.position.y - frame.rect.height / 2;
        //bottomPoint = frame.position.y;
        //float targetY = bottomPoint + frame.rect.height * percentage;
        // arrow.position = new Vector3(arrow.position.x, targetY, arrow.position.z);

        float percentClimbed = progressTracker.percentClimbed;
        arrow.anchoredPosition = new Vector2(
            arrow.anchoredPosition.x,
            (percentClimbed - 0.5f) * frame.rect.height);
        
        
    }
}
