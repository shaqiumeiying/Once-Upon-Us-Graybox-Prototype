using UnityEngine;

public class Collect : MonoBehaviour
{
    private GameObject currentCollectible;

    void Update()
    {
        // If player is near a collectible and presses Space
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

            // show indicator (if exists)
            var indicator = other.transform.Find("InteractIndicator");
            if (indicator != null)
                indicator.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            // hide indicator (if exists)
            var indicator = other.transform.Find("InteractIndicator");
            if (indicator != null)
                indicator.gameObject.SetActive(false);

            // reset collectible reference
            if (currentCollectible == other.gameObject)
                currentCollectible = null;
        }
    }

    void CollectItem(GameObject collectible)
    {
        Debug.Log("Picked up " + collectible.name);

        var item = collectible.GetComponent<CollectibleItem>();
        var feedback = GetComponent<PickupFeedback>();

        if (feedback != null && item != null)
        {
            feedback.ShowPickup(item);
        }

        Destroy(collectible);
        currentCollectible = null;
    }

}
