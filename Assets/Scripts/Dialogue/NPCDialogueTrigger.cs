using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager; // drag your DialogueManager object here
    public GameObject talkIndicator;        // optional: ¡°Press Space¡± bubble

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            if (!dialogueManager.dialogueUI.activeSelf)
            {
                dialogueManager.StartDialogue();

                if (talkIndicator != null)
                    talkIndicator.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (talkIndicator != null)
                talkIndicator.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (talkIndicator != null)
                talkIndicator.SetActive(false);
        }
    }
}
