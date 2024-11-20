using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Notes", menuName = "Notes")]
public class Notes : ScriptableObject {
    public Clue[] discoveredClues = new Clue[0];
}