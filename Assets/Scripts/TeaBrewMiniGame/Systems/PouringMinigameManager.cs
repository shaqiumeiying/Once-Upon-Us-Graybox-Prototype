using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the pouring minigame timing bar mechanics.
/// </summary>
public class PouringMinigameManager : MonoBehaviour
{
    // === References ===
    [Header("References")]
    [SerializeField] private Transform timingBar;
    [SerializeField] private Transform marker;

    // === Movement Settings ===
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float pauseDuration = 0.5f;  // Delay before disabling after spacebar press

    // === Internal State ===
    private float minX;                 // Left boundary for marker movement
    private float maxX;                 // Right boundary for marker movement
    private bool movingRight = true;    // Current direction of marker movement
    private bool isStopped = false;     // Whether marker has been stopped by player input

    /// <summary>
    /// Initialize the timing bar bounds and position the marker at the start.
    /// </summary>
    void Start()
    {
        InitializeTimingBar();
    }

    /// <summary>
    /// Update marker position each frame, handling movement/direction changes and spacebar pressed
    /// </summary>
    void Update()
    {
        // Safety check: exit if references are missing
        if (timingBar == null || marker == null) return;

        HandlePlayerInput();

        // Exit if marker has been stopped by player
        if (isStopped) return;

        UpdateMarkerPosition();
    }

    // === Helper Methods ===

    /// <summary>
    /// Calculate movement bounds and set initial marker position.
    /// </summary>
    private void InitializeTimingBar()
    {
        if (timingBar == null || marker == null) return;

        Renderer barRenderer = timingBar.GetComponent<Renderer>();
        Renderer markerRenderer = marker.GetComponent<Renderer>();

        if (barRenderer != null && markerRenderer != null)
        {
            CalculateMovementBounds(barRenderer, markerRenderer);
            SetMarkerToStartPosition();
        }
    }

    /// <summary>
    /// Calculate the min and max X boundaries for marker movement.
    /// </summary>
    private void CalculateMovementBounds(Renderer barRenderer, Renderer markerRenderer)
    {
        // Get the world space bounds of both objects
        float barWidth = barRenderer.bounds.size.x;
        float markerWidth = markerRenderer.bounds.size.x;

        // Calculate min and max X positions in world space
        // This keeps the marker within the visual bounds of the timing bar
        float barCenterX = timingBar.position.x;
        minX = barCenterX - (barWidth / 2) + (markerWidth / 2);
        maxX = barCenterX + (barWidth / 2) - (markerWidth / 2);
    }

    /// <summary>
    /// Position the marker at the starting (left) position.
    /// </summary>
    private void SetMarkerToStartPosition()
    {
        Vector3 pos = marker.position;
        pos.x = minX;
        marker.position = pos;
    }

    /// <summary>
    /// Check for player input (spacebar) to stop the marker.
    /// </summary>
    private void HandlePlayerInput()
    {
        // Check for spacebar input to stop the marker
        if (Input.GetKeyDown(KeyCode.Space) && !isStopped)
        {
            isStopped = true;
            StartCoroutine(DisableAfterDelay());
        }

    }

    /// <summary>
    /// Enforces a timer to disable the minigame after a delay.
    /// </summary>
    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(pauseDuration);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Update the marker's position and handle boundary detection.
    /// </summary>
    private void UpdateMarkerPosition()
    {
        Vector3 pos = marker.position;

        if (movingRight)
        {
            MoveMarkerRight(ref pos);
        }
        else
        {
            MoveMarkerLeft(ref pos);
        }

        marker.position = pos;
    }

    /// <summary>
    /// Move the marker right and check for right boundary.
    /// </summary>
    private void MoveMarkerRight(ref Vector3 pos)
    {
        pos.x += movementSpeed * Time.deltaTime;

        // Check if reached right boundary
        if (pos.x >= maxX)
        {
            pos.x = maxX;
            movingRight = false;
        }
    }

    /// <summary>
    /// Move the marker left and check for left boundary.
    /// </summary>
    private void MoveMarkerLeft(ref Vector3 pos)
    {
        pos.x -= movementSpeed * Time.deltaTime;

        // Check if reached left boundary
        if (pos.x <= minX)
        {
            pos.x = minX;
            movingRight = true;
        }
    }

    // === Public Control Methods ===

    /// <summary>
    /// Dynamically adjust the marker's movement speed.
    /// </summary>
    /// <param name="speed">New speed in units per second</param>
    public void SetSpeed(float speed)
    {
        movementSpeed = speed;
    }
}