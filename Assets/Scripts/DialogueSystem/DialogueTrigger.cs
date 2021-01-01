using UnityEngine;

public class DialogueTrigger : VolumeInteractionTrigger
{
    public TextAsset JsonFile;
    public string EntityKey;

    private Entities entitiesInJson;
    private bool isTriggered = false;
    private Transform target;
    private float triggerRange = 4;

    protected override void Awake()
    {
        base.Awake();

        IsInteractable = true;
        Toggle = true;
    }

    void Start()
    {
        entitiesInJson = JsonUtility.FromJson<Entities>(JsonFile.text);
    }

    public bool getState()
    {
        return isTriggered;
    }

    public override void Interact()
    {
        base.Interact();

        foreach (Entity entity in entitiesInJson.entities)
        {
            //StopAllCoroutines();
            if (entity.Key == EntityKey)
            {
                FindObjectOfType<DialogueManager>().DisplayDialogue(entity.Name, entity.Dialogue, entity.TextType, this);
                FindObjectOfType<DialogueManager>().DisplayControls(entity.Control, "control", this);
            }
        }
    }

    //public void setState(bool state)
    //{
    //    isTriggered = state;
    //}

    //private void Update()
    //{
    //    //if (target == null) return;

    //    //float distance = Vector3.Distance(transform.position, target.position);

    //    //if(distance > triggerRange)
    //    //{
    //    //    isTriggered = false;
    //    //}
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player") && isTriggered)
    //        isTriggered = false;
    //}

    //void OnTriggerEnter(Collider other)
    //{
    //    target = other.transform;

    //    if (other.CompareTag("Player") && !isTriggered)
    //    {
    //        foreach (Entity entity in entitiesInJson.entities)
    //        {
    //            //StopAllCoroutines();
    //            if (entity.Key == EntityKey)
    //            {
    //                FindObjectOfType<DialogueManager>().DisplayDialogue(entity.Name, entity.Dialogue, entity.TextType, this);
    //                FindObjectOfType<DialogueManager>().DisplayControls(entity.Control, "control", this);
    //            }
    //        }

    //        isTriggered = true;
    //    }
    //}
}