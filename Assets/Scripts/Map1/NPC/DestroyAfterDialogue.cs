using UnityEngine;
using System.Collections;

public class DestroyAfterDialogue : MonoBehaviour
{
    [Header("Assign the DialogueManager on this NPC")]
    public DialogueManager dialogueManager;

    private bool hasDestroyed = false;

    void Start()
    {
        if (dialogueManager == null)
            dialogueManager = GetComponent<DialogueManager>();

        if (dialogueManager != null)
            StartCoroutine(WatchForDialogueEnd());
        else
            Debug.LogWarning($"[DestroyAfterDialogue] {gameObject.name} has no DialogueManager assigned.");
    }

    private IEnumerator WatchForDialogueEnd()
    {
        yield return new WaitUntil(() => dialogueManager.dialogueUI.activeSelf);

        yield return new WaitUntil(() => !dialogueManager.dialogueUI.activeSelf);

        yield return new WaitForSeconds(0.5f);

        if (!hasDestroyed)
        {
            hasDestroyed = true;
            Destroy(gameObject);
        }
    }
}
