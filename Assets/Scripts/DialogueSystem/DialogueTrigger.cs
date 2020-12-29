using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset JsonFile;
    public string EntityName;

    private Entities entitiesInJson;
    private bool isTriggered = false;
    private Transform target;
    private float triggerRange = 4;

    void Start()
    {
        entitiesInJson = JsonUtility.FromJson<Entities>(JsonFile.text);
    }

    public bool getState()
    {
        return isTriggered;
    }

    public void setState(bool state)
    {
        isTriggered = state;
    }

    private void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if(distance > triggerRange)
        {
            isTriggered = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        target = other.transform;

        if (other.CompareTag("Player") && !isTriggered)
        {
            foreach (Entity entity in entitiesInJson.entities)
            {
                StopAllCoroutines();

                if (entity.Name == EntityName && entity.TextType == "dialogue")
                {
                    FindObjectOfType<DialogueManager>().DisplayDialogue(entity.Name, entity.Dialogue, entity.TextType, this);
                }
                else if (entity.Name == EntityName && entity.TextType == "control")
                {
                    FindObjectOfType<DialogueManager>().DisplayControls(entity.Dialogue, entity.TextType, this);
                }
            }

            isTriggered = true;
        }
    }
}