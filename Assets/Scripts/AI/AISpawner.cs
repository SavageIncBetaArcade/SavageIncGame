using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
public class AISpawner : MonoBehaviour
{
    [System.Serializable]
    private enum CompleteState
    {
        CompleteOnTrigger,
        CompleteOnSpawnLimit
    }

    public InteractionTrigger[] SpawnTriggers;
    public InteractionTrigger[] StopTriggers;
    public AIBase SpawnAI;
    public Transform SpawnPoint;
    public bool SpawnEnemies;
    public float SpawnFrequency = 5;
    public int SpawnLimit = 3;

    [SerializeField]
    private CompleteState completeState;
    private int currentSpanwedCount;
    private float spawnTimer;

    private InteractionTrigger completeTrigger;


    void Awake()
    {
        foreach (var trigger in SpawnTriggers)
        {
            trigger.OnTrigger += checkSpawnTriggers;
        }

        foreach (var trigger in StopTriggers)
        {
            trigger.OnTrigger += checkStopTriggers;
        }

        spawnTimer = SpawnFrequency;
        completeTrigger = GetComponent<InteractionTrigger>();
        completeTrigger.IsInteractable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(SpawnEnemies)
        {
            spawnAI();

            if (hasCompleted())
            {
                SpawnEnemies = false;
                completeTrigger.Interact();
            }
        }
    }

    void checkSpawnTriggers(bool triggered, InteractionTrigger trigger)
    {
        //check if all triggers are met if so invert spawn enemies
        if (InteractionTrigger.AllTrue(SpawnTriggers))
        {
            SpawnEnemies = true;
        }
    }

    void checkStopTriggers(bool triggered, InteractionTrigger trigger)
    {
        //check if all triggers are met if so invert spawn enemies
        if (InteractionTrigger.AllTrue(StopTriggers))
        {
            SpawnEnemies = false;
        }
    }

    void spawnAI()
    {
        if (currentSpanwedCount < SpawnLimit && spawnTimer >= SpawnFrequency)
        {
            AIBase aiBase = Instantiate(SpawnAI);
            aiBase.transform.position = SpawnPoint.position;
            aiBase.transform.rotation = SpawnPoint.rotation;
            aiBase.gameObject.SetActive(true);

            aiBase.OnDeath += () =>
            {
                currentSpanwedCount--;
            };

            spawnTimer = 0;
            currentSpanwedCount++;
        }

        spawnTimer += Time.deltaTime;
    }

    bool hasCompleted()
    {
        switch (completeState)
        {
            case CompleteState.CompleteOnTrigger:
                return InteractionTrigger.AllTrue(StopTriggers);
            case CompleteState.CompleteOnSpawnLimit:
                return currentSpanwedCount >= SpawnLimit;
            default:
                return false;
        }
    }
}
