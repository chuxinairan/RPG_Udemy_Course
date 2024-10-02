using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.instance.swordSkill.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.instance.swordSkill.DotsActive(false);
        player.StartCoroutine("BusyFor", .2f);
        Input.ResetInputAxes();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.idleState);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < player.transform.position.x && player.facingDir == 1)
        {
            player.Flip();
        }
        else if (mousePosition.x > player.transform.position.x && player.facingDir == -1)
        {
            player.Flip();
        }
    }
}
