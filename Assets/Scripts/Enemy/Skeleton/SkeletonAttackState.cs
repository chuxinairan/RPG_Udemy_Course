using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    protected Enemy_Skeleton enemy;
    public SkeletonAttackState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_Skeleton _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachinde.ChangeState(enemy.battleState);
        }
    }
}
