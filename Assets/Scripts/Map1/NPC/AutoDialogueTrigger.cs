using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AutoDialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private bool hasTriggered = false;

    void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();

        // make sure collider is trigger
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            // start dialogue automatically
            if (dialogueManager != null)
                dialogueManager.StartDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // optional: allow retrigger after leaving
            hasTriggered = false;
        }
    }
}
