using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    [SerializeField] private RockSpawner rockSpawner;

    [SerializeField] private AudioSource shakeSound;

    public Cinemachine.CinemachineVirtualCamera cinemaCamera;
    public float noiseAmplitude = 1f;
    public float noiseDuration = 1f;

    private bool isApplyingNoise = false;
    private float noiseEndTime = 0f;

    private void Start()
    {
        rockSpawner.OnRockSpawn += TriggerShake;
    }

    private void OnDestroy()
    {
        rockSpawner.OnRockSpawn -= TriggerShake;
    }

    void Update()
    {
        if (isApplyingNoise)
        {
            float t = Mathf.Clamp01((noiseEndTime - Time.time) / noiseDuration);
            float amplitude = Mathf.Lerp(0f, noiseAmplitude, t);
            cinemaCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;

            if (Time.time >= noiseEndTime)
            {
                cinemaCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
                isApplyingNoise = false;
            }
        }
    }

    void TriggerShake()
    {
        shakeSound.Play();
        isApplyingNoise = true;
        noiseEndTime = Time.time + noiseDuration;
    }
}
