using System.Collections.Generic; 
using UnityEngine; 
public class GameStateManager : MonoBehaviour { 
    public static GameStateManager Instance; 
    // store general decisions: e.g. "TeaWitch" ¡ú "CompletedMainDialogue"
    private Dictionary<string, string> decisions = new Dictionary<string, string>(); 
    // optional numeric stage tracker: e.g. "TeaWitch"
    private Dictionary<string, int> npcStages = new Dictionary<string, int>(); 
    void Awake() { 
        // Singleton pattern ¡ª only one instance across all scenes
        if (Instance == null) { 
            Instance = this; 
            DontDestroyOnLoad(gameObject); 
        } 
        else 
        { Destroy(gameObject); 
        } 
    } 
    // =============== STRING-BASED DECISIONS =============== 
    /// Save a string decision
    public void SaveDecision(string npcName, string choice) { 
        decisions[npcName] = choice; 
        Debug.Log($"[GameState] Saved choice for {npcName}: {choice}"); 
    } /// Get a previously saved decision (returns null if none found) 
    public string GetDecision(string npcName) { 
        if (decisions.TryGetValue(npcName, out string value)) 
            return value; 
        return null; 
    } 

    public void SaveStage(string npcName, int stage) 
    { npcStages[npcName] = stage; 
        Debug.Log($"[GameState] Saved stage for {npcName}: {stage}"); 
    } 
    /// Get the saved numeric stage (returns 0 if not found)

    public int GetStage(string npcName) 
    { if (npcStages.TryGetValue(npcName, out int stage)) return stage; return 0; 
    } 

    // =============== UTILITY HELPERS =============== // Checks if a decision exists for the given NPC.

    public bool HasDecision(string npcName) { 
        return decisions.ContainsKey(npcName); 
    } /// Resets all stored states ¡ª useful for testing. 
    public void ClearAll() { 
        decisions.Clear(); npcStages.Clear(); 
        Debug.Log("[GameState] All saved data cleared."); 
    } 
}