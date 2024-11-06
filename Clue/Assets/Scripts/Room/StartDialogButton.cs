using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogButton : MonoBehaviour {
    public void OnStartDialog(DialogNode node) {
        GameEventSystem.GetInstance().TriggerEvent<DialogNode>(nameof(GameEventName.StartDialog), node);
    }
}