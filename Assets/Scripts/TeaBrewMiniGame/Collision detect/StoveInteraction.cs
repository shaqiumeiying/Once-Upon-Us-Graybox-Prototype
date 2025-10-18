using UnityEngine;

public class StoveInteraction : MonoBehaviour
{
    private TeaBrewManager game;
    private InventoryManagerTBM inv;

    private void Start()
    {
        game = TeaBrewManager.Instance;
        inv = FindObjectOfType<InventoryManagerTBM>();
        // Debug.Log($"[DEBUG] Found inventory: {inv != null}");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"[DEBUG] OnTriggerEnter: {other.name}");

        // Mortar on stove
        if (other.CompareTag("Mortar") && game.mortarHasLeaves)
        {
            Debug.Log("Mortar placed on stove — baking in progress...");
            // TODO: trigger QTE here
            game.mortarHasLeaves = false;
            game.mortarBaked = true;
            Debug.Log("Baked leaves ready!");
            inv.AddItem("BakedLeaves", 1);
        }

        // 🫖 Teapot on stove
        if (other.CompareTag("Teapot") && game.teapotHasWater && game.teapotHasLeaves)
        {
            Debug.Log("Teapot placed on stove — boiling water...");
            // TODO: trigger Minigame here
            game.teapotBoiled = true;
            Debug.Log("Tea boiled!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log($"[DEBUG] OnTriggerExit: {other.name}");
    }
}
