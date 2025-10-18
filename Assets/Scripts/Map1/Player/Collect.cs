using UnityEngine;

public class Collect : MonoBehaviour
{
    private GameObject currentCollectible;

    void Update()
    {
        // Press Space to collect when near a collectible
        if (currentCollectible != null && Input.GetKeyDown(KeyCode.Space))
        {
            CollectItem(currentCollectible);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            currentCollectible = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectible") && currentCollectible == other.gameObject)
        {
            currentCollectible = null;
        }
    }

    void CollectItem(GameObject collectible)
    {
        var item = collectible.GetComponent<CollectibleItem>();
        var feedback = GetComponent<PickupFeedback>();

        if (item != null)
        {
            Debug.Log("Picked up " + item.itemName);

            if (feedback != null)
                feedback.ShowPickup(item);  // display pickup message, play sound, etc.

            Destroy(collectible);
        }

        currentCollectible = null;
    }
}
