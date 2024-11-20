using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookButton : MonoBehaviour {
    [SerializeField] private GameObject notification;

    private void OnEnable() {
        GameEventSystem.GetInstance().SubscribeToEvent<Clue[]>(nameof(GameEventName.FoundClues), OnFoundClues);
    }

    private void OnDisabled() {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Clue[]>(nameof(GameEventName.FoundClues), OnFoundClues);
    }

    private void Start() {

    }

    private void Update() {

    }

    private void OnFoundClues(Clue[] foundClues) {
        NotificationOn();
    }

    public void NotificationOn() {
        notification.SetActive(true);
    }

    public void NotificationOff() {
        notification.SetActive(false);
    }
}