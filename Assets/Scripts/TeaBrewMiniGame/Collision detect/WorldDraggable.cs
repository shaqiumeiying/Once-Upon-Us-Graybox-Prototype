using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class WorldDraggable : MonoBehaviour
{
    private Camera mainCam;
    private bool isDragging = false;
    private Vector3 offset;
    private float zDistance;

    //[Header("Item Info")]
    //public string itemName;

    [Header("Audio")]
    public AudioClip dingSound;      
    private AudioSource audioSource;

    private Vector3 startPos;          // starting position for auto-return
    public float returnSpeed = 3f;     

    void Start()
    {
        mainCam = Camera.main;
        startPos = transform.position;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
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

    // Public method called when process completes
    public void ReturnToStart(float delay = 500f)
    {
        if (dingSound != null)
            audioSource.PlayOneShot(dingSound);

        StartCoroutine(ReturnAfterDelay(delay));
    }

    IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 start = transform.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * returnSpeed;
            transform.position = Vector3.Lerp(start, startPos, t);
            yield return null;
        }
    }
}
