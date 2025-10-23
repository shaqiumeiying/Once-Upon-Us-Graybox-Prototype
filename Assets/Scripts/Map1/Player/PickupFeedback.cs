//using System.Collections;
//using UnityEngine;
//using TMPro;

//public class PickupFeedback : MonoBehaviour
//{
//    public AudioClip pickupSound;
//    public TextMeshProUGUI pickupText;
//    public InventoryManager inventory;

//    [Header("Bookmark UI References")]
//    public GameObject sugarIconUI; 

//    private Coroutine hideTextCoroutine;

//    public void ShowPickup(CollectibleItem item)
//    {
//        Debug.Log($"Picked up item: {item.itemName}");

//        // Skip inventory for sugar
//        if (inventory && !item.itemName.Equals("Sugar(Artifact)", System.StringComparison.OrdinalIgnoreCase))
//        {
//            Debug.Log("Added to inventory.");
//            inventory.AddItem(item.icon);
//        }

//        // Sound
//        if (pickupSound)
//            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

//        // Text feedback
//        if (pickupText)
//        {
//            pickupText.text = "Picked up " + item.itemName + "!";
//            if (hideTextCoroutine != null)
//                StopCoroutine(hideTextCoroutine);
//            hideTextCoroutine = StartCoroutine(HideTextAfterDelay(2f));
//        }

//        // Try activating sugar icon
//        if (item.itemName.Equals("Sugar(Artifact)", System.StringComparison.OrdinalIgnoreCase))
//        {
//            if (sugarIconUI != null)
//            {
//                Debug.Log("Found sugarIconUI reference, activating it now.");
//                sugarIconUI.SetActive(true);
//            }
//        }
//    }

//    private IEnumerator HideTextAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        pickupText.text = "";
//    }
//}
using System.Collections;
using UnityEngine;
using TMPro;

public class PickupFeedback : MonoBehaviour
{
    public AudioClip pickupSound;
    public TextMeshProUGUI pickupText;
    public InventoryManager inventory;

    [Header("Bookmark UI References")]
    public GameObject sugarIconUI;

    private Coroutine hideTextCoroutine;

    public void ShowPickup(CollectibleItem item)
    {
        Debug.Log($"Picked up item: {item.itemName}");

        // --- Save pickup globally ---
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SaveDecision(item.itemName, "PickedUp");
            Debug.Log($"[GameState] Recorded pickup: {item.itemName}");
        }
        else
        {
            Debug.LogWarning("GameStateManager not found when trying to save item state!");
        }

        // --- Add to inventory (skip sugar if handled elsewhere) ---
        if (inventory && !item.itemName.Equals("Sugar(Artifact)", System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Added to inventory.");
            inventory.AddItem(item.icon);
        }

        // --- Sound ---
        if (pickupSound)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // --- Text feedback ---
        if (pickupText)
        {
            pickupText.text = "Picked up " + item.itemName + "!";
            if (hideTextCoroutine != null)
                StopCoroutine(hideTextCoroutine);
            hideTextCoroutine = StartCoroutine(HideTextAfterDelay(2f));
        }

        // --- Activate sugar icon in bookmark UI ---
        if (item.itemName.Equals("Sugar(Artifact)", System.StringComparison.OrdinalIgnoreCase))
        {
            if (sugarIconUI != null)
            {
                Debug.Log("Found sugarIconUI reference.");
                sugarIconUI.SetActive(true);
            }
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pickupText.text = "";
    }
}
