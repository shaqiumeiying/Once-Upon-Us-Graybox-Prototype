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
    private bool isTyping = false;
    private bool finishedTyping = false;

    // --- new sound system ---
    private AudioSource typingSource;
    private float lastPlayTime = 0f;

    // --- players ---
    private PlayerMovement playerController;

    void Start()
    {
        dialogueUI.SetActive(false);

        // Create reusable AudioSource for typing
        typingSource = gameObject.AddComponent<AudioSource>();
        typingSource.playOnAwake = false;
        typingSource.spatialBlend = 0f; // 2D sound for UI

        // find player controller
        playerController = FindObjectOfType<PlayerMovement>();
    }

    public void StartDialogue()
    {
        currentIndex = 0;
        dialogueUI.SetActive(true);

        if (playerController != null)
            playerController.enabled = false; // disable movement

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

            // Play controlled typing sound
            if (typingSound && Time.time - lastPlayTime > 0.05f)
            {
                typingSource.pitch = Random.Range(0.95f, 1.05f);
                typingSource.PlayOneShot(typingSound);
                lastPlayTime = Time.time;
            }

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

    public void EndDialogue()
    {
        StartCoroutine(WaitThenClose());
    }

    IEnumerator WaitThenClose()
    {
        if (continueArrow != null)
            continueArrow.SetActive(true);

        ClearChoices();

        // Wait until player presses Space to close
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        dialogueUI.SetActive(false);

        if (continueArrow != null)
            continueArrow.SetActive(false);

        // re-enable player movement
        if (playerController != null)
            playerController.enabled = true;
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
