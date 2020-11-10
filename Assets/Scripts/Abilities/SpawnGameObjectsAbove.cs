using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGameObjectsAbove : MonoBehaviour
{
    public GameObject SpawnObject;
    public float SpawnHeight;
    public int MaxObjectCount = 25;
    public float SpawnTime = 5.0f;
    public float SpawnFrequency = 0.25f;
    public float SpawnRadius = 5.0f;
    public float LifeTime = 2.0f;

    private List<GameObject> objectPool;

    void Awake()
    {
        if(!gameObject)
            return;

        objectPool = new List<GameObject>(25);
        for (int i = 0; i < MaxObjectCount; i++)
        {
            objectPool.Add( Instantiate(SpawnObject,transform));
            objectPool[i].SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
    }

    private IEnumerator spawn()
    {
        float endTime = Time.time + SpawnTime;
        float currentTime = Time.time;

        while (currentTime <= endTime)
        {
            currentTime += SpawnFrequency;

            GameObject spawnedObject = getObjectFromPool();

            if (spawnedObject)
            {
                EnableObject(spawnedObject, GetRandomSpawnPoint());
                StartCoroutine(disableObjectAfterLifetime(spawnedObject));
            }

            yield return new WaitForSeconds(SpawnFrequency);
        }
        Destroy(gameObject,LifeTime);
    }

    private void EnableObject(GameObject gameObject, Vector3 position)
    {
        gameObject.transform.position = transform.position + position;
        gameObject.SetActive(true);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector2 radiusPoint = Random.insideUnitCircle * SpawnRadius;
        return new Vector3(radiusPoint.x, transform.position.y + SpawnHeight, radiusPoint.y);
    }

    private GameObject getObjectFromPool()
    {
        for (int i = 0; i < MaxObjectCount; i++)
        {
            if (!objectPool[i].activeInHierarchy)
                return objectPool[i];
        }

        return null;
    }

    private IEnumerator disableObjectAfterLifetime(GameObject gameObject)
    {
        yield return new WaitForSeconds(LifeTime);
        gameObject.SetActive(false);
    }
}
