using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStunnedState : EnemyState
{
    private Enemy_Archer enemy;

    public ArcherStunnedState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_Archer _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunnedDuration;
        enemy.fx.InvokeRepeating("RedWhiteBlink", 0, .1f);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.CancleColorChange();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0)
            stateMachinde.ChangeState(enemy.idleState);
    }
}
