using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCount;

    private float lastAttackTime;
    private float combooWindow = 2f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (comboCount > 2 || Time.time > lastAttackTime + combooWindow)
            comboCount = 0;

        #region Attack Direction
        float attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput;
        #endregion
        player.SetVelocity(player.attackMovement[comboCount].x * player.attackStepScale * attackDir, player.attackMovement[comboCount].y);
        player.anim.SetInteger("ComboCounter", comboCount);
        player.anim.speed = player.attackSpeed;
        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        comboCount++;
        player.anim.speed = 1.0f;
        lastAttackTime = Time.time;
        player.StartCoroutine("BusyFor", .2f); // 进入idle状态不能进行移动的时间
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
