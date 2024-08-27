using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private GameObject sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        sword = player.sword;

        if (sword.transform.position.x < player.transform.position.x && player.facingDir == 1)
        {
            player.Flip();
        }
        else if (sword.transform.position.x > player.transform.position.x && player.facingDir == -1)
        {
            player.Flip();
        }

        player.rb.velocity = new Vector2(player.catchSwordSpeed * -player.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
