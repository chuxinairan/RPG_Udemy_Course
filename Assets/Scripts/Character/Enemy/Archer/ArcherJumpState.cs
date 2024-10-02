using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherJumpState : EnemyState
{
    private Enemy_Archer enemy;
    public ArcherJumpState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_Archer _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.lastJumpTime = Time.time;
        rb.velocity = new Vector2(enemy.jumpForce.x * -enemy.facingDir, enemy.jumpForce.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y <= 0 && enemy.IsGroundDetected())
            stateMachinde.ChangeState(enemy.battleState);

        enemy.anim.SetFloat("yVelocity", rb.velocity.y);
    }
}
