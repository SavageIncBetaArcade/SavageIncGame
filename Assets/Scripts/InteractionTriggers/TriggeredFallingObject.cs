using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TriggeredFallingObject : MonoBehaviour
{
    public InteractionTrigger[] Triggers;

    public float FallDelay;
    public float DestroyDelay;

    private Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        foreach (var trigger in Triggers)
        {
            trigger.OnTrigger += Trigger;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Trigger(bool triggered)
    {
        if (Triggers.All(x => x.Triggered))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(FallDelay);
        rigidbody.isKinematic = false;
    }

}
