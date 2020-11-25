using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public InteractionTrigger[] Triggers;
    public AIBase SpawnAI;
    public Transform SpawnPoint;
    public bool SpawnEnemies;
    public float SpawnFrequency = 5;
    public int SpawnLimit = 3;

    private int currentSpanwedCount;
    private float spawnTimer;


    void Awake()
    {
        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger += checkTriggers;
        }

        spawnTimer = SpawnFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if(SpawnEnemies)
            spawnAI();
    }

    void checkTriggers(bool triggered)
    {
        //check if all triggers are met if so invert spawn enemies
        if (Triggers.All(x => x.Triggered))
        {
            SpawnEnemies = !SpawnEnemies;
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
}
