using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SnowFlakeSway : MonoBehaviour
{
    [SerializeField] private float minTimeBetweenSways = 0.5f;
    [SerializeField] private float maxTimeBetweenSways = 1;
    
    [SerializeField] private float forceAmount = 1f;
    
    private Rigidbody2D _rigidbody2D;
    private float _lastPush = 0;
    private bool _swayRight;
    private float _timeBetweenSways;
    [SerializeField] private float _swayDuration = 1f;
    private bool _isSwaying = false;
    [SerializeField] private float upwardsForce = 0.3f;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _timeBetweenSways = Random.Range(minTimeBetweenSways, maxTimeBetweenSways);
    }

    private void FixedUpdate()
    {
        if (_isSwaying)
        {
            float forceX = _swayRight ? 1 : -1;
            forceX *= forceAmount;
            float forceY = (Time.time - _lastPush) > _swayDuration / 2 ? upwardsForce : 0;
            Vector2 force = new Vector2(forceX, forceY);
            _rigidbody2D.AddForce(force, ForceMode2D.Force);
            _isSwaying = Time.time - _lastPush < _swayDuration;
            return;
        }
        if (Time.time - _lastPush > _timeBetweenSways)
        {
            _swayRight = !_swayRight;
            _isSwaying = true;
            _timeBetweenSways = Random.Range(minTimeBetweenSways, maxTimeBetweenSways);
            _lastPush = Time.time;
        }
            
    }
}
