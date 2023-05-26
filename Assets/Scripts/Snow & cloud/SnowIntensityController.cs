using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowIntensityController : MonoBehaviour
{
    [SerializeField] private ProgressTracker progressTracker;
    
    [SerializeField] private float minEmmision;
    [SerializeField] private float maxEmmision;

    private float minSimSpeed = 1;
    [SerializeField] private float maxSimSpeed;
    
    [SerializeField] private float minWindForce;
    [SerializeField] private float maxWindForce;

    private ParticleSystem _snowParticleSystem;
    [SerializeField] private float minNoise;
    [SerializeField] private float maxNoise;

    private void Start()
    {
        _snowParticleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        float percentClimbed = progressTracker.percentClimbed;
        ChangeEmissionRate(percentClimbed);
        ChangeSimSpeed(percentClimbed);
        ChangeWindForce(percentClimbed);
        ChangeNoiseIntensity(percentClimbed);
    }

    private void ChangeNoiseIntensity(float percentClimbed)
    {
        float newSimSpeed = minNoise + (maxNoise - minNoise) * percentClimbed;
        var noiseModule = _snowParticleSystem.noise;
       noiseModule.strength = newSimSpeed;
    }

    private void ChangeWindForce(float percentClimbed)
    {
        float newWindForce = minWindForce + (maxWindForce - minWindForce) * percentClimbed;
        var forceOverLifetimeModule = _snowParticleSystem.forceOverLifetime;
        forceOverLifetimeModule.x = -newWindForce;
    }

    private void ChangeSimSpeed(float percentClimbed)
    {
        float newSimSpeed = minSimSpeed + (maxSimSpeed - minSimSpeed) * percentClimbed;
        var mainModule = _snowParticleSystem.main;
        mainModule.simulationSpeed = newSimSpeed;
    }

    private void ChangeEmissionRate(float percentClimbed)
    {
        float newEmission = minEmmision + (maxEmmision - minEmmision) * percentClimbed;
        var emissionModule = _snowParticleSystem.emission;
        emissionModule.rateOverTime = newEmission;
    }
}
