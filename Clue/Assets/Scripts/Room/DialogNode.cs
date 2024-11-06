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
    public VideoClip clip;
    public ButtonOption[] options = new ButtonOption[0];
}