using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Setup")]
    public DialogueLine[] dialogueLines;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public Transform choicesContainer;
    public Button choiceButtonPrefab;

    [Header("Visuals")]
    public float typingSpeed = 0.03f;
    public GameObject continueArrow;
    public AudioClip typingSound;

    private int currentIndex = 0;
    private bool finishedTyping = false;

    void Start()
    {
        dialogueUI.SetActive(false);
    }

    public void StartDialogue()
    {
        currentIndex = 0;
        dialogueUI.SetActive(true);
        ShowLine();
    }

    void ShowLine()
    {
        StopAllCoroutines();
        dialogueText.text = "";
        finishedTyping = false;
        ClearChoices();

        DialogueLine line = dialogueLines[currentIndex];
        StartCoroutine(TypeLine(line.npcLine));
    }

    IEnumerator TypeLine(string line)
    {
        foreach (char c in line)
        {
            dialogueText.text += c;
            if (typingSound) AudioSource.PlayClipAtPoint(typingSound, transform.position);
            yield return new WaitForSeconds(typingSpeed);
        }
        finishedTyping = true;
        ShowChoices();
    }

    void ShowChoices()
    {
        DialogueLine line = dialogueLines[currentIndex];

        if (line.choices == null || line.choices.Length == 0)
        {
            // no choices ¡ú press space to continue
            if (continueArrow) continueArrow.SetActive(true);
        }
        else
        {
            if (continueArrow) continueArrow.SetActive(false);

            foreach (var choice in line.choices)
            {
                Button b = Instantiate(choiceButtonPrefab, choicesContainer);
                b.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
                b.onClick.AddListener(() => OnChoiceSelected(choice));
            }
        }
    }

    void OnChoiceSelected(DialogueChoice choice)
    {
        ClearChoices();
        StartCoroutine(ShowResponse(choice));
    }

    IEnumerator ShowResponse(DialogueChoice choice)
    {
        dialogueText.text = "";
        yield return StartCoroutine(TypeLine(choice.npcResponse));
        yield return new WaitUntil(() => finishedTyping);

        if (choice.nextIndex >= 0 && choice.nextIndex < dialogueLines.Length)
        {
            currentIndex = choice.nextIndex;
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void ClearChoices()
    {
        foreach (Transform c in choicesContainer)
            Destroy(c.gameObject);
    }

    //public void EndDialogue()
    //{
    //    dialogueUI.SetActive(false);
    //    ClearChoices();
    //}

    public void EndDialogue()
    {
        StartCoroutine(WaitThenClose());
    }

    IEnumerator WaitThenClose()
    {
        if (continueArrow != null)
            continueArrow.SetActive(true);

        ClearChoices();
        // wait until player presses Space
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        dialogueUI.SetActive(false);

        if (continueArrow != null)
            continueArrow.SetActive(false);
    }


    void Update()
    {
        if (dialogueUI.activeSelf && finishedTyping && Input.GetKeyDown(KeyCode.Space))
        {
            DialogueLine line = dialogueLines[currentIndex];
            if (line.choices == null || line.choices.Length == 0)
            {
                if (currentIndex + 1 < dialogueLines.Length)
                {
                    currentIndex++;
                    ShowLine();
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }
}
