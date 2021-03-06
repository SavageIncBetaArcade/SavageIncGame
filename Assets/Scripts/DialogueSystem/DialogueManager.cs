﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text NameText;
    public Text DialogueText;
    public Text ControlText;

    private Queue<string> dialogue;
    private float fadeDuration = 0.5f;

    void Start()
    {
        dialogue = new Queue<string>();

        GameObject.Find("DialogueBox").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("ControlBox").GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void DisplayDialogue(string name, string[] sentences, string textType, DialogueTrigger trigger)
    {
        GameObject.Find("DialogueBox").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("ControlBox").GetComponent<CanvasGroup>().alpha = 0f;

        NameText.text = name;

        dialogue.Clear();

        foreach (string sentence in sentences)
        {
            dialogue.Enqueue(sentence);
        }

        StartCoroutine(DisplaySentence(textType, trigger));
    }

    public void DisplayControls(string[] sentences, string textType, DialogueTrigger trigger)
    {
        GameObject.Find("DialogueBox").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("ControlBox").GetComponent<CanvasGroup>().alpha = 0f;

        dialogue.Clear();

        foreach (string sentence in sentences)
        {
            dialogue.Enqueue(sentence);
        }

        StartCoroutine(DisplaySentence(textType, trigger));
    }

    IEnumerator DisplaySentence(string textType, DialogueTrigger trigger)
    {
        if (dialogue.Count == 0)
        {
            bool triggerState = trigger.getState();

            if (triggerState == true)
            {
                StartCoroutine(DialogueDelay(textType, 1, trigger));
                yield return null;
            }
            else
            {
                StartCoroutine(EndDialogue(textType));
                //  trigger.setState(false);

                yield return null;
            }
        }

        if (dialogue.Count > 0)
        {
            string sentence = dialogue.Dequeue();

            StartCoroutine(Fade(0f, 1f, fadeDuration, textType));
            StartCoroutine(TypeSentence(sentence, textType));
            StartCoroutine(DialogueDelay(textType, 5, trigger));
        }
    }

    IEnumerator Fade(float start, float end, float duration, string textType)
    {
        for (float i = 0f; i < duration; i += Time.deltaTime)
        {
            if (textType == "dialogue")
            {
                GameObject.Find("DialogueBox").GetComponent<CanvasGroup>().alpha = Mathf.Lerp(start, end, i / duration);
            }
            else if (textType == "control")
            {
                GameObject.Find("ControlBox").GetComponent<CanvasGroup>().alpha = Mathf.Lerp(start, end, i / duration);
            }

            yield return null;
        }

        if (textType == "dialogue")
        {
            GameObject.Find("DialogueBox").GetComponent<CanvasGroup>().alpha = end;
        }
        else if (textType == "control")
        {
            GameObject.Find("ControlBox").GetComponent<CanvasGroup>().alpha = end;
        }
    }

    IEnumerator TypeSentence(string sentence, string textType)
    {
        if (textType == "dialogue")
        {
            DialogueText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                DialogueText.text += letter;

                yield return null;
            }
        }
        else if (textType == "control")
        {
            ControlText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                ControlText.text += letter;

                yield return null;
            }
        }
    }

    IEnumerator DialogueDelay(string textType, int delayTime, DialogueTrigger trigger)
    {
        yield return new WaitForSeconds(delayTime);

        StartCoroutine(DisplaySentence(textType, trigger));
    }

    IEnumerator EndDialogue(string textType)
    {
        float currentAlpha = 0f;

        if (textType == "dialogue")
        {
            currentAlpha = GameObject.Find("DialogueBox").GetComponent<CanvasGroup>().alpha;
        }
        else if (textType == "control")
        {
            currentAlpha = GameObject.Find("ControlBox").GetComponent<CanvasGroup>().alpha;
        }

        StartCoroutine(Fade(currentAlpha, 0f, fadeDuration, textType));

        yield return null;
    }
}
