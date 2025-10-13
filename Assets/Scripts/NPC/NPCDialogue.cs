using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public string[] dialogueLines;
    public TextMeshPro dialogueText3D;
    public GameObject talkIndicator;     // "..." object
    private bool playerInRange;
    private int currentLine = 0;
    private bool dialogueActive = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            if (!dialogueActive)
            {
                dialogueActive = true;
                dialogueText3D.text = dialogueLines[currentLine];
                talkIndicator.SetActive(false);
            }
            else
            {
                currentLine++;
                if (currentLine < dialogueLines.Length)
                {
                    dialogueText3D.text = dialogueLines[currentLine];
                }
                else
                {
                    dialogueText3D.text = "";
                    dialogueActive = false;
                    currentLine = 0;
                    talkIndicator.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            talkIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueText3D.text = "";
            dialogueActive = false;
            currentLine = 0;
            talkIndicator.SetActive(false);
        }
    }
}
