using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUseableAbilitiy : UseableAbility
{
    public bool IgnoreCooldown = false;
    public LayerMask Layermask;

    protected override void Initilise()
    {
        base.Initilise();

        if (worldGameObject)
        {
            ForwardTriggerCollision trigger = worldGameObject.GetComponent<ForwardTriggerCollision>();
            if(trigger)
                trigger.CollisionLayers = Layermask; //set the melee forward trigger to player
        }
    }

    public void Attack()
    {
        if (ScriptableAbility != null && (IgnoreCooldown || !OnCooldown()))
        {
            ExecuteUse();
        }
    }
}
