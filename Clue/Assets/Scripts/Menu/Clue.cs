using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomName {
    Garden,
    Study,
    Lounge,
}

public enum ItemName {
    // Weapons
    MeasuringTape,
    Book,
    Scissors,

    // Suspects
    DocGreen,
    ProfPlum,
    MrScarlet,
}

[System.Serializable]
public class Clue {
    public RoomName room;
    public ItemName item;
    public string description;
}