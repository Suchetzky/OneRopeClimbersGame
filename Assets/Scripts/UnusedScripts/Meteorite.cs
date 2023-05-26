using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Meteorite : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 _prevVelocity;
    private float _speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
        
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        // Check if the meteorite collided with another meteorite.
        if (col.gameObject.CompareTag("Meteorite"))
        {
            _prevVelocity = rb.velocity;
            // Calculate the bounce direction based on the collision normal.
            ContactPoint2D contact = col.contacts[0];

            Vector2 contactNormal = contact.normal;
        
            FixCollisionVelocity(contactNormal);

            Vector2 newVelocity = Vector2.Reflect(_prevVelocity, contactNormal).normalized * _speed;

            rb.velocity = newVelocity;

            _prevVelocity = newVelocity;
        }
    }
    
    private void FixCollisionVelocity(Vector2 contactNormal)
        {
            float angleDeg = Vector2.SignedAngle(contactNormal, -_prevVelocity);
            float absDeg = Mathf.Abs(angleDeg);
    
            float clampedDeg = Mathf.Clamp(absDeg, 15, 70);
    
            if (Mathf.Approximately(absDeg, clampedDeg))
                return;
    
            float signedRad = clampedDeg * Mathf.Sign(angleDeg) * Mathf.Deg2Rad;
            float normalRad = Mathf.Atan2(contactNormal.y, contactNormal.x);
            float newRad = normalRad + signedRad;
    
            Vector2 newDir = new Vector2(Mathf.Cos(newRad), Mathf.Sin(newRad));
            float magnitude = _prevVelocity.magnitude;
            _prevVelocity = newDir * -magnitude;
        }
}
