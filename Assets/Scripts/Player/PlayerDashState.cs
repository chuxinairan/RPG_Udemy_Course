using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
        player.skillMgr.cloneSkill.CreateCloneOnDashStart(player.transform);
    }

    public override void Exit()
    {
        base.Exit();
        player.skillMgr.cloneSkill.CreateCloneOnDashEnd(player.transform);
    }

    public override void Update()
    {
        base.Update();
        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        if (stateTimer <= 0)
            stateMachine.ChangeState(player.idleState);
    }
}