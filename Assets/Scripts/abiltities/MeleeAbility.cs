using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbility : AttackAbility
{
    private bool _hasHit;

    private ScriptableMeleeAbility meleeAbility;

    protected override void Awake()
    {
        base.Awake();

        if (Ability != null && Ability is ScriptableMeleeAbility ability)
        {
            meleeAbility = ability;

            //Forward the melees OnTriggerEnter to the TriggerEnter method by searching all the children for ForwardTriggerCollision
            ForwardTriggerCollision forwardTrigger = GetComponentInChildren<ForwardTriggerCollision>();
            if(forwardTrigger != null)
                forwardTrigger.Initialize(TriggerEnter);
        }
        else
        {
            Debug.LogAssertion("Melee Ability doesn't have an ability");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    public void TriggerEnter(Collider collider)
    {
        //First check if it has a health component
        //TODO add health component
        if (!_hasHit && OnCooldown())
        {
            Hit();
            _hasHit = true;
        }
    }

    public override void Use()
    {
        _hasHit = false;

        Debug.Log("Attacking");
    }

    protected override void Hit()
    {
        Debug.Log("Hit");
    }
}
