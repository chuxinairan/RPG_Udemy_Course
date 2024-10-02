using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : playerGroundedState
{
    private float moveSFXTimer;
    private float minimumMoveForSFX = .1f;

    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        moveSFXTimer = minimumMoveForSFX;
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(14);
    }

    public override void Update()
    {
        base.Update();

        moveSFXTimer -= Time.deltaTime;
        if(moveSFXTimer <= 0)
            AudioManager.instance.PlaySFX(14, null);

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
        if (xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
