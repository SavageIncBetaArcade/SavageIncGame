using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueTrigger : MonoBehaviour
{
    public TextAsset JsonFile;
    public String EntityName;
    private Entities entitiesInJson;
    private bool isTriggered = false;

    void Start()
    {
        entitiesInJson = JsonUtility.FromJson<Entities>(JsonFile.text);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isTriggered == false)
        {
            foreach (Entity entity in entitiesInJson.entities)
            {
                if (entity.name == EntityName && entity.textType == "dialogue")
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(entity.name, entity.dialogue, entity.textType);
                }
                else if (entity.name == EntityName && entity.textType == "control")
                {
                    FindObjectOfType<DialogueManager>().DisplayControls(entity.name, entity.dialogue, entity.textType);
                }
            }

            isTriggered = true;
        }
    }
}
