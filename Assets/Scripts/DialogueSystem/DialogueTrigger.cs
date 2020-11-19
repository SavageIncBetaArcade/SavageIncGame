using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueTrigger : MonoBehaviour
{
    public TextAsset jsonFile;
    private Entities entitiesInJson;

    void Start()
    {
        entitiesInJson = JsonUtility.FromJson<Entities>(jsonFile.text);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (Entity entity in entitiesInJson.entities)
            {
                if (entity.name == "UHFGYEG")
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(entity.name, entity.dialogue);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.enabled)
        {
            this.enabled = false;
        }

        if(other.CompareTag("Player"))
        {
            foreach (Entity entity in entitiesInJson.entities)
            {
                if (entity.name == "Gertrude")
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(entity.name, entity.dialogue);
                }
            }
        }
    }
}
