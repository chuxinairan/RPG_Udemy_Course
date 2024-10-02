using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    private float moveDir;

    protected Enemy_DeathBringer enemy;
    private Transform playerTrans;
    public DeathBringerBattleState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_DeathBringer _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        playerTrans = PlayerManager.instance.player.transform;
        stateTimer = enemy.battleTime;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        // ∑¢œ÷ÕÊº“
        if (enemy.IsPlayerDetected())
        {
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance && CanAttack())
            {
                if (CanAttack())
                {
                    stateMachinde.ChangeState(enemy.attackState);
                    return;
                }
            }
        }

        if (playerTrans.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (playerTrans.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time > enemy.lastAttackTime + enemy.attackCoolDown)
        {
            return true;
        }
        return false;
    }
}
