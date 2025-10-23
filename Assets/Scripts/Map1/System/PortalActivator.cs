using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalActivator : MonoBehaviour
{
    public GameObject portal;

    [Header("Required Items")]
    public string requiredItem1 = "TeaLeaves";
    public string requiredItem2 = "SpringWater";

    private bool portalActive = false;

    void Start()
    {
        // Hide the portal effect at start
        if (portal != null)
            portal.SetActive(false);
    }

    void Update()
    {
        // Make sure GameStateManager exists
        if (GameStateManager.Instance == null) return;

        // Check whether both items were picked up
        bool hasLeaves = GameStateManager.Instance.GetDecision(requiredItem1) == "PickedUp";
        bool hasWater = GameStateManager.Instance.GetDecision(requiredItem2) == "PickedUp";

        if (!portalActive && hasLeaves && hasWater)
        {
            ActivatePortal();
        }
    }

    void ActivatePortal()
    {
        portalActive = true;

        if (portal != null)
            portal.SetActive(true);

        Debug.Log("Portal activated! Player can now enter the next scene.");
    }

}

