using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BookmarkItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("This Slot's Data")]
    public Sprite icon;
    public string itemName;
    [TextArea] public string description;

    [Header("Header References (assign from the same panel)")]
    public Image headerImage;
    public TMP_Text headerName;
    public TMP_Text headerDescription;

    [Header("Optional: Default header state when not hovering")]
    public Sprite defaultIcon;
    public string defaultName = "";
    [TextArea] public string defaultDescription = "";

    void Start()
    {
        ClearHeader();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show info when hovered
        if (headerImage) headerImage.sprite = icon;
        if (headerName) headerName.text = itemName;
        if (headerDescription) headerDescription.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Clear or reset when mouse leaves the icon
        ClearHeader();
    }

    void ClearHeader()
    {
        if (headerImage) headerImage.sprite = defaultIcon;
        if (headerName) headerName.text = defaultName;
        if (headerDescription) headerDescription.text = defaultDescription;
    }
}
