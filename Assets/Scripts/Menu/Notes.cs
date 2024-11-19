using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Notebook Data", menuName = "Notebook Data")]
public class Notes : ScriptableObject {
    public Clue[] discoveredClues = new Clue[0];
}