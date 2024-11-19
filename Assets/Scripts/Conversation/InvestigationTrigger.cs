using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationTrigger : MonoBehaviour {
    public void StartInvestigation(Investigation investigation) {
        GameEventSystem.GetInstance().TriggerEvent<Investigation>(nameof(GameEventName.StartInvestigation), investigation);
    }
}