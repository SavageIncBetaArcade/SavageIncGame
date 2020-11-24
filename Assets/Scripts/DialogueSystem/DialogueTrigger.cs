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
                if (entity.Name == EntityName && entity.TextType == "dialogue")
                {
                    FindObjectOfType<DialogueManager>().DisplayDialogue(entity.Name, entity.Dialogue, entity.TextType);
                }
                else if (entity.Name == EntityName && entity.TextType == "control")
                {
                    FindObjectOfType<DialogueManager>().DisplayControls(entity.Name, entity.Dialogue, entity.TextType);
                }
            }

            isTriggered = true;
        }
    }
}
