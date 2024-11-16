using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;
using System;

public class Notebook : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI text;

    private HashSet<Clue> clues = new HashSet<Clue>();

    private void OnEnable() {
        GameEventSystem.GetInstance().SubscribeToEvent<Clue[]>(nameof(GameEventName.FoundClues), OnFoundClues);
    }

    private void OnDisabled() {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Clue[]>(nameof(GameEventName.FoundClues), OnFoundClues);
    }

    private string ToString(RoomName room) {
        switch (room) {
            case RoomName.Garden: return "Garden";
            case RoomName.Study: return "Study";
            case RoomName.Lounge: return "Lounge";
            default:
                throw new Exception(MethodBase.GetCurrentMethod().Name + " - Undefined room!");
        }
    }

    private string ToString(ItemName item) {
        switch (item) {
            case ItemName.MeasuringTape: return "Measuring Tape";
            case ItemName.Book: return "Book";
            case ItemName.Scissors: return "Scissors";
            case ItemName.DocGreen: return "Dr. Green";
            case ItemName.ProfPlum: return "Professor Plum";
            case ItemName.MrScarlet: return "Mr. Scarlet";
            default: throw new Exception(MethodBase.GetCurrentMethod().Name + " - Undefined item!");
        }
    }

    // GameEvent Callbacks
    private void OnFoundClues(Clue[] newClues) {
        for (int i = 0; i < newClues.Length; ++i) {
            clues.Add(newClues[i]);
        }

        // Organise clues by room and item.
        Dictionary<string, Dictionary<string, List<string>>> notes = new Dictionary<string, Dictionary<string, List<string>>>();
        foreach (Clue c in clues) {
            string room = ToString(c.room);
            string item = ToString(c.item);
            string desc = c.description;

            if (!notes.ContainsKey(room)) {
                notes.Add(room, new Dictionary<string, List<string>>());
            }
            if (!notes[room].ContainsKey(item)) {
                notes[room].Add(item, new List<string>());
            }
            notes[room][item].Add(desc);
        }

        // Write to notebook text.
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

    public void ToggleVisibility() {
        if (background.activeSelf) {
            background.SetActive(false);
            text.gameObject.SetActive(false);
        } else {
            background.SetActive(true);
            text.gameObject.SetActive(true);
        }
    }
}