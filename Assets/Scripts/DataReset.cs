using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataReset : MonoBehaviour {
    [SerializeField] private Answer answer;
    [SerializeField] private Notes notes;

    public void ResetData() {
        if (answer != null) { answer.Reset(); }
        if (notes != null) { notes.Reset(); }
    }
}