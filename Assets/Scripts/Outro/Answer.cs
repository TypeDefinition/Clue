using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Answer", menuName = "Outro/Answer")]
public class Answer : ScriptableObject {
    public ItemName murderer = ItemName.None;
    public ItemName murderWeapon = ItemName.None;
}