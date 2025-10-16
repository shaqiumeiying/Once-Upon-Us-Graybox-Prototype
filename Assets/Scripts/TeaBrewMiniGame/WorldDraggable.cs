//using UnityEngine;
//[RequireComponent(typeof(Collider))]
//public class WorldDraggable : MonoBehaviour
//{
//    private Camera mainCam;
//    private bool isDragging = false;
//    private Vector3 offset;
//    private float zDistance;

//    [Header("Item Info")]
//    public string itemName;
//    // e.g. "Mortar" or "Teapot"
//    [Header("Snap Settings")]
//    public float snapDistance = 1.0f; // how close it needs to be to snap
//    public Vector3 snapOffset = new Vector3(0, 0.5f, 0); // offset when snapping onto target

//    void Start()
//    {
//        mainCam = Camera.main;
//    }

//    void OnMouseDown()
//    {
//        isDragging = true;
//        // store z distance
//        zDistance = Vector3.Distance(transform.position, mainCam.transform.position);
//        // store offset
//        Vector3 mousePos = Input.mousePosition;
//        mousePos.z = zDistance;
//        offset = transform.position - mainCam.ScreenToWorldPoint(mousePos);
//    }

//    void OnMouseDrag()
//    {
//        if (!isDragging) return;
//        Vector3 mousePos = Input.mousePosition;
//        mousePos.z = zDistance;
//        Vector3 newPos = mainCam.ScreenToWorldPoint(mousePos) + offset;
//        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
//    }

//    void OnMouseUp()
//    {
//        isDragging = false;
//        // Try to detect DropTarget (like Stove or Cup)
//        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
//        if (Physics.Raycast(ray, out RaycastHit hit))
//        {
//            DropTarget target = hit.collider.GetComponent<DropTarget>();
//            if (target != null)
//            {
//                // snap if within range
//                if (Vector3.Distance(transform.position, target.transform.position) < snapDistance)
//                {
//                    transform.position = target.transform.position + snapOffset;
//                    Debug.Log($"{itemName} snapped onto {target.objectType}");
//                } // Call target logic (same as inventory drops)
//                target.OnItemDropped(itemName);
//                return;
//            }
//        }
//        Debug.Log($"{itemName} released ¡ª no valid target found.");
//    }

//}
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldDraggable : MonoBehaviour
{
    private Camera mainCam;
    private bool isDragging = false;
    private Vector3 offset;
    private float zDistance;

    [Header("Item Info")]
    public string itemName; // e.g. "Mortar" or "Teapot"

    void Start()
    {
        mainCam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;

        // Store how far away the object is from the camera
        zDistance = Vector3.Distance(transform.position, mainCam.transform.position);

        // Store offset so we don't jump when grabbing
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zDistance;
        offset = transform.position - mainCam.ScreenToWorldPoint(mousePos);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zDistance;

        // Move with mouse, but keep Z the same
        Vector3 newPos = mainCam.ScreenToWorldPoint(mousePos) + offset;
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    void OnMouseUp()
    {
        isDragging = false;
        // No raycast needed ¡ª triggers will handle interactions
    }
}
