using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;
    private Transform playerTrans;

    public SkeletonGroundedState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_Skeleton _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        playerTrans = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() || Vector2.Distance(playerTrans.position, enemy.transform.position) < 2f)
        {
            stateMachinde.ChangeState(enemy.battleState);
        }
    }
}
