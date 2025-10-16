using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class DialogueNode
{
    [TextArea(2, 5)] public string npcLine;

    [Header("Player Choices")]
    public string[] playerChoices;

    [Header("NPC Reactions (matches choice index)")]
    [TextArea(2, 5)] public string[] npcReactions;

    [Header("Next Node Index (matches choice index)")]
    public int[] nextNodeIndexes;

    [Header("Optional Conditional Override")]
    public string requiredChoiceNPC;
    public string requiredChoiceValue;
    [TextArea(2, 5)] public string alternateLine;
}




public class MainNPCDialogue : MonoBehaviour
{
    [Header("Dialogue Text Setup")]
    public DialogueNode[] dialogueNodes;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.03f;
    public AudioClip typingSound; // sound

    [Header("Dialogue Button Setup")]
    public GameObject choicesContainer;
    public Button choiceButtonPrefab;

    [Header("Aesthetics Setup")]
    public GameObject talkIndicator;
    public GameObject continueArrow;

    private bool playerInRange;
    private int currentNode;
    private bool isTyping;
    private bool finishedTyping;

    void Update()
    {
        if (!playerInRange) return;

        // Only respond to Space if no buttons are active
        bool hasChoices = choicesContainer.transform.childCount > 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!dialogueUI.activeSelf)
            {
                OpenDialogue();
            }
            else if (!hasChoices)
            {
                if (finishedTyping)
                {
                    NextNode();
                }
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogueNodes[currentNode].npcLine;
                    finishedTyping = true;
                    ShowChoices();
                }
            }
        }
    }

    void OpenDialogue()
    {
        dialogueUI.SetActive(true);
        talkIndicator.SetActive(false);

        string lastChoice = GameStateManager.Instance?.GetDecision(gameObject.name);

        if (lastChoice == "Be kind")
        {
            // NPC reacts differently next time
            currentNode = 1; // maybe jump to a specific dialogue node
        }
        else if (lastChoice == "Be rude")
        {
            currentNode = 2;
        }
        else
        {
            currentNode = 0; // default dialogue
        }

        ShowNode();
    }


    void ShowNode()
    {
        DialogueNode node = dialogueNodes[currentNode];

        // check for conditional logic
        if (!string.IsNullOrEmpty(node.requiredChoiceNPC))
        {
            string prevDecision = GameStateManager.Instance?.GetDecision(node.requiredChoiceNPC);
            if (prevDecision == node.requiredChoiceValue && !string.IsNullOrEmpty(node.alternateLine))
            {
                node.npcLine = node.alternateLine;
            }
        }

        // clear old choices
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);

        finishedTyping = false;
        StartCoroutine(TypeLine(node.npcLine));
    }


    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        if (continueArrow != null)
            continueArrow.SetActive(false); // hide while typing

        int charCount = 0;
        foreach (char c in line)
        {
            dialogueText.text += c;
            charCount++;

            // play typing sound occasionally
            if (typingSound != null && charCount % 3 == 0)
                AudioSource.PlayClipAtPoint(typingSound, transform.position);

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        finishedTyping = true;

        if (continueArrow != null)
            StartCoroutine(BlinkArrow()); // start blinking when done

        ShowChoices();
    }

    IEnumerator BlinkArrow()
    {
        while (finishedTyping && continueArrow != null)
        {
            continueArrow.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            continueArrow.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }



    void ShowChoices()
    {
        DialogueNode node = dialogueNodes[currentNode];

        // Hide until finished typing
        choicesContainer.SetActive(finishedTyping);

        foreach (string choice in node.playerChoices)
        {
            string captured = choice;
            Button b = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            b.GetComponentInChildren<TextMeshProUGUI>().text = captured;
            b.onClick.AddListener(() => OnChoiceSelected(captured));
        }
    }

    void OnChoiceSelected(string choice)
    {
        Debug.Log("Player chose: " + choice);

        // Save player's decision
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SaveDecision(gameObject.name, choice);

        DialogueNode node = dialogueNodes[currentNode];
        int choiceIndex = System.Array.IndexOf(node.playerChoices, choice);

        if (choiceIndex >= 0)
            StartCoroutine(ShowReaction(node, choiceIndex));
    }

    IEnumerator ShowReaction(DialogueNode node, int choiceIndex)
    {
        // Clear choice buttons
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);

        // Display the NPC's reaction
        string reactionText = (choiceIndex < node.npcReactions.Length)
            ? node.npcReactions[choiceIndex]
            : "";

        yield return StartCoroutine(TypeLine(reactionText));

        yield return new WaitForSeconds(0.3f);

        // Move to the next dialogue node (branch)
        if (choiceIndex < node.nextNodeIndexes.Length)
        {
            int nextIndex = node.nextNodeIndexes[choiceIndex];
            if (nextIndex >= 0 && nextIndex < dialogueNodes.Length)
            {
                currentNode = nextIndex;
                ShowNode();
                yield break;
            }
        }

        // if -1 or out of range, close dialogue
        CloseDialogue();
    }


    void NextNode()
    {
        if (continueArrow != null)
            continueArrow.SetActive(false);

        currentNode++;
        if (currentNode < dialogueNodes.Length)
            ShowNode();
        else
            CloseDialogue();
    }


    void CloseDialogue()
    {
        dialogueUI.SetActive(false);
        currentNode = 0;
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
            if (dialogueUI.activeSelf) CloseDialogue();
        }
    }
}
