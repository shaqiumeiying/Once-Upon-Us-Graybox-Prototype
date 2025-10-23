using UnityEngine;
using TMPro;

public class InventoryManagerTBM : MonoBehaviour
{
    [Header("Item Counts")]
    public int teaLeavesCount = 5;
    public int waterCount = 3;
    public int bakedLeavesCount = 0;

    [Header("UI References")]
    public TextMeshProUGUI teaLeavesText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI bakedLeavesText;

    [Header("Item Objects")]
    public GameObject teaLeavesItem;  
    public GameObject waterItem;
    public GameObject bakedLeavesItem;

    void Start()
    {
        if (bakedLeavesItem != null)
            bakedLeavesItem.SetActive(bakedLeavesCount > 0);

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
        else if (itemName == "BakedTeaLeaves")
        {
            bakedLeavesCount -= amount;
            if (bakedLeavesCount <= 0)
            {
                bakedLeavesCount = 0;
                if (bakedLeavesItem != null) Destroy(bakedLeavesItem);
            }
        }

        UpdateUI();
    }

    public void AddItem(string itemName, int amount)
    {
        if (itemName == "BakedLeaves")
        {
            bakedLeavesCount += amount;
            UpdateUI();

            if (bakedLeavesItem != null)
                bakedLeavesItem.SetActive(true);
        }
    }

    public void UpdateUI()
    {
        if (teaLeavesText != null)
            teaLeavesText.text = $"x{teaLeavesCount}";

        if (waterText != null)
            waterText.text = $"x{waterCount}";

        if (bakedLeavesText != null)
            bakedLeavesText.text = $"x{bakedLeavesCount}";
    }

}
