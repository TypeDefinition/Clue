using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomName {
    Garden,
    Study,
    Lounge,

    Num,
}

public enum ItemName {
    None = -1,

    // Weapons
    MeasuringTape,
    Book,
    Scissors,
    BillardBall,

    // Suspects
    DrGreen,
    ProfPlum,
    MrScarlet,

    Num,
}

[System.Serializable]
public class Clue {
    public RoomName room;
    public ItemName item;
    public string description;
}