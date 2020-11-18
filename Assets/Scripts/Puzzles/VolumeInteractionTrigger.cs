using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VolumeInteractionTrigger : InteractionTrigger
{
    public LayerMask mask = 1024;

    protected override void Awake()
    {
        base.Awake();

        GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        //For some reason character controllers don't work well with OnTriggerEnter
        //To overcome this issue you need to attach a rigidbody that is kinematic on the character controller object
        //Then have a collider that is InteractionTrigger only on the character controller 
        if (((1 << collider.gameObject.layer) & mask) == 0)
            return;
        

        Interact();
    }

    void OnTriggerExit(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & mask) == 0)
            return;

        Reset();
    }
}