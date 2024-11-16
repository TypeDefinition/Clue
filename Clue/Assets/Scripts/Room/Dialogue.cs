using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class DialogueOption {
    public string text;
    public Dialogue nextDialogue;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject {
    [Header("Settings")]
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private DialogueOption[] options = new DialogueOption[0];

    [Header("Clue (Optional)")]
    [SerializeField] private Clue[] clues = new Clue[0];

    public VideoClip GetVideoClip() { return videoClip; }
    public DialogueOption[] GetOptions() { return options; }

    public Clue[] GetClues() { return clues; }
}