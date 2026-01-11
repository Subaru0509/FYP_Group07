using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
}

[System.Serializable]
public class DialogueLine
{
    [Header("Speaker Info")]
    public string speakerName;
    public Sprite portrait;

    [Header("Dialogue Content")]
    [TextArea(3, 5)]
    public string text;

    [Header("Camera Focus (Optional)")]
    public Transform focusTarget;
}
