using System.Collections;
using UnityEngine;
using TMPro;

public class PickupFeedback : MonoBehaviour
{
    public AudioClip pickupSound;
    public TextMeshProUGUI pickupText;
    public InventoryManager inventory;

    private Coroutine hideTextCoroutine;

    // Called manually when player picks up an item
    public void ShowPickup(CollectibleItem item)
    {
        if (inventory)
            inventory.AddItem(item.icon);

        if (pickupSound)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        if (pickupText)
        {
            pickupText.text = "Picked up " + item.itemName + "!";
            if (hideTextCoroutine != null)
                StopCoroutine(hideTextCoroutine);
            hideTextCoroutine = StartCoroutine(HideTextAfterDelay(2f));
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pickupText.text = "";
    }
}
