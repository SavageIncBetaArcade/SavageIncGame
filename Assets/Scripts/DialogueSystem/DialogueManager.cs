using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Text controlText;

    private Queue<string> dialogue;
    private float fadeDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = new Queue<string>();

        GameObject.Find("DialogueBox").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("ControlBox").GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void StartDialogue(string name, string[] sentences, string textType)
    {
        GameObject.Find("DialogueBox").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("ControlBox").GetComponent<CanvasGroup>().alpha = 0f;

        nameText.text = name;

        dialogue.Clear();

        foreach (string sentence in sentences)
        {
            dialogue.Enqueue(sentence);
        }

        DisplayDialogueSentence(textType);
    }

    public void DisplayControls(string name, string[] sentences, string textType)
    {
        GameObject.Find("DialogueBox").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("ControlBox").GetComponent<CanvasGroup>().alpha = 0f;

        dialogue.Clear();

        foreach (string sentence in sentences)
        {
            dialogue.Enqueue(sentence);
        }

        DisplayControlSentence(textType);
    }

    public void DisplayDialogueSentence(string textType)
    {
        if (dialogue.Count == 0)
        {
            StartCoroutine(EndDialogue(textType));
            return;
        }

        string sentence = dialogue.Dequeue();

        StartCoroutine(Fade(0f, 1f, fadeDuration, textType));
        StartCoroutine(TypeSentence(sentence, textType));
        StartCoroutine(DialogueDelay(textType));
    }

    public void DisplayControlSentence(string textType)
    {
        if (dialogue.Count == 0)
        {
            StartCoroutine(EndDialogue(textType));
            return;
        }

        string sentence = dialogue.Dequeue();

        StartCoroutine(Fade(0f, 1f, fadeDuration, textType));
        StartCoroutine(TypeSentence(sentence, textType));
        StartCoroutine(DialogueDelay(textType));
    }

    IEnumerator TypeSentence(string sentence, string textType)
    {
        if (textType == "dialogue")
        {
            dialogueText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;

                yield return null;
            }
        }
        else if (textType == "control")
        {
            controlText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                controlText.text += letter;

                yield return null;
            }
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

    IEnumerator DialogueDelay(string textType)
    {
        yield return new WaitForSeconds(5);

        if (textType == "dialogue")
        {
            DisplayDialogueSentence(textType);
        }
        else if (textType == "control")
        {
            DisplayControlSentence(textType);
        }
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
