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
    public void Initialize(TriggerEnter triggerDelecate)
    {
        onTrigger = triggerDelecate;
    }

    void OnTriggerEnter(Collider other)
    {
        onTrigger?.Invoke(other);
    }
}