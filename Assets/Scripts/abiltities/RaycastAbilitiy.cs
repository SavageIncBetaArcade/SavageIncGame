using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAbilitiy : BaseAbility
{
    public float Range = 25.0f;
    public float HitDelay = 0.0f;
    public Transform RayOrigin;

    public override void Attack()
    {
        base.Attack();

        RaycastHit hitInfo;
        if (Physics.Raycast(RayOrigin.position, RayOrigin.forward, out hitInfo, Range))
        {
            //First check if it has a health component
            Hit();
            
        }


        Debug.Log("Attacking");
    }

    public override void Hit()
    {
        base.Hit();

        Debug.Log("Hit");
    }
}
