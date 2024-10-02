using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
    private float moveDir;

    private Enemy_Archer enemy;
    private Transform playerTrans;

    public ArcherBattleState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_Archer _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
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
        // 发现玩家
        if (enemy.IsPlayerDetected())
        {
            if (enemy.IsPlayerDetected().distance < enemy.safeDistance && CanJump())
            {
                stateMachinde.ChangeState(enemy.jumpState);
                return;
            }

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance && CanAttack())
            {
                if (CanAttack())
                {
                    stateMachinde.ChangeState(enemy.attackState);
                    return;
                }
            }
        }
        else
        {
            // 玩家远离或者超出一定时间
            if (stateTimer <= 0 || Vector2.Distance(playerTrans.position, enemy.transform.position) > enemy.playerAwayDiatance)
            {
                stateMachinde.ChangeState(enemy.idleState);
                return;
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

    private bool CanJump()
    {
        if (Time.time > enemy.lastJumpTime + enemy.jumpCooldown)
        {
            return true;
        }
        return false;
    }
}
