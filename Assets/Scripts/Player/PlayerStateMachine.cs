using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState;

    public void Initialize(PlayerState _startState)
    {
        this.currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        this.currentState.Exit();
        this.currentState = _newState;
        this.currentState.Enter();
    }
}
