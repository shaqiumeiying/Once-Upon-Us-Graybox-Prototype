using UnityEngine;

public class StoveInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<InteractableState>();
        if (item == null) return;

        // Mortar placed on stove �� start baking
        if (other.CompareTag("Mortar"))
        {
            item.isBaking = true;
            Debug.Log("Mortar placed on stove �� baking in progress...");
        }

        // Teapot placed on stove �� start boiling
        else if (other.CompareTag("Teapot"))
        {
            item.isBoiling = true;
            Debug.Log("Teapot placed on stove �� boiling in progress...");
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
