using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)] public string npcLine;
    public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueChoice
{
    public string text;              // Player¡¯s choice line
    [TextArea(2, 5)] public string npcResponse; // NPC reply
    public int nextIndex = -1;       // Next line index (-1 = end)
}
