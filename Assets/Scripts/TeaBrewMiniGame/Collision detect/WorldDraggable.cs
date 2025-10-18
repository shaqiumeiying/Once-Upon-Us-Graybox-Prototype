using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldDraggable : MonoBehaviour
{
    private Camera mainCam;
    private bool isDragging = false;
    private Vector3 offset;
    private float zDistance;

    void Start()
    {
        mainCam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;

        zDistance = Vector3.Distance(transform.position, mainCam.transform.position);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zDistance;
        offset = transform.position - mainCam.ScreenToWorldPoint(mousePos);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zDistance;

        Vector3 newPos = mainCam.ScreenToWorldPoint(mousePos) + offset;
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
