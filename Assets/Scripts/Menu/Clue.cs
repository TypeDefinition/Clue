using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomName {
    Garden,
    Lounge,
    Ballroom,
    Study,

    Num,
}

// TODO: Split this into 2 enums. Right now ItemName is being used in ways not originally designed for.
// Should be for this simple assignment, but it is not ideal.
public enum ItemName {
    None = -1,

    // Suspects
    DrGreen = 0,
    ProfPlum,
    MrScarlet,

    // Weapons
    MeasuringTape,
    Book,
    Scissors,
    BillardBall,
    Corpse,

    Num,
}

[System.Serializable]
public class Clue {
    public RoomName room;
    public ItemName item;
    public string description;
}