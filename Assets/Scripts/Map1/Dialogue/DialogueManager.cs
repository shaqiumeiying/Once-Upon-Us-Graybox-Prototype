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
    public TextMeshProUGUI npcNameText;

    [Header("Portraits")]
    public Image characterPortrait;
    public Sprite defaultPortrait;

    [Header("NPC State Info")]
    public string npcName;
    public DialogueLine[] firstMeetingLines;
    public DialogueLine[] repeatLines;

    [Header("Visuals")]
    public float typingSpeed = 0.03f;
    public GameObject continueArrow;
    public AudioClip typingSound;

    private int currentIndex = 0;
    private bool finishedTyping = false;

    private AudioSource typingSource;
    private float lastPlayTime = 0f;

    private PlayerMovement playerController;

    // --- new fields ---
    private bool skipRequested = false;
    private bool suppressSpaceThisFrame = false;

    void Start()
    {
        dialogueUI.SetActive(false);

        typingSource = gameObject.AddComponent<AudioSource>();
        typingSource.playOnAwake = false;
        typingSource.spatialBlend = 0f;

        playerController = FindObjectOfType<PlayerMovement>();
    }

    public void StartDialogue()
    {
        currentIndex = 0;
        dialogueUI.SetActive(true);

        if (playerController != null)
            playerController.enabled = false;

        if (characterPortrait != null && defaultPortrait != null)
            characterPortrait.sprite = defaultPortrait;

        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning("GameStateManager not found ¡ª creating one automatically.");
            new GameObject("GameStateManager").AddComponent<GameStateManager>();
        }

        string savedState = GameStateManager.Instance?.GetDecision(npcName);
        if (savedState == "CompletedMainDialogue")
            dialogueLines = repeatLines;
        else
            dialogueLines = firstMeetingLines;

        if (npcNameText != null)
            npcNameText.text = npcName;

        suppressSpaceThisFrame = true;
        StartCoroutine(EnableSpaceNextFrame());

        ShowLine();
    }

    void ShowLine()
    {
        StopAllCoroutines();
        dialogueText.text = "";
        finishedTyping = false;
        ClearChoices();

        if (continueArrow)
            continueArrow.SetActive(false);

        if (currentIndex < 0 || currentIndex >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueLines[currentIndex];

        suppressSpaceThisFrame = true;
        StartCoroutine(EnableSpaceNextFrame());

        StartCoroutine(TypeLine(line.npcLine));
        StartCoroutine(WaitThenShowChoices());
    }

    IEnumerator WaitThenShowChoices()
    {
        yield return new WaitUntil(() => finishedTyping);
        ShowChoices();
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        finishedTyping = false;
        skipRequested = false;

        for (int i = 0; i < line.Length; i++)
        {
            if (skipRequested)
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += line[i];

            if (typingSound && Time.time - lastPlayTime > 0.08f)
            {
                typingSource.pitch = Random.Range(0.95f, 1.05f);
                typingSource.PlayOneShot(typingSound);
                lastPlayTime = Time.time;
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        finishedTyping = true;
        skipRequested = false;
    }

    void ShowChoices()
    {
        DialogueLine line = dialogueLines[currentIndex];
        ClearChoices();

        if (line.choices == null || line.choices.Length == 0)
        {
            if (continueArrow) continueArrow.SetActive(true);
            return;
        }

        if (continueArrow) continueArrow.SetActive(false);

        foreach (var choice in line.choices)
        {
            Button b = Instantiate(choiceButtonPrefab, choicesContainer);
            b.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            b.onClick.AddListener(() => OnChoiceSelected(choice));
        }
    }

    void OnChoiceSelected(DialogueChoice choice)
    {
        ClearChoices();
        StopAllCoroutines();
        StartCoroutine(ShowResponse(choice));
    }

    IEnumerator ShowResponse(DialogueChoice choice)
    {
        dialogueText.text = "";

        if (string.IsNullOrEmpty(choice.npcResponse))
        {
            GoToNext(choice.nextIndex);
            yield break;
        }

        suppressSpaceThisFrame = true;
        StartCoroutine(EnableSpaceNextFrame());

        yield return StartCoroutine(TypeLine(choice.npcResponse));
        yield return new WaitUntil(() => finishedTyping);

        if (continueArrow != null)
            continueArrow.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (continueArrow != null)
            continueArrow.SetActive(false);

        GoToNext(choice.nextIndex);
    }

    void GoToNext(int nextIndex)
    {
        if (nextIndex >= 0 && nextIndex < dialogueLines.Length)
        {
            currentIndex = nextIndex;
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

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        dialogueUI.SetActive(false);
        if (continueArrow != null)
            continueArrow.SetActive(false);

        if (!string.IsNullOrEmpty(npcName))
            GameStateManager.Instance.SaveDecision(npcName, "CompletedMainDialogue");

        if (playerController != null)
            playerController.enabled = true;
    }

    void Update()
    {
        if (!dialogueUI.activeSelf) return;

        // Skip mid-typing
        if (!finishedTyping && !suppressSpaceThisFrame && Input.GetKeyDown(KeyCode.Space))
        {
            skipRequested = true;
            return;
        }

        // Continue after typing finished
        if (finishedTyping && Input.GetKeyDown(KeyCode.Space))
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

    IEnumerator EnableSpaceNextFrame()
    {
        yield return null;
        suppressSpaceThisFrame = false;
    }
}
