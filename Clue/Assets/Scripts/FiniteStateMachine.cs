using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FiniteStateMachine
{
    public const int INVALID_STATE = -1;
    public delegate void FuncPtr();

    // AI Finite State Machine
    private int currentState = INVALID_STATE; // Use negative value to denote an invalid state.
    private int nextState = INVALID_STATE;
    private FuncPtr[] stateEntries = null;
    private FuncPtr[] stateUpdates = null;
    private FuncPtr[] stateExits = null;

    public void SetNumStates(int numStates)
    {
        stateEntries = new FuncPtr[numStates];
        stateUpdates = new FuncPtr[numStates];
        stateExits = new FuncPtr[numStates];
    }

    public void SetStateEntry(int index, FuncPtr funcPtr = null) { stateEntries[index] = funcPtr; }
    public void SetStateUpdate(int index, FuncPtr funcPtr = null) { stateUpdates[index] = funcPtr; }
    public void SetStateExit(int index, FuncPtr funcPtr = null) { stateExits[index] = funcPtr; }

    public int GetCurrentState() { return currentState; }

    public void Update()
    {
        // Transit state.
        if (nextState != currentState)
        {
            // Exit current state.
            if (0 <= currentState) { stateExits[currentState]?.Invoke(); }

            // Enter next state.
            currentState = nextState;
            if (0 <= currentState) { stateEntries[currentState]?.Invoke(); }
        }

        // Update current state.
        if (0 <= currentState) { stateUpdates[currentState]?.Invoke(); }
    }

    public void ChangeState(int nextState) { this.nextState = nextState; }
}