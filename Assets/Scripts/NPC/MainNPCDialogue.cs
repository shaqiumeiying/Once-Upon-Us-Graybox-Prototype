using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class DialogueNode
{
    [TextArea(2, 5)] public string npcLine;
    public string[] playerChoices;
}

public class MainNPCDialogue : MonoBehaviour
{
    [Header("Dialogue Setup")]
    public DialogueNode[] dialogueNodes;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public GameObject choicesContainer;
    public Button choiceButtonPrefab;
    public GameObject talkIndicator;

    private bool playerInRange;
    private int currentNode;

    // Reference to your PlayerMovement script
    private PlayerMovement playerMovement;

    void Start()
    {
        // Find the player movement controller by tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogWarning("PlayerMovement not found! Make sure your Player has tag 'Player'.");
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            if (!dialogueUI.activeSelf)
            {
                OpenDialogue();
            }
        }
    }

    void OpenDialogue()
    {
        dialogueUI.SetActive(true);
        talkIndicator.SetActive(false);
        ShowNode();

        // disable movement during dialogue
        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    void ShowNode()
    {
        DialogueNode node = dialogueNodes[currentNode];
        dialogueText.text = node.npcLine;

        // clear previous choices
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);

        // spawn new choice buttons
        foreach (string choice in node.playerChoices)
        {
            string capturedChoice = choice; // local variable capture
            Button b = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            b.GetComponentInChildren<TextMeshProUGUI>().text = capturedChoice;
            b.onClick.AddListener(() => OnChoiceSelected(capturedChoice));
        }
    }

    void OnChoiceSelected(string choice)
    {
        Debug.Log("Player chose: " + choice);

        currentNode++;

        if (currentNode >= dialogueNodes.Length)
        {
            CloseDialogue();
        }
        else
        {
            ShowNode();
        }
    }

    void CloseDialogue()
    {
        dialogueUI.SetActive(false);
        currentNode = 0;

        // re-enable movement
        if (playerMovement != null)
            playerMovement.enabled = true;

        // clean up leftover buttons
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
            talkIndicator.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            talkIndicator.SetActive(false);
            if (dialogueUI.activeSelf)
                CloseDialogue();
        }
    }
}