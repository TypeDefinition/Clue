using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Investigation", menuName = "Conversation/Investigation")]
public class Investigation : ScriptableObject {
    [Header("Settings")]
    [SerializeField] private VideoClip videoClip;
    
    [Header("Clue (Optional)")]
    [SerializeField] private Clue[] clues = new Clue[0];

    public VideoClip GetVideoClip() { return videoClip; }

    public Clue[] GetClues() { return clues; }
}