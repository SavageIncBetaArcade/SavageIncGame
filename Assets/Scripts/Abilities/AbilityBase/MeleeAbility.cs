﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbility : AttackAbility
{
    //TODO keep track of the character bases hit so you can hit multiple enemies in one swing
    private bool _hasHit;
    //ForwardTriggerCollision from the weapons collider, so when the OnTriggerEnter get called the TriggerEnter also gets called
    private ForwardTriggerCollision forwardTrigger;

    public MeleeAbility(UseableAbility useableAbility, ForwardTriggerCollision forwardTrigger, CharacterBase ownerCharacter) : base(useableAbility, ownerCharacter)
    {
        this.forwardTrigger = forwardTrigger;
        this.forwardTrigger.Initialize(TriggerEnter);
        this.forwardTrigger.InitializeStay(TriggerEnter);
        _hasHit = true;
    }

    /// <summary>
    /// This method gets called from the first collier in the children of this gameobjects OnTriggerEnter method.
    /// </summary>
    /// <param name="collider"></param>
    public void TriggerEnter(Collider collider)
    {
        //First check if it has a health component
        //TODO add health component
        if (!_hasHit)
        {
            ScriptableMeleeAbility meleeAbility = Ability as ScriptableMeleeAbility;
                if (meleeAbility == null)
                Debug.LogError("MeleeAbility: ScriptableAbility is not of type ScriptableMeleeAbility");

            //TODO get hit normal from InteractionTrigger enter
            Hit(collider.gameObject, meleeAbility != null ? meleeAbility.Damage : 0.0f, collider.ClosestPoint(collider.transform.position), OwnerCharacter.transform.forward, Vector3.zero);
            _hasHit = true;
        }
    }

    public override void Initilise()
    {
        if (forwardTrigger != null)
        {
            forwardTrigger.Initialize(TriggerEnter);
            forwardTrigger.InitializeStay(TriggerEnter);
        }
    }

    public override void Use()
    {
        _hasHit = false;
    }

    public override void Hit(GameObject hitObject, float damage, Vector3 hitPoint, Vector3 hitDirection,
        Vector3 surfaceNormal)
    {
        Debug.Log("Hit");

        base.Hit(hitObject, damage, hitPoint, hitDirection, surfaceNormal);
    }
}
