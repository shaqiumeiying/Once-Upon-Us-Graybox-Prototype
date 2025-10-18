using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    // Store player decisions as key/value pairs
    private Dictionary<string, string> decisions = new Dictionary<string, string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // stays between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveDecision(string npcName, string choice)
    {
        decisions[npcName] = choice;
        Debug.Log($"Saved choice for {npcName}: {choice}");
    }

    public string GetDecision(string npcName)
    {
        if (decisions.ContainsKey(npcName))
            return decisions[npcName];
        return null;
    }
}
