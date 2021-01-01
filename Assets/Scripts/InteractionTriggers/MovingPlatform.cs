using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UUID))]
public class MovingPlatform : MonoBehaviour, IDataPersistance
{
    public Transform StartTransform;
    public Transform EndTransform;
    public float Speed = 5.0f;
    public InteractionTrigger[] Triggers;
    public bool InverseTriggers = true;
    public bool StartByDefault = true;
    public bool DefaultToStartPositon = true;
    public float StartDelay = 0.0f;


    private bool isMoving;
    private BoxCollider triggerCollider;
    private UUID uuid;

    public void Awake()
    {
        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger += togglePlatforms;
        }

        transform.position = StartTransform.position;

        triggerCollider = gameObject.AddComponent<BoxCollider>();
        triggerCollider.isTrigger = true;

        //get mesh renderer for bounds
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        const float colliderYOffset = 0.25f;
        if (meshRenderer)
        {
            triggerCollider.center = new Vector3(0, meshRenderer.bounds.extents.y + (colliderYOffset*0.5f), 0);
            triggerCollider.size = new Vector3(triggerCollider.size.x - colliderYOffset, colliderYOffset, triggerCollider.size.z - colliderYOffset);
        }
        else
        {
            triggerCollider.center = Vector3.up * colliderYOffset;
            triggerCollider.size += new Vector3(-1, 1, -1) * colliderYOffset;
        }
        uuid = GetComponent<UUID>();
        StartCoroutine(startDelay());
    }

    IEnumerator startDelay()
    {
        if (StartByDefault)
        {
            yield return new WaitForSeconds(StartDelay);
            isMoving = true;
        }
    }

    public void OnDestroy()
    {
        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger -= togglePlatforms;
        }
    }


    void togglePlatforms(bool triggered, InteractionTrigger trigger)
    {
        //check if all triggers are met
        isMoving = InverseTriggers ? InteractionTrigger.AllFalse(Triggers) :
            InteractionTrigger.AllTrue(Triggers);
    }

    void Update()
    {
        if (isMoving)
        {
            float t = Mathf.Clamp(0.5f * (1 + Mathf.Sin((Time.time * Speed) + StartDelay)), 0.01f, 0.99f);
            transform.position = Vector3.Lerp(StartTransform.position, EndTransform.position, t);
        }
        else
        {
            Vector3 target = EndTransform.position;
            if (DefaultToStartPositon)
                target = StartTransform.position;

            transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (!triggerCollider.bounds.Intersects(collision.bounds))
            return;

        var playerTransform = GetPlayerTransformFromParent(collision.gameObject.transform);
        if (playerTransform)
        {
            playerTransform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        var playerTransform = GetPlayerTransformFromParent(collision.gameObject.transform);
        if (playerTransform)
        {
            playerTransform.SetParent(null);
        }
    }

    private Transform GetPlayerTransformFromParent(Transform currentTransform)
    {
        if (currentTransform.tag == "Player")
            return currentTransform;

        if (!currentTransform.parent)
            return null;

        return GetPlayerTransformFromParent(currentTransform.parent);
    }

    public Dictionary<string, object> Save()
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
        if (!uuid)
            return dataDictionary;

        //Load currently saved values
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        DataPersitanceHelpers.SaveValueToDictionary(ref dataDictionary, "isMoving", isMoving);

        //save json to file
        DataPersitanceHelpers.SaveDictionary(ref dataDictionary, uuid.ID);

        return dataDictionary;
    }

    public Dictionary<string, object> Load(bool destroyUnloaded = false)
    {
        //create new dictionary to contain data for characterbase
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        if (!uuid)
            return dataDictionary;

        //load dictionary
        DataPersitanceHelpers.LoadDictionary(ref dataDictionary, uuid.ID);

        isMoving = DataPersitanceHelpers.GetValueFromDictionary<bool>(ref dataDictionary, "isMoving");

        return dataDictionary;
    }
}
