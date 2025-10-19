using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveMinigameManager : MonoBehaviour
{
    // === References ===
    [Header("References")]
    public Material[] materials;
    public GameObject key1;
    public GameObject key2;
    public GameObject key3;
    public GameObject key1Overlay;
    public GameObject key2Overlay;
    public GameObject key3Overlay;

    [Header("Overlay Settings")]
    public float overlayScaleTime = 1f;

    [Header("Sound Effects")]
    public AudioClip wrongKey;
    public AudioClip correctKey;
    private AudioSource audioSource;

    // Store the direction for each key (0=up, 1=down, 2=left, 3=right)
    private int key1Direction;
    private int key2Direction;
    private int key3Direction;

    // Track which key should be pressed next
    private int currentKeyIndex = 1;

    // Track if all keys were pressed correctly
    private bool allCorrect = true;

    // Coroutine references
    private Coroutine key1OverlayCoroutine;
    private Coroutine key2OverlayCoroutine;
    private Coroutine key3OverlayCoroutine;

    void Start()
    {
        // Get or add AudioSource component (only needs to happen once)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        SetOverlayRenderQueue();
    }

    void OnEnable()
    {
        // Reset the minigame state every time it's enabled
        ResetMinigame();
    }

    void ResetMinigame()
    {
        // Reset tracking variables
        currentKeyIndex = 1;
        allCorrect = true;

        // Stop any running coroutines
        if (key1OverlayCoroutine != null) StopCoroutine(key1OverlayCoroutine);
        if (key2OverlayCoroutine != null) StopCoroutine(key2OverlayCoroutine);
        if (key3OverlayCoroutine != null) StopCoroutine(key3OverlayCoroutine);

        // Re-enable all keys
        if (key1 != null) key1.SetActive(true);
        if (key2 != null) key2.SetActive(true);
        if (key3 != null) key3.SetActive(true);

        // Disable overlays 2 and 3
        if (key2Overlay != null) key2Overlay.SetActive(false);
        if (key3Overlay != null) key3Overlay.SetActive(false);

        // Reset overlay 1 scale
        if (key1Overlay != null)
        {
            Vector3 scale = key1Overlay.transform.localScale;
            scale.y = 0;
            key1Overlay.transform.localScale = scale;
            key1Overlay.SetActive(true);
        }

        // Change materials to new random directions
        ChangeKeyMaterial();

        // Start key1 overlay timer
        key1OverlayCoroutine = StartCoroutine(ScaleOverlay(key1Overlay, 1));
    }

    private void Update()
    {
        CheckKeyInput();
    }

    // === Helper Methods ===
    /// <summary>
    /// Changes the material of each key to a random direction material.
    /// </summary>
    void ChangeKeyMaterial()
    {
        // Initialize renderers for each key
        Renderer key1_renderer = key1.GetComponent<Renderer>();
        Renderer key2_renderer = key2.GetComponent<Renderer>();
        Renderer key3_renderer = key3.GetComponent<Renderer>();

        // Randomly choose direction for key1
        if (key1_renderer != null)
        {
            key1Direction = Random.Range(0, materials.Length);
            key1_renderer.material = materials[key1Direction];
            key1_renderer.material.renderQueue = 2000;
        }

        // Randomly choose direction for key2
        if (key2_renderer != null)
        {
            key2Direction = Random.Range(0, materials.Length);
            key2_renderer.material = materials[key2Direction];
            key2_renderer.material.renderQueue = 2000;
        }

        // Randomly choose direction for key3
        if (key3_renderer != null)
        {
            key3Direction = Random.Range(0, materials.Length);
            key3_renderer.material = materials[key3Direction];
            key3_renderer.material.renderQueue = 2000;
        }
    }

    /// <summary>
    /// Animates the overlay Y scale from 0 to 1 over time.
    /// When it reaches 1, disables the current key (counts as incorrect).
    /// </summary>
    IEnumerator ScaleOverlay(GameObject overlay, int keyIndex)
    {
        if (overlay == null) yield break;

        // Enable the overlay
        overlay.SetActive(true);

        Vector3 startScale = overlay.transform.localScale;
        startScale.y = 0;
        overlay.transform.localScale = startScale;

        float elapsed = 0f;

        GameObject targetKey = null;
        if (keyIndex == 1) targetKey = key1;
        else if (keyIndex == 2) targetKey = key2;
        else if (keyIndex == 3) targetKey = key3;

        while (elapsed < overlayScaleTime)
        {
            // Check if key was already disabled by user input
            if (targetKey != null && !targetKey.activeSelf)
            {
                yield break;
            }

            elapsed += Time.deltaTime;
            float t = elapsed / overlayScaleTime;

            Vector3 newScale = overlay.transform.localScale;
            newScale.y = Mathf.Lerp(0, 1, t);
            overlay.transform.localScale = newScale;

            yield return null;
        }

        // Ensure it ends at exactly 1
        Vector3 finalScale = overlay.transform.localScale;
        finalScale.y = 1;
        overlay.transform.localScale = finalScale;

        // Time ran out - disable the key and mark as incorrect
        if (currentKeyIndex == keyIndex && targetKey != null && targetKey.activeSelf)
        {
            targetKey.SetActive(false);
            allCorrect = false;
            Debug.Log("Key " + keyIndex + " - Time ran out!");

            PlayWrongKeySFX();

            currentKeyIndex += 1;

            // Start next key's overlay timer
            if (keyIndex == 1 && key2Overlay != null)
            {
                key2OverlayCoroutine = StartCoroutine(ScaleOverlay(key2Overlay, 2));
            }
            else if (keyIndex == 2 && key3Overlay != null)
            {
                key3OverlayCoroutine = StartCoroutine(ScaleOverlay(key3Overlay, 3));
            }
            else if (keyIndex == 3)
            {
                Debug.Log("Oops!");
                StartCoroutine(DisableMinigameAfterDelay(1f));
            }
        }
    }

    /// <summary>
    /// Plays the wrong key sound effect.
    /// </summary>
    void PlayWrongKeySFX()
    {
        if (wrongKey != null && audioSource != null)
        {
            audioSource.PlayOneShot(wrongKey);
        }
    }

    /// <summary>
    /// Plays the correct key sound effect.
    /// </summary>
    void PlayCorrectKeySFX()
    {
        if (correctKey != null && audioSource != null)
        {
            audioSource.PlayOneShot(correctKey);
        }
    }

    /// <summary>
    /// Checks if the user presses any arrow key and disables the current key, tracking correctness.
    /// </summary>
    void CheckKeyInput()
    {
        // Check if any arrow key was pressed
        int pressedDirection = GetPressedArrowKey();

        if (pressedDirection != -1)
        {
            // Check key1
            if (currentKeyIndex == 1 && key1.activeSelf)
            {
                key1.SetActive(false);

                if (pressedDirection != key1Direction)
                {
                    allCorrect = false;
                    Debug.Log("Key 1 wrong!");
                    PlayWrongKeySFX();
                }
                else
                {
                    Debug.Log("Key 1 correct!");
                    PlayCorrectKeySFX();
                }

                currentKeyIndex += 1;

                if (key2Overlay != null)
                {
                    key2OverlayCoroutine = StartCoroutine(ScaleOverlay(key2Overlay, 2));
                }
            }
            // Check key2
            else if (currentKeyIndex == 2 && key2.activeSelf)
            {
                key2.SetActive(false);

                if (pressedDirection != key2Direction)
                {
                    allCorrect = false;
                    Debug.Log("Key 2 wrong!");
                    PlayWrongKeySFX();
                }
                else
                {
                    Debug.Log("Key 2 correct!");
                    PlayCorrectKeySFX();
                }

                currentKeyIndex += 1;

                if (key3Overlay != null)
                {
                    key3OverlayCoroutine = StartCoroutine(ScaleOverlay(key3Overlay, 3));
                }
            }
            // Check key3
            else if (currentKeyIndex == 3 && key3.activeSelf)
            {
                key3.SetActive(false);

                if (pressedDirection != key3Direction)
                {
                    allCorrect = false;
                    Debug.Log("Key 3 wrong!");
                    PlayWrongKeySFX();
                }
                else
                {
                    Debug.Log("Key 3 correct!");
                    PlayCorrectKeySFX();
                }

                currentKeyIndex += 1;

                // Display final result
                if (allCorrect)
                {
                    Debug.Log("Perfectly roasted!");
                }
                else
                {
                    Debug.Log("Oops!");
                }

                StartCoroutine(DisableMinigameAfterDelay(1f));
            }
        }
    }

    /// <summary>
    /// Returns the direction of the arrow key pressed this frame, or -1 if none pressed.
    /// 0=up, 1=down, 2=left, 3=right
    /// </summary>
    int GetPressedArrowKey()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            return 0;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            return 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            return 2;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            return 3;

        return -1;
    }

    void SetOverlayRenderQueue()
    {
        if (key1Overlay != null)
        {
            Renderer renderer = key1Overlay.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.renderQueue = 3100;
            }
        }

        if (key2Overlay != null)
        {
            Renderer renderer = key2Overlay.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.renderQueue = 3100;
            }
        }

        if (key3Overlay != null)
        {
            Renderer renderer = key3Overlay.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.renderQueue = 3100;
            }
        }
    }

    IEnumerator DisableMinigameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}