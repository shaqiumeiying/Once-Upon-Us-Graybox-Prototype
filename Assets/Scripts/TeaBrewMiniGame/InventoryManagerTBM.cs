using UnityEngine;
using TMPro;

public class InventoryManagerTBM : MonoBehaviour
{
    [Header("Item Counts")]
    public int teaLeavesCount = 5;
    public int waterCount = 3;

    [Header("UI References")]
    public TextMeshProUGUI teaLeavesText;
    public TextMeshProUGUI waterText;

    [Header("Item Objects")]
    public GameObject teaLeavesItem;  
    public GameObject waterItem; 

    void Start()
    {
        UpdateUI();
    }

    public void UseItem(string itemName, int amount)
    {
        if (itemName == "TeaLeaves")
        {
            teaLeavesCount -= amount;
            if (teaLeavesCount <= 0)
            {
                teaLeavesCount = 0;
                if (teaLeavesItem != null) Destroy(teaLeavesItem);
            }
        }
        else if (itemName == "Water")
        {
            waterCount -= amount;
            if (waterCount <= 0)
            {
                waterCount = 0;
                if (waterItem != null) Destroy(waterItem);
            }
        }

        UpdateUI();
    }

    public void AddItem(string itemName, int amount)
    {
        if (itemName == "BakedLeaves")
        {
            teaLeavesCount += amount; // or add a separate bakedLeavesCount if you prefer
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (teaLeavesText != null)
            teaLeavesText.text = $"x{teaLeavesCount}";

        if (waterText != null)
            waterText.text = $"x{waterCount}";
    }
}
