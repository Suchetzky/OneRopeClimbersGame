using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedPlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float rotateSpeed = 3f;

    [SerializeField]
    private Rigidbody2D ropeTarget;

    private Rigidbody2D myRb;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();

        CreateRope(20, myRb, ropeTarget);
    }

    void FixedUpdate()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        if (hInput != 0)
        {
            Rotate(hInput);
        }

        if (vInput != 0)
        {
            Move(vInput);
        }
    }

    private void Move(float input)
    {
        Vector2 facingDirection = myRb.transform.up.normalized;
        Vector2 force = facingDirection * input * speed * Time.fixedDeltaTime;

        myRb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Rotate(float input)
    {
        var newRotation = myRb.rotation + (-input * rotateSpeed * Time.fixedDeltaTime);

        myRb.MoveRotation(newRotation);
    }

    private void VisualizeRope()
    {
        DistanceJoint2D joint = GetComponent<DistanceJoint2D>();

        Vector3 pos1 = joint.connectedBody.transform.position;
        Vector3 pos2 = transform.position;

        int numPoints = 20;
        Vector3[] positions = new Vector3[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (float)(numPoints - 1);
            positions[i] = Vector3.Lerp(pos1, pos2, t);
        }

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        lineRenderer.SetPositions(positions);
    }

    void CreateRope(int numSegments, Rigidbody2D startRigidbody, Rigidbody2D endRigidbody)
    {
        GameObject ropeContainer = new GameObject("RopeContainer");

        var startPos = new Vector2(startRigidbody.transform.position.x, startRigidbody.transform.position.z);
        var endPos = new Vector2(endRigidbody.transform.position.x, endRigidbody.transform.position.z);
        float segmentLength = Vector2.Distance(startPos, endPos) / numSegments;

        Rigidbody2D prevRigidbody = startRigidbody;

        for (int i = 1; i < numSegments; i++)
        {
            GameObject segmentObject = new GameObject("RopeSegment");
            segmentObject.transform.parent = ropeContainer.transform;

            Rigidbody2D rigidbody = segmentObject.AddComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;

            Vector2 offset = (i * segmentLength) * (endPos - startPos).normalized;
            rigidbody.position = startPos + offset;

            DistanceJoint2D distanceJoint = segmentObject.AddComponent<DistanceJoint2D>();
            distanceJoint.connectedBody = prevRigidbody;
            distanceJoint.distance = segmentLength;

            prevRigidbody = rigidbody;
        }

        DistanceJoint2D finalJoint = prevRigidbody.gameObject.AddComponent<DistanceJoint2D>();
        finalJoint.connectedBody = endRigidbody;
        finalJoint.distance = segmentLength;
    }
}
