using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestionList : MonoBehaviour {
    [SerializeField] private GameObject questionButtonPrefab;
    [SerializeField] private float padding = 8.0f;

    private List<GameObject> buttons = new List<GameObject>();

    private void Awake() {

    }

    private void Start() {

    }

    private void Update() {

    }

    public void AddButton(string text, UnityAction callback) {
        GameObject button = Instantiate(questionButtonPrefab, GetComponent<RectTransform>());
        button.GetComponent<QuestionButton>().SetText(text);
        button.GetComponent<QuestionButton>().AddButtonCallback(callback);
        buttons.Add(button);

        float y = padding;
        for (int i = 0; i < buttons.Count; ++i) {
            RectTransform rt = GetComponent<RectTransform>();
            buttons[buttons.Count - 1 - i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f, y, 0.0f);
            y += (padding + buttons[buttons.Count - 1 - i].GetComponent<RectTransform>().rect.height);
        }
    }

    public void ClearButtons() {
        for (int i = 0; i < buttons.Count; ++i) {
            Destroy(buttons[i]);
        }
        buttons.Clear();
    }
}