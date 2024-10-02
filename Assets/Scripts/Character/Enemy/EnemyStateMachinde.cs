using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachinde
{
    public EnemyState currentState;

    public void Initialize(EnemyState _startState)
    {
        this.currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState _newState)
    {
        this.currentState.Exit();
        this.currentState = _newState;
        this.currentState.Enter();
    }
}
