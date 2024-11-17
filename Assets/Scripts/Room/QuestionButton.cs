using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

// Simple helper class to consolidate the functionalities of various components into one.
[RequireComponent(typeof(Button))]
public class QuestionButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textUI;

    private void Start() {
    }

    private void Update() {

    }

    public void SetText(string text) {
        textUI.text = text;
    }

    public void AddButtonCallback(UnityAction callback) {
        GetComponent<Button>().onClick.AddListener(callback);
    }

    public void RemoveButtonCallback(UnityAction callback) {
        GetComponent<Button>().onClick.RemoveListener(callback);
    }
}