using UnityEngine;

public class StoveInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<InteractableState>();
        if (item == null) return;

        // Mortar placed on stove ¡ú start baking
        if (other.CompareTag("Mortar"))
        {
            item.isBaking = true;
            Debug.Log("Mortar placed on stove ¡ª baking in progress...");
        }

        // Teapot placed on stove ¡ú start boiling
        else if (other.CompareTag("Teapot"))
        {
            item.isBoiling = true;
            Debug.Log("Teapot placed on stove ¡ª boiling in progress...");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var item = other.GetComponent<InteractableState>();
        if (item == null) return;

        if (other.CompareTag("Mortar"))
        {
            item.isBaking = false;
            Debug.Log("Mortar removed from stove.");
        }
        else if (other.CompareTag("Teapot"))
        {
            item.isBoiling = false;
            Debug.Log("Teapot removed from stove.");
        }
    }
}
