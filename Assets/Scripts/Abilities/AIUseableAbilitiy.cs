using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUseableAbilitiy : UseableAbility
{
    public bool IgnoreCooldown = false;
    public LayerMask Layermask;
    public float OverrideCooldown = -1.0f;

    private float useTimer = 0.0f;
    protected override void Initilise()
    {
        base.Initilise();
        useTimer = 0.0f;
        if (worldGameObject)
        {
            ForwardTriggerCollision trigger = worldGameObject.GetComponent<ForwardTriggerCollision>();
            if(trigger)
                trigger.CollisionLayers = Layermask; //set the melee forward trigger to player
        }
    }

    public void Attack()
    {
        if (ScriptableAbility != null)
        {
            if (OverrideCooldown > 0.0f && useTimer > OverrideCooldown)
            {
                ExecuteUse();
                useTimer = 0.0f;
            }
            else if(OverrideCooldown <= 0.0f && (IgnoreCooldown || !OnCooldown()))
            {
                ExecuteUse();
            }
        }
    }

    void Update()
    {
        useTimer += Time.deltaTime;
    } 
}
