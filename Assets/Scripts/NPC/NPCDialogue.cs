using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public string[] dialogueLines;
    public TextMeshPro dialogueText3D;

    private InteractionIndicator indicator;
    private bool dialogueActive;
    private int currentLine = 0;

    void Start()
    {
        indicator = GetComponent<InteractionIndicator>();
        if (dialogueText3D != null)
            dialogueText3D.text = ""; // start blank
    }

    void Update()
    {
        if (indicator == null) return;

        if (indicator.IsPlayerInRange() && Input.GetKeyDown(KeyCode.Space))
        {
            if (!dialogueActive)
            {
                StartDialogue();
            }
            else
            {
                ContinueDialogue();
            }
        }

        // auto-hide if player leaves trigger area
        if (dialogueActive && !indicator.IsPlayerInRange())
        {
            EndDialogue();
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        currentLine = 0;

        if (indicator.indicator != null)
            indicator.indicator.SetActive(false);

        dialogueText3D.text = dialogueLines[currentLine];
    }

    void ContinueDialogue()
    {
        currentLine++;

        if (currentLine < dialogueLines.Length)
        {
            dialogueText3D.text = dialogueLines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        currentLine = 0;
        dialogueText3D.text = "";

        // Only show indicator again if player is still in range
        if (indicator.IsPlayerInRange() && indicator.indicator != null)
            indicator.indicator.SetActive(true);
    }
}
