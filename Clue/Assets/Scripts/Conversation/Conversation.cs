using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Conversation", menuName = "Conversation/Conversation")]
public class Conversation : ScriptableObject {
    [Header("Settings")]
    [SerializeField] private VideoClip startClip;
    [SerializeField] private VideoClip endClip;
    [SerializeField] private Dialogue initialDialogue;

    public VideoClip GetStartVideoClip() { return startClip; }
    public VideoClip GetEndVideoClip() { return endClip; }
    public Dialogue GetInitialDialogue() { return initialDialogue; }
}