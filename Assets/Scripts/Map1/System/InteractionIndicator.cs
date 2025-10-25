//using UnityEngine;

//public class InteractionIndicator : MonoBehaviour
//{
//    [Header("Indicator Setup")]
//    public GameObject indicator;          
//    public string playerTag = "Player";
//    public bool requireKeyPress = false;  // if true, only shows when player presses key
//    public KeyCode triggerKey = KeyCode.Space;

//    private bool playerInRange;

//    void Start()
//    {
//        if (indicator != null)
//            indicator.SetActive(false);
//    }

//    void Update()
//    {
//        if (!playerInRange || indicator == null) return;

//        if (requireKeyPress)
//        {
//            indicator.SetActive(Input.GetKey(triggerKey));
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag(playerTag))
//        {
//            playerInRange = true;
//            if (!requireKeyPress && indicator != null)
//                indicator.SetActive(true);
//        }
//    }

//    void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag(playerTag))
//        {
//            playerInRange = false;
//            if (indicator != null)
//                indicator.SetActive(false);
//        }
//    }

//    public bool IsPlayerInRange() => playerInRange;
//}

using UnityEngine;

public class InteractionIndicator : MonoBehaviour
{
    [Header("Indicator Setup")]
    public GameObject indicator;
    public string playerTag = "Player";
    public bool requireKeyPress = false;
    public KeyCode triggerKey = KeyCode.Space;

    [Header("Outline Highlight (Cakeslice)")]
    public bool enableOutline = true;
    private cakeslice.Outline outline;
    private bool playerInRange;

    void Start()
    {
        if (indicator != null)
            indicator.SetActive(false);

        outline = GetComponent<cakeslice.Outline>();
        if (outline != null)
            outline.enabled = false;
    }

    void Update()
    {
        if (!playerInRange || indicator == null) return;

        if (requireKeyPress)
            indicator.SetActive(Input.GetKey(triggerKey));
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = true;
        if (!requireKeyPress && indicator != null)
            indicator.SetActive(true);

        if (enableOutline && outline != null)
            outline.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = false;
        if (indicator != null)
            indicator.SetActive(false);

        if (enableOutline && outline != null)
            outline.enabled = false;
    }

    public bool IsPlayerInRange() => playerInRange;
}
