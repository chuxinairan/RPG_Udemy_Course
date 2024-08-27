using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private float moveDir;

    private float lastAttackTime;

    private Enemy_Skeleton enemy;
    public SkeletonBattleState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_Skeleton _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
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
            if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachinde.ChangeState(enemy.attackState);
                }
                return;
            }
        } else
        {
            if(stateTimer <= 0 || Vector2.Distance(player.position, enemy.transform.position) > enemy.playerAwayDiatance)
            {
                stateMachinde.ChangeState(enemy.idleState);
                return;
            }
        }

        // 向玩家方向前进
        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
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
