using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogueButton : MonoBehaviour {
    public void OnStartDialogue(Dialogue dialogue) {
        GameEventSystem.GetInstance().TriggerEvent<Dialogue>(nameof(GameEventName.StartDialogue), dialogue);
    }
}