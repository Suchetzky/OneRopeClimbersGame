using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rock : MonoBehaviour
{
    [SerializeField] private GameObject _warningPrefab;
    [SerializeField] private Sprite[] rockSprites;
    
    [SerializeField] private float minScaleFactor;
    [SerializeField] private float maxScaleFactor;
    public Canvas targetCanvas;

    public static event Action OnRockHitRope;

    private Camera mainCamera;
    private GameObject _warning;
    private bool _gameStarted = false;
    private SpriteRenderer _spriteRenderer;
    private PolygonCollider2D _collider;
    private AudioSource _hitSound;
    [SerializeField] private float timeBetweenWarningFlashes = 0.5f;
    [SerializeField] private int numOfWarningFlashes = 3;

    private void Awake()
    {
        mainCamera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
        _hitSound = GetComponent<AudioSource>();

        // _warning.SetActive(false);
    }

    private void Start()
    {
        //_gameStarted = true;
        _collider.enabled = true;
    }

    private void OnEnable()
    {
        if (_gameStarted)
        {
            ChooseRandomRockSprite();
            
            // commented due to correct rock size in sprite, no need for this no more
            // ChangeScale();
            
            ResetCollider();
            StartCoroutine(WarnThenFall());
        }
        else
            _gameStarted = true;
    }

    private void ChangeScale()
    {
        float scaleFactor = Random.Range(minScaleFactor, maxScaleFactor);
        transform.localScale = Vector3.one * scaleFactor;
    }

    private void ResetCollider()
    {
        
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void ChooseRandomRockSprite()
    {
        _spriteRenderer.sprite = rockSprites[Random.Range(0, rockSprites.Length)];
    }


    private IEnumerator WarnThenFall()
    {
        _warning = Instantiate(_warningPrefab, Vector3.zero, Quaternion.identity, targetCanvas.transform);

        transform.position += new Vector3(0, Random.Range(3, 6), 0);
        Vector3 warningPos = new Vector3(transform.position.x, 0f, 0f);
        float initialDist = transform.position.y - warningPos.y;

        WarningSignController warningController = _warning.GetComponent<WarningSignController>();
        warningController.canvas = targetCanvas;
        warningController.targetTranform = transform;
        warningController.StartFlashing();
        
        float dist = initialDist;
        while (dist > 0.1)
        {
            dist = transform.position.y - warningPos.y;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Rope")
        {
            if (OnRockHitRope != null)
                OnRockHitRope();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player1") || col.gameObject.CompareTag("Player2"))
            _hitSound.Play();
    }
}