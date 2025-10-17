using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouringMinigameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform timingBar;
    [SerializeField] private Transform marker;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 2f; // Units per second
    [SerializeField] private float pauseDuration = 0.2f; // Pause at ends (optional)

    private float minX;
    private float maxX;
    private bool movingRight = true;
    private float pauseTimer = 0f;
    private bool isPaused = false;

    void Start()
    {
        // Calculate the movement bounds based on the TimingBar bounds
        if (timingBar != null && marker != null)
        {
            Renderer barRenderer = timingBar.GetComponent<Renderer>();
            Renderer markerRenderer = marker.GetComponent<Renderer>();

            if (barRenderer != null && markerRenderer != null)
            {
                // Get the world space bounds
                float barWidth = barRenderer.bounds.size.x;
                float markerWidth = markerRenderer.bounds.size.x;

                // Calculate min and max X positions in world space
                float barCenterX = timingBar.position.x;
                minX = barCenterX - (barWidth / 2) + (markerWidth / 2);
                maxX = barCenterX + (barWidth / 2) - (markerWidth / 2);

                // Start at the left position
                Vector3 pos = marker.position;
                pos.x = minX;
                marker.position = pos;
            }
        }
    }

    void Update()
    {
        if (timingBar == null || marker == null) return;

        // Handle pause at ends
        if (isPaused)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration)
            {
                isPaused = false;
                pauseTimer = 0f;
            }
            return;
        }

        // Move the marker
        Vector3 pos = marker.position;

        if (movingRight)
        {
            pos.x += movementSpeed * Time.deltaTime;

            if (pos.x >= maxX)
            {
                pos.x = maxX;
                movingRight = false;
                isPaused = pauseDuration > 0;
            }
        }
        else
        {
            pos.x -= movementSpeed * Time.deltaTime;

            if (pos.x <= minX)
            {
                pos.x = minX;
                movingRight = true;
                isPaused = pauseDuration > 0;
            }
        }

        marker.position = pos;
    }

    // Public methods for controlling the marker
    public void SetSpeed(float speed)
    {
        movementSpeed = speed;
    }

    public void ResetPosition()
    {
        if (marker != null)
        {
            Vector3 pos = marker.position;
            pos.x = minX;
            marker.position = pos;
            movingRight = true;
        }
    }

    public void PauseMovement()
    {
        enabled = false;
    }

    public void ResumeMovement()
    {
        enabled = true;
    }

    // Get the normalized position (0 to 1) of the marker
    public float GetNormalizedPosition()
    {
        if (marker == null) return 0f;
        return Mathf.InverseLerp(minX, maxX, marker.position.x);
    }
}