using System.Collections;
using UnityEngine;
using TMPro;

public class DelayText : MonoBehaviour
{
    // The delay time in seconds before the text is activated
    public float delayInSeconds = 4.0f;

    // Reference to the TextMeshPro component
    private TextMeshProUGUI textComponent;

    void Awake()
    {
        // Get the TextMeshProUGUI component attached to this GameObject.
        // Make sure your GameObject uses TextMeshPro (UI -> Text - TextMeshPro).
        textComponent = GetComponent<TextMeshProUGUI>();

        // IMPORTANT: Ensure the text is hidden (inactive) immediately when the scene starts.
        if (textComponent != null)
        {
            textComponent.enabled = false;
        }
    }

    void Start()
    {
        // Start the coroutine that will handle the timing
        StartCoroutine(ShowTextAfterDelay(delayInSeconds));
    }

    /// <summary>
    /// Coroutine to wait for the specified time and then show the text.
    /// </summary>
    /// <param name="delay">The waiting time in seconds.</param>
    IEnumerator ShowTextAfterDelay(float delay)
    {
        // 1. Wait for the specified amount of time.
        yield return new WaitForSeconds(delay);

        // 2. After the wait, check if the component exists and activate it.
        if (textComponent != null)
        {
            Debug.Log($"Activating text component after {delay} seconds.");
            textComponent.enabled = true;
        }
    }
}