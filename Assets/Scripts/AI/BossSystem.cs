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
            }
            immobileTimer -= Time.deltaTime;
        }
        if(freezePadsStarted)
        {
            if (freezeTimer <= 0.0f)
            {
                freezePadsStarted = false;
            }
            freezeTimer -= Time.deltaTime;
        }
    }

    void checkFreezePads(bool triggered)
    {
        if (!freezePadsStarted)
        {
            freezePadsStarted = true;
            freezeTimer = 10.0f;
        }
        if (InteractionTrigger.AllTrue(freezePads))
        {
            Boss.BossStopped = true;
            immobileTimer = 6.0f;
        }
    }
}
