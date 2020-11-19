using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    private Queue<string> dialogue;
    private float fadeDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = new Queue<string>();

        GetComponent<CanvasGroup>().alpha = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(string name, string[] sentences)
    {
        nameText.text = name;

        dialogue.Clear();

        foreach (string sentence in sentences)
        {
            dialogue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialogue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogue.Dequeue();
        StopAllCoroutines();

        if (GetComponent<CanvasGroup>().alpha < 1f)
        {
            StartCoroutine(Fade(0f, 1f, fadeDuration));
        }

        StartCoroutine(TypeSentence(sentence));
        StartCoroutine(DialogueDelay());
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            yield return null;
        }
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        for (float i = 0f; i < duration; i += Time.deltaTime)
        {
            GetComponent<CanvasGroup>().alpha = Mathf.Lerp(start, end, i / duration);
            yield return null;
        }

        GetComponent<CanvasGroup>().alpha = end;
    }

    IEnumerator DialogueDelay()
    {
        yield return new WaitForSeconds(5);
        DisplayNextSentence();
    }

    void EndDialogue()
    {
        StartCoroutine(Fade(1f, 0f, fadeDuration));
    }
}
