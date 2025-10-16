using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemTBM : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Item Settings")]
    public string itemName = "TeaLeaves";   // Name must match DropTarget.acceptedItem
    public int useAmount = 1;               // How many units to consume per drop

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private Camera mainCam;                 // to raycast into 3D world

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        mainCam = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
        canvasGroup.alpha = 0.6f; // semi-transparent while dragging
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position; // follow mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore UI appearance
        rectTransform.position = startPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // --- Detect 3D object under mouse when released ---
        if (mainCam == null) mainCam = Camera.main;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            DropTarget target = hit.collider.GetComponent<DropTarget>();
            if (target != null)
            {
                // Found a DropTarget in the world!
                target.OnItemDropped(itemName);
                Debug.Log($"Dropped {itemName} on {target.gameObject.name}");
            }
            else
            {
                Debug.Log("No valid DropTarget hit.");
            }
        }
        else
        {
            Debug.Log("Nothing hit by raycast.");
        }
    }
}
