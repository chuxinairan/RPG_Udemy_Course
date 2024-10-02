using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{
    protected Enemy_DeathBringer enemy;

    public DeathBringerTeleportState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_DeathBringer _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.stats.MakeInvincible(true);
        stateTimer = 1f;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer <= 0)
        {
            enemy.FindTeleportPosition();
            if (enemy.CanSpellCast())
                stateMachinde.ChangeState(enemy.spellCastState);
            else
                stateMachinde.ChangeState(enemy.battleState);
        }
    }
}
