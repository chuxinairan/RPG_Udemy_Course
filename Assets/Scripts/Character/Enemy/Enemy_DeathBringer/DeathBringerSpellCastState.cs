using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    protected Enemy_DeathBringer enemy;
    private int castAmount;
    private float castTimer;

    public DeathBringerSpellCastState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName, Enemy_DeathBringer _enemy) : base(_baseEnemy, _stateMachinde, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        castAmount = enemy.castAmount;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        castTimer -= Time.deltaTime;
        if (CanCast())
        {
            castAmount--;
            enemy.CreateSpellCast();
        }
        if(castAmount <= 0)
            stateMachinde.ChangeState(enemy.teleportState);
    }


    public bool CanCast()
    {
        if (castTimer <= 0)
        {
            castTimer = enemy.castCooldown;
            return true;
        }
        return false;
    }
}
