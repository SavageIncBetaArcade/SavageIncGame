using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggeredTranslate : MonoBehaviour
{
    public InteractionTrigger[] Triggers;
    public Vector3 TriggeredPositionOffset;
    public float Speed = 5.0f;
    private Vector3 origin;
    private Vector3 targetPosition;


    public void Awake()
    {
        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger += translate;
        }

        origin = transform.position;
        targetPosition = origin;
    }

    public void OnDestroy()
    {
        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger -= translate;
        }
    }

    private void translate(bool triggered, InteractionTrigger trigger)
    {
        //check if all triggers are met
        if (InteractionTrigger.AnyFalse(Triggers))
        {
            targetPosition = origin;
            return;
            
        }

        targetPosition = origin - TriggeredPositionOffset;
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
    }
}
