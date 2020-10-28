using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ForwardTriggerCollision : MonoBehaviour
{ 
    public delegate void TriggerEnter(Collider other);
    private TriggerEnter onTrigger;

    public delegate void CollisionEnter(Collision other);
    private CollisionEnter onCollision;


    public void Initialize(TriggerEnter triggerDelecate)
    {
        onTrigger = triggerDelecate;
    }

    public void Initialize(CollisionEnter collisionEnter)
    {
        onCollision = collisionEnter;
    }

    void OnTriggerEnter(Collider other)
    {
        onTrigger?.Invoke(other);
    }

    void OnCollisionEnter(Collision other)
    {
        onCollision?.Invoke(other);
    }
}