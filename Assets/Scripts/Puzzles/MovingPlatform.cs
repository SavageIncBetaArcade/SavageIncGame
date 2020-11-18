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


    private Vector3 targetPosition;
    private bool isMoving;

    public void Awake()
    {
        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger += togglePlatforms;
        }

        transform.position = StartTransform.position;
        targetPosition = EndTransform.position;
        isMoving = StartByDefault;
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
        isMoving = InverseTriggers ? Triggers.All(x => !x.Triggered) :
            Triggers.All(x => x.Triggered);

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
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);

        if (isMoving && Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            if (targetPosition == StartTransform.position)
                targetPosition = EndTransform.position;
            else
                targetPosition = StartTransform.position;
        }
    }
}
