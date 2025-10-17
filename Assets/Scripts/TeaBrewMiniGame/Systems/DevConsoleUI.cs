using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DevConsoleUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI logText;
    [Range(5, 100)]
    public int maxLines = 15;

    private readonly Queue<string> logQueue = new Queue<string>();

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Only show normal and warnings (skip errors)
        if (type == LogType.Log || type == LogType.Warning)
        {
            string msg = $" {logString}";
            logQueue.Enqueue(msg);

            while (logQueue.Count > maxLines)
                logQueue.Dequeue();

            if (logText != null)
                logText.text = string.Join("\n", logQueue);
        }
    }
}
