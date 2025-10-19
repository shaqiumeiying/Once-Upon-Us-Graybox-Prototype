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

    [Header("Portraits")]
    public Image characterPortrait;
    public Sprite defaultPortrait;

    [Header("NPC State Info")]
    public string npcName;                   
    public DialogueLine[] firstMeetingLines;  // first time dialogue
    public DialogueLine[] repeatLines;        // shorter repeat dialogue

    [Header("Visuals")]
    public float typingSpeed = 0.03f;
    public GameObject continueArrow;
    public AudioClip typingSound;

    private int currentIndex = 0;
    private bool finishedTyping = false;

    private AudioSource typingSource;
    private float lastPlayTime = 0f;

    private PlayerMovement playerController;

    void Start()
    {
        dialogueUI.SetActive(false);

        // Create a reusable AudioSource for typing
        typingSource = gameObject.AddComponent<AudioSource>();
        typingSource.playOnAwake = false;
        typingSource.spatialBlend = 0f; // 2D UI sound

        // Find player movement controller
        playerController = FindObjectOfType<PlayerMovement>();
    }

    // Called externally when dialogue starts
    public void StartDialogue()
    {
        currentIndex = 0;
        dialogueUI.SetActive(true);
 
        // Disable player control
        if (playerController != null)
            playerController.enabled = false;

        if (characterPortrait != null && defaultPortrait != null)
            characterPortrait.sprite = defaultPortrait;

        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning("GameStateManager not found ¡ª creating one automatically.");
            new GameObject("GameStateManager").AddComponent<GameStateManager>();
        }

        // --- Determine which dialogue to use ---
        string savedState = GameStateManager.Instance?.GetDecision(npcName);

        if (savedState == "CompletedMainDialogue")
        {
            dialogueLines = repeatLines;
            // Debug.Log($"[DialogueManager] {npcName} using REPEAT dialogue.");
        }
        else
        {
            dialogueLines = firstMeetingLines;
            // Debug.Log($"[DialogueManager] {npcName} using FIRST meeting dialogue.");
        }

        ShowLine();
    }

    void ShowLine()
    {
        StopAllCoroutines();
        dialogueText.text = "";
        finishedTyping = false;
        ClearChoices();

        if (currentIndex < 0 || currentIndex >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueLines[currentIndex];
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
        foreach (char c in line)
        {
            dialogueText.text += c;

            if (typingSound && Time.time - lastPlayTime > 0.08f)
            {
                typingSource.pitch = Random.Range(0.95f, 1.05f);
                typingSource.PlayOneShot(typingSound);
                lastPlayTime = Time.time;
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        finishedTyping = true;
    }

    void ShowChoices()
    {
        DialogueLine line = dialogueLines[currentIndex];

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

        // If the choice has no NPC response, skip typing
        if (string.IsNullOrEmpty(choice.npcResponse))
        {
            GoToNext(choice.nextIndex);
            yield break;
        }

        yield return StartCoroutine(TypeLine(choice.npcResponse));
        yield return new WaitUntil(() => finishedTyping);

        if (continueArrow != null)
            continueArrow.SetActive(true);

        // Wait for player to continue
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

        // Wait for player to press Space to close
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        dialogueUI.SetActive(false);
        if (continueArrow != null)
            continueArrow.SetActive(false);

        // Save dialogue state
        if (!string.IsNullOrEmpty(npcName))
        {
            GameStateManager.Instance.SaveDecision(npcName, "CompletedMainDialogue");
        }

        // Re-enable player control
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
