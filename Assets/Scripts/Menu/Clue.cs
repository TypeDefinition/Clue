using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public ItemName item;
    public string description;

    public override bool Equals(object obj) {
        var other = obj as Clue;
        if (other == null) return false;
        return other.item == item && other.description == description;
    }

    public override int GetHashCode() {
        return item.GetHashCode() + description.GetHashCode();
    }
}