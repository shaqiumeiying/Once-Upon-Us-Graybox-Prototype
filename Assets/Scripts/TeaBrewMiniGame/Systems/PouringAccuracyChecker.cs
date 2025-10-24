using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks the accuracy of the player's pour timing by detecting if the marker
/// is touching the target area when they press the spacebar.
/// </summary>
public class PouringAccuracyChecker : MonoBehaviour
{
    // === References ===
    [Header("References")]
    [SerializeField] private Transform marker;
    [SerializeField] private Transform targetArea;
    [SerializeField] private PouringMinigameManager pouringManager;

    // === Settings ===
    [Header("Feedback Settings")]
    [SerializeField] private string perfectMessage = "Perfect pour!";
    [SerializeField] private string failMessage = "Careful! That's too much!";

    // === Sound Effects ===
    [Header("Sound Effects")]
    public AudioClip badPour;
    public AudioClip goodPour;
    private AudioSource audioSource;

    void Start()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        CheckForPourAttempt();
    }

    /// <summary>
    /// Check if the player pressed space and evaluate their accuracy.
    /// </summary>
    private void CheckForPourAttempt()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool isAccurate = CheckTouching();
            DisplayFeedback(isAccurate);
        }
    }

    /// <summary>
    /// Check if the marker is touching the target area using bounds overlap.
    /// </summary>
    /// <returns>True if marker is touching target area, false otherwise</returns>
    private bool CheckTouching()
    {
        if (marker == null || targetArea == null) return false;

        Renderer markerRenderer = marker.GetComponent<Renderer>();
        Renderer targetRenderer = targetArea.GetComponent<Renderer>();

        if (markerRenderer == null || targetRenderer == null) return false;

        // Check if the bounds of the two objects are overlapping
        return markerRenderer.bounds.Intersects(targetRenderer.bounds);
    }

    /// <summary>
    /// Display feedback message based on pouring accuracy.
    /// </summary>
    /// <param name="isAccurate">Whether the pour was accurate</param>
    private void DisplayFeedback(bool isAccurate)
    {
        if (isAccurate)
        {
            Debug.Log(perfectMessage);
            // Optional: Call success method
            OnPerfectPour();
        }
        else
        {
            Debug.Log(failMessage);
            // Optional: Call failure method
            OnFailedPour();
        }
    }

    /// <summary>
    /// Called when player achieves a perfect pour.
    /// Add your success logic here (sounds, effects, score, etc.)
    /// </summary>
    private void OnPerfectPour()
    {
        // TODO: Add success feedback (particles, sounds, score increase, etc.)
        // Example: ParticleManager.Instance.PlayEffect("Success", marker.position);
        audioSource.PlayOneShot(goodPour);

    }

    /// <summary>
    /// Called when player misses the target area.
    /// Add your failure logic here (sounds, effects, penalties, etc.)
    /// </summary>
    private void OnFailedPour()
    {
        // TODO: Add failure feedback (sounds, effects, etc.)
        audioSource.PlayOneShot(badPour);
    }

    /// <summary>
    /// Public method to check accuracy without requiring spacebar press.
    /// Useful if you want to check accuracy from other scripts.
    /// </summary>
    /// <returns>True if marker is in target area</returns>
    public bool IsMarkerInTargetArea()
    {
        return CheckTouching();
    }

    /// <summary>
    /// Get how close the marker is to the center of the target (0 = center, 1 = edge or outside).
    /// Useful for partial scoring systems.
    /// </summary>
    /// <returns>Distance factor from 0 (perfect center) to 1+ (outside)</returns>
    public float GetAccuracyFactor()
    {
        if (marker == null || targetArea == null) return 1f;

        float markerX = marker.position.x;
        float targetCenterX = targetArea.position.x;

        Renderer targetRenderer = targetArea.GetComponent<Renderer>();
        if (targetRenderer == null) return 1f;

        float targetHalfWidth = targetRenderer.bounds.size.x / 2f;
        float distance = Mathf.Abs(markerX - targetCenterX);

        // Return 0 at center, 1 at edge, >1 if outside
        return distance / targetHalfWidth;
    }
}