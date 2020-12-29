using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSystem : MonoBehaviour
{
    public InteractionTrigger[] freezePads;
    public InteractionTrigger[] wobblyPlatforms;
    public AIBase Boss;
    float immobileTimer = 0.0f;
    bool freezePadsStarted = false;
    float freezeTimer = 0.0f;

    private void Awake()
    {
        foreach (var trigger in freezePads)
        {
            trigger.OnTrigger += checkFreezePads;
        }
    }
    
    private void Update()
    {
        float healthPercentage = Boss.CurrentHealth / Boss.MaxHealth;
        if (healthPercentage < 0.25f)
        {
            foreach (var floor in wobblyPlatforms)
            {
                floor.IsInteractable =  false;
            }
            GetComponent<AISpawner>().SpawnEnemies = false;
        }
        else if (healthPercentage < 0.6f)
        {
            foreach(var floor in wobblyPlatforms)
            {
                floor.IsInteractable = true;
            }
            GetComponent<AISpawner>().SpawnEnemies = true;
        }

        if (Boss.BossStopped)
        {
            if (immobileTimer <= 0.0f)
            {
                Boss.BossStopped = false;
                Boss.NavAgent.isStopped = false;
            }
            immobileTimer -= Time.deltaTime;
        }
        if(freezePadsStarted)
        {
            if (freezeTimer <= 0.0f)
            {
                freezePadsStarted = false;
                foreach (var trigger in freezePads)
                {
                    trigger.Reset();
                }
            }
            freezeTimer -= Time.deltaTime;
        }
    }

    private void checkFreezePads(bool triggered, InteractionTrigger trigger)
    {
        if (InteractionTrigger.AllTrue(freezePads))
        {
            freezePadsStarted = false;
            Boss.BossStopped = true;
            Boss.NavAgent.isStopped = true;
            immobileTimer = 10.0f;
            Debug.Log("bossFrozen: " + freezeTimer);
            freezeTimer = 0.0f;
        }
        else
        {
            freezePadsStarted = true;
            freezeTimer = 10.0f;
        }
    }
}
