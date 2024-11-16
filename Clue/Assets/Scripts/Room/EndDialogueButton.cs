using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDialogueButton : MonoBehaviour {
    public void OnEndDialogue() {
        GameEventSystem.GetInstance().TriggerEvent(nameof(GameEventName.EndDialogue));
    }
}