using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeGenerator : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    public float stiffness = .5f;
    public Vector2 gravity = new Vector2(0, -1f);
    public int constraintCount = 50;
    public int segmentCount = 20;
    public float segmentLength = .15f;

    private List<RopeSegment> ropeSegments;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;

    private void Awake()
    {
        if (edgeCollider == null)
            edgeCollider = GetComponent<EdgeCollider2D>();

        Rock.OnRockHitRope += OnRockHit;
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        ropeSegments = GenerateRope(startPos.position, segmentLength, segmentCount);
    }

    void Update()
    {
        SimulateRope(
            ropeSegments,
            gravity,
            stiffness,
            startPos.position,
            endPos.position,
            constraintCount);

        if (lineRenderer != null)
            DrawRope(ropeSegments, lineRenderer);

        edgeCollider.points = RopeSegmentUtility.MapRopeSegmentsToPoints(ropeSegments).ToArray();
    }

    private void OnDestroy()
    {
        Rock.OnRockHitRope -= OnRockHit;
    }

    private void SimulateRope(List<RopeSegment> rope, Vector2 gravity, float stiffness, Vector2 fixedPos1, Vector2 fixedPos2, int numConstraints)
    {
        for (int i = 0; i < rope.Count; i++)
        {
            Vector2 temp = rope[i].currentPos;

            if (i == 0)
            {
                rope[i].currentPos = fixedPos1;
            }
            else if (i == rope.Count - 1)
            {
                rope[i].currentPos = fixedPos2;
            }
            else
            {
                rope[i].currentPos = rope[i].currentPos + (rope[i].currentPos - rope[i].previousPos) * (1 - stiffness) + gravity;
            }

            rope[i].previousPos = temp;
        }

        for (int j = 0; j < numConstraints; j++)
        {
            rope[0].currentPos = fixedPos1;
            rope[rope.Count - 1].currentPos = fixedPos2;

            for (int i = 0; i < rope.Count - 1; i++)
            {
                Vector2 delta = rope[i + 1].currentPos - rope[i].currentPos;
                float deltaLength = delta.magnitude;
                float diff = deltaLength - stiffness;
                delta /= deltaLength;
                rope[i].currentPos += delta * 0.5f * diff;
                rope[i + 1].currentPos -= delta * 0.5f * diff;
            }
        }
    }


    private void DrawRope(List<RopeSegment> rope, LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = rope.Count;

        for (int i = 0; i < rope.Count; i++)
        {
            lineRenderer.SetPosition(i, rope[i].currentPos);
        }
    }

    private List<RopeSegment> GenerateRope(Vector2 startPos, float segmentLength, int numSegments)
    {
        List<RopeSegment> rope = new List<RopeSegment>();

        for (int i = 0; i < numSegments; i++)
        {
            Vector2 pos = startPos + Vector2.down * i * segmentLength;
            RopeSegment segment = new RopeSegment(pos);
            rope.Add(segment);
        }

        return rope;
    }

    public void RopeStiffnessUpdater(float stiff)
    {
        stiffness = stiff;
    }

    private void OnRockHit()
    {
    }
}

public class RopeSegment
{
    public Vector2 currentPos;
    public Vector2 previousPos;

    public RopeSegment(Vector2 pos)
    {
        currentPos = pos;
        previousPos = pos;
    }
}

public static class RopeSegmentUtility
{
    public static List<Vector2> MapRopeSegmentsToPoints(List<RopeSegment> ropeSegments)
    {
        List<Vector2> points = new List<Vector2>();

        foreach (RopeSegment segment in ropeSegments)
        {
            points.Add(segment.currentPos);
        }

        return points;
    }
}