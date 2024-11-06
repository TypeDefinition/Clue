using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class ButtonOption {
    public string text;
    public DialogNode node;
}

[CreateAssetMenu(fileName = "Dialog Node", menuName = "Conversation/Dialog Node")]
public class DialogNode : ScriptableObject {
    [Header("Settings")]
    public VideoClip clip;
    public ButtonOption[] options = new ButtonOption[0];

    [Header("Clues (Optional)")]
    public string clueRoom = string.Empty;
    public string clueItem = string.Empty;
    public string clueDesc = string.Empty;
}