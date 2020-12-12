using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform StartTransform;
    public Transform EndTransform;
    public float Speed = 5.0f;
    public InteractionTrigger[] Triggers;
    public bool InverseTriggers = true;
    public bool StartByDefault = true;
    public bool DefaultToStartPositon = true;
    public float StartDelay = 0.0f;


    private Vector3 targetPosition;
    private bool isMoving;
    private BoxCollider triggerCollider;

    public void Awake()
    {
        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger += togglePlatforms;
        }

        transform.position = StartTransform.position;
        targetPosition = EndTransform.position;

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


    void togglePlatforms(bool triggered)
    {
        //check if all triggers are met
        isMoving = InverseTriggers ? InteractionTrigger.AllFalse(Triggers) :
            InteractionTrigger.AllTrue(Triggers);

        if (!isMoving)
        {
            if (DefaultToStartPositon)
            {
                //move platform to start
                targetPosition = StartTransform.position;
            }
            else
            {
                //move platform to end
                targetPosition = EndTransform.position;
            }
        }
    }

    void Update()
    {
        if (isMoving)
        {
            float t = Mathf.Clamp(0.5f * (1 + Mathf.Sin(Time.time * Speed)), 0.01f, 0.99f);
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
}
