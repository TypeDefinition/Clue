﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class Notebook : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject scrollView;
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

    /*private string ToString(RoomName room) {
        switch (room) {
            case RoomName.Garden: return "Garden";
            case RoomName.Study: return "Study";
            case RoomName.Lounge: return "Lounge";
            case RoomName.Ballroom: return "Ballroom";

            default: throw new Exception(MethodBase.GetCurrentMethod().Name + " - Undefined room!");
        }
    }*/

    private string ToString(ItemName item) {
        switch (item) {
            // Weapons
            case ItemName.MeasuringTape: return "Measuring Tape";
            case ItemName.Book: return "Book";
            case ItemName.Scissors: return "Scissors";
            case ItemName.BillardBall: return "Billard Ball";
            case ItemName.Corpse: return "Mr. Black's Corpse";

            // Suspects
            case ItemName.DrGreen: return "Dr. Green";
            case ItemName.ProfPlum: return "Prof. Plum";
            case ItemName.MrScarlet: return "Mr. Scarlet";

            default: throw new Exception(MethodBase.GetCurrentMethod().Name + " - Undefined weapon or suspect!");
        }
    }

    private void UpdateText() {
        // Organise clues by room and item.
        Dictionary<string, HashSet<string>> notes = new Dictionary<string, HashSet<string>>();
        foreach (Clue clue in discoveredClues) {
            string item = ToString(clue.item);
            string desc = clue.description;

            if (!notes.ContainsKey(item)) {
                notes.Add(item, new HashSet<string>());
            }
            notes[item].Add(desc);
        }

        // Write to notebook text.
        if (notes.Count == 0) { return; }
        text.text = string.Empty;
        foreach (var iter in notes) {
            string item = iter.Key;
            text.text += item + ":";

            HashSet<string> descriptions = iter.Value;
            foreach (string desc in descriptions) {
                text.text += "\n◦ ";
                text.text += desc;
            }

            text.text += "\n\n";
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
        scrollView.SetActive(visible);
    }
}