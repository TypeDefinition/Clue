using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDialogButton : MonoBehaviour {
    public void OnEndDialog() {
        GameEventSystem.GetInstance().TriggerEvent(nameof(GameEventName.EndDialog));
    }
}