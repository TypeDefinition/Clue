using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class Notebook : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Notes data;

    private bool visible = false;
    private HashSet<Clue> discoveredClues = new HashSet<Clue>();

    private void OnEnable() {
        GameEventSystem.GetInstance().SubscribeToEvent<Clue[]>(nameof(GameEventName.FoundClues), OnFoundClues);
    }

    private void OnDisabled() {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Clue[]>(nameof(GameEventName.FoundClues), OnFoundClues);
    }

    private void Start() {
        // Read from persistent data.
        for (int i = 0; i < data.discoveredClues.Length; ++i) {
            discoveredClues.Add(data.discoveredClues[i]);
        }
        UpdateText();
    }

    private string ToString(RoomName room) {
        switch (room) {
            case RoomName.Garden: return "Garden";
            case RoomName.Study: return "Study";
            case RoomName.Lounge: return "Lounge";
            case RoomName.Ballroom: return "Ballroom";

            default: throw new Exception(MethodBase.GetCurrentMethod().Name + " - Undefined room!");
        }
    }

    private string ToString(ItemName item) {
        switch (item) {
            // Weapons
            case ItemName.MeasuringTape: return "Measuring Tape";
            case ItemName.Book: return "Book";
            case ItemName.Scissors: return "Scissors";
            case ItemName.BillardBall: return "Billard Ball";

            // Suspects
            case ItemName.DrGreen: return "Dr. Green";
            case ItemName.ProfPlum: return "Prof. Plum";
            case ItemName.MrScarlet: return "Mr. Scarlet";

            default: throw new Exception(MethodBase.GetCurrentMethod().Name + " - Undefined weapon or suspect!");
        }
    }

    private void UpdateText() {
        // Organise clues by room and item.
        Dictionary<string, Dictionary<string, List<string>>> notes = new Dictionary<string, Dictionary<string, List<string>>>();
        foreach (Clue clue in discoveredClues) {
            string room = ToString(clue.room);
            string item = ToString(clue.item);
            string desc = clue.description;

            if (!notes.ContainsKey(room)) {
                notes.Add(room, new Dictionary<string, List<string>>());
            }
            if (!notes[room].ContainsKey(item)) {
                notes[room].Add(item, new List<string>());
            }
            notes[room][item].Add(desc);
        }

        // Write to notebook text.
        if (notes.Count == 0) { return; }
        text.text = string.Empty;
        foreach (var i in notes) {
            string room = i.Key;
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

            text.text += "\n";
        }
    }

    // GameEvent Callbacks
    private void OnFoundClues(Clue[] foundClues) {
        // Add new clues.
        for (int i = 0; i < foundClues.Length; ++i) {
            discoveredClues.Add(foundClues[i]);
        }

        // Save discovered clues to persistent memory.
        data.discoveredClues = discoveredClues.ToArray();

        UpdateText();
    }

    public void ToggleVisibility() {
        visible = !visible;
        background.SetActive(visible);
        text.gameObject.SetActive(visible);
    }
}