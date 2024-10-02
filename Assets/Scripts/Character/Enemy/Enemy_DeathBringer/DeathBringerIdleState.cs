using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    protected Enemy_DeathBringer enemy;

    public DeathBringerIdleState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_DeathBringer _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.V))
            stateMachinde.ChangeState(enemy.teleportState);
    }
}
