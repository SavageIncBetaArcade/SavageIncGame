using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbility : AttackAbility
{
    private bool _hasHit;

    private ForwardTriggerCollision forwardTrigger;

    public MeleeAbility(UseableAbility useableAbility, ForwardTriggerCollision forwardTrigger) 
        : base(useableAbility)
    {
        this.forwardTrigger = forwardTrigger;
        this.forwardTrigger.Initialize(TriggerEnter);
    }

    public void TriggerEnter(Collider collider)
    {
        //First check if it has a health component
        //TODO add health component
        if (!_hasHit)
        {
            CharacterBase hitCharacter = collider.GetComponent<CharacterBase>();
            if (hitCharacter != null && hitCharacter != useableAbility.CharacterBase)
            {
                Hit(hitCharacter);
                _hasHit = true;
            }
        }
    }

    public override void Initilise()
    {
        if (forwardTrigger != null)
            forwardTrigger.Initialize(TriggerEnter);
    }

    public override void Use()
    {
        _hasHit = false;

        Debug.Log("Attacking");
    }

    protected override void Hit(CharacterBase hitCharacter)
    {
        Debug.Log("Hit");
    }
}
