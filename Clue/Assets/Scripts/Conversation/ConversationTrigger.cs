using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationTrigger : MonoBehaviour {
    public void StartConversation(Conversation conversation) {
        GameEventSystem.GetInstance().TriggerEvent<Conversation>(nameof(GameEventName.StartConversation), conversation);
    }

    public void EndConversation() {
        GameEventSystem.GetInstance().TriggerEvent(nameof(GameEventName.EndConversation));
    }
}