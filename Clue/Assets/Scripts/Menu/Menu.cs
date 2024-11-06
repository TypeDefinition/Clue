using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    [SerializeField] private GameObject notesUI;
    [SerializeField] private GameObject solverUI;

    private void Start() {

    }

    private void OnEnable() {
        // GameEventSystem.GetInstance().SubscribeToEvent<bool>(nameof(GameEventName.NotesEnable), OnNotesEnable);
    }

    private void OnDisable() {
        // GameEventSystem.GetInstance().UnsubscribeFromEvent<bool>(nameof(GameEventName.NotesEnable), OnNotesEnable);
    }

    private void Update() {

    }

    public void ToggleNotesUI() {
        notesUI.SetActive(!notesUI.activeSelf);
    }

    public void ToggleSolverUI() {
        solverUI.SetActive(!solverUI.activeSelf);
    }
}