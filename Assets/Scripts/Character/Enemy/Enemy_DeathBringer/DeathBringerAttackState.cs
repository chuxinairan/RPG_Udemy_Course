using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    protected Enemy_DeathBringer enemy;

    public DeathBringerAttackState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_DeathBringer _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        enemy.chanceToTeleport += 5;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (animationFinishedTrigger)
        {
            if(enemy.CanTeleport())
                stateMachinde.ChangeState(enemy.teleportState);
            else
                stateMachinde.ChangeState(enemy.battleState);
        }
    }
}
