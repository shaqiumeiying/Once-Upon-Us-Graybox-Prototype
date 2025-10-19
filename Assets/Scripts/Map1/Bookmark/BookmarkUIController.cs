using UnityEngine;

public class BookmarkUIController : MonoBehaviour
{
    public GameObject bookmarkUI;
    public MonoBehaviour playerMovement;
    private bool isOpen = false;

    void Start()
    {
        if (bookmarkUI) bookmarkUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Pressed Tab");
            ToggleBookmark();
        }
    }

    void ToggleBookmark()
    {
        isOpen = !isOpen;
        bookmarkUI.SetActive(isOpen);
        if (playerMovement) playerMovement.enabled = !isOpen;
    }
}
