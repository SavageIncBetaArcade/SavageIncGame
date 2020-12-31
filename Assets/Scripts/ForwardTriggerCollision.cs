using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ForwardTriggerCollision : MonoBehaviour
{
    public LayerMask CollisionLayers;

    public delegate void TriggerEnter(Collider other);
    private TriggerEnter onTrigger;

    public delegate void CollisionEnter(Collision other);
    private CollisionEnter onCollision;

    public delegate void TriggerStay(Collider other);
    private TriggerStay onTriggerStay;


    public void Initialize(TriggerEnter triggerDelecate)
    {
        onTrigger = triggerDelecate;
    }

    public void Initialize(CollisionEnter collisionEnter)
    {
        onCollision = collisionEnter;
    }

    public void InitializeStay(TriggerStay triggerStay)
    {
        onTriggerStay = triggerStay;
    }

    void OnTriggerEnter(Collider other)
    {
        if (CollisionLayers == (CollisionLayers | (1 << other.gameObject.layer)))
            onTrigger?.Invoke(other);
    }

    void OnCollisionEnter(Collision other)
    {
        if (CollisionLayers == (CollisionLayers | (1 << other.gameObject.layer)))
        {
            onCollision?.Invoke(other);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (CollisionLayers == (CollisionLayers | (1 << other.gameObject.layer)))
        {
            onTriggerStay?.Invoke(other);
        }
    }
}