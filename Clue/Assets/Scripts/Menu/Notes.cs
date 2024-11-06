using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Notes : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;

    // Ensure static to allow data persistence between scenes.
    private static Dictionary<string, Dictionary<string, List<string>>> data = new Dictionary<string, Dictionary<string, List<string>>>();

    // Listen for events even when disabled.
    private void Awake() {
        GameEventSystem.GetInstance().SubscribeToEvent<string, string, string>(nameof(GameEventName.FoundClue), OnFoundClue);
    }

    private void OnDestroy() {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<string, string, string>(nameof(GameEventName.FoundClue), OnFoundClue);
    }

    private void Start() {
    }

    private void Update() {
    }

    private void UpdateText() {
        // Clear string.
        text.text = string.Empty;

        // Write to string.
        bool isFirst = true;
        foreach (var i in data) {
            string room = i.Key;

            if (!isFirst) {
                text.text += "\n";
            }
            isFirst = false;
            text.text += room;

            Dictionary<string, List<string>> items = i.Value;
            foreach (var j in items) {
                string item = j.Key;
                text.text += "\n    • ";
                text.text += item;

                List<string> descriptions = j.Value;
                foreach (string desc in descriptions) {
                    text.text += "\n        ◦ ";
                    text.text += desc;
                }
            }
        }
    }

    // GameEvent Callbacks
    private void OnFoundClue(string room, string item, string desc) {
        // Update data.
        if (!data.ContainsKey(room)) {
            data.Add(room, new Dictionary<string, List<string>>());
        }
        if (!data[room].ContainsKey(item)) {
            data[room].Add(item, new List<string>());
        }
        data[room][item].Add(desc);

        // Update text.
        UpdateText();
    }
}