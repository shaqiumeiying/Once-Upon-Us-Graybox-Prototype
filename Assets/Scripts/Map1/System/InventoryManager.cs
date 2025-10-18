using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<Image> slots = new List<Image>();
    public Sprite defaultIcon;

    // Store collected item icons
    private int currentIndex = 0;

    public void AddItem(Sprite itemIcon)
    {
        if (currentIndex < slots.Count)
        {
            slots[currentIndex].sprite = itemIcon;
            slots[currentIndex].color = Color.white; // make sure it's visible
            currentIndex++;
        }
        else
        {
            Debug.Log("Inventory full!");
        }
    }
}
