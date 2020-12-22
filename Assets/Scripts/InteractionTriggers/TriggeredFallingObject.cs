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
    public AudioSource ImpactAudioSource;

    private Rigidbody rigidbody;
    private bool impacted = false;

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

    private void Trigger(bool triggered, InteractionTrigger trigger)
    {
        if (InteractionTrigger.AllTrue(Triggers))
        {
            FindObjectOfType<PlayerCamera>()?.ShakePosition(FallDelay, new Vector2(0.4f, 0.2f), -0.1f, 0.01f, 7.5f);
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(FallDelay);
        rigidbody.isKinematic = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!impacted)
        {
            ImpactAudioSource?.Play();
            //add a shake once method for impacts
            FindObjectOfType<PlayerCamera>()?.ShakePosition(0.1f, new Vector2(0.2f, 0.8f), -0.01f, 0.1f, 20.0f);
            impacted = true;

            StartCoroutine(disableAfterSound());
        }
    }

    IEnumerator disableAfterSound()
    {
        if(ImpactAudioSource && ImpactAudioSource.clip)
            yield return new WaitForSeconds(ImpactAudioSource.clip.length);
        gameObject.SetActive(false);
    }

}
