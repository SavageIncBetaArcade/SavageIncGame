using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAbilitiy : AttackAbility
{
    public float Damage;
    public float Range = 25.0f;
    public Transform RayOrigin;

    public override void Use()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(RayOrigin.position, RayOrigin.forward, out hitInfo, Range))
        {
            //First check if it has a health component
            Hit();
            
        }


        Debug.Log("Attacking");
    }

    protected override void Hit()
    {

        Debug.Log("Hit");
    }
}
