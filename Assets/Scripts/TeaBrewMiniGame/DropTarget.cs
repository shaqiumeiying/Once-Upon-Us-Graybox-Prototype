using UnityEngine;

public class DropTarget : MonoBehaviour
{
    [Header("Accepted Items")]
    public string[] acceptedItems;   // e.g. Water, BakedLeaves

    [Header("Object Type")]
    public string objectType;        // e.g. Mortar, Teapot, Stove, Cup

    public void OnItemDropped(string itemName)
    {
        var game = TeaGameManager.Instance;
        var inv = FindObjectOfType<InventoryManagerTBM>();

        // Check if the dropped item is valid for this target
        bool acceptsItem = acceptedItems != null && System.Array.Exists(acceptedItems, x => x == itemName);

        switch (objectType)
        {
            // --- Mortar ---
            case "Mortar":
                if (acceptsItem && itemName == "TeaLeaves")
                {
                    Debug.Log("Tea leaves added to mortar.");
                    game.mortarHasLeaves = true;
                    inv.UseItem(itemName, 1);
                }
                break;

            // --- Stove ---
            case "Stove":
                // Bake mortar ¡ú stove
                if (game.mortarHasLeaves)
                {
                    Debug.Log("Mortar placed on stove ¡ª baking in progress...");
                    // TODO: trigger QTE 
                    game.mortarHasLeaves = false;
                    inv.AddItem("BakedLeaves", 1);
                    Debug.Log("Baked Leaves added to inventory!");
                }
                // Boil teapot ¡ú stove
                else if (game.teapotHasWater && game.teapotHasLeaves)
                {
                    Debug.Log("Teapot placed on stove ¡ª boiling water...");
                    // TODO: trigger minigame
                    game.teapotBoiled = true;
                    Debug.Log("Water boiled!");
                }
                break;

            // --- Teapot ---
            case "Teapot":
                if (acceptsItem)
                {
                    if (itemName == "Water")
                    {
                        Debug.Log("Water added to teapot.");
                        game.teapotHasWater = true;
                        inv.UseItem(itemName, 1);
                    }
                    else if (itemName == "BakedTeaLeaves")
                    {
                        Debug.Log("Dried leaves added to teapot.");
                        game.teapotHasLeaves = true;
                        inv.UseItem(itemName, 1);
                    }
                }
                break;

            // --- Cup ---
            case "Cup":
                if (game.teapotBoiled)
                {
                    Debug.Log("Tea poured into cup! Tea brewed successfully.");
                    game.cupHasTea = true;
                    // TODO: trigger final minigame
                }
                break;

            default:
                Debug.Log($"No rule for object type: {objectType}");
                break;
        }
    }
}
