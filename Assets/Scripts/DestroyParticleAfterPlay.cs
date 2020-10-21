using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleAfterPlay : MonoBehaviour
{
    private ParticleSystem particleSystem;
    
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        float time = particleSystem.main.duration;
        Destroy(gameObject,time);
    }

}
