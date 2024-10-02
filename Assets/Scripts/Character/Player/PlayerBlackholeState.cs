using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .3f;
    private bool skillused;
    private float defaultGravityScale;
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = flyTime;
        skillused = false;
        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultGravityScale;
        UI.instance.inGameUI.GetComponent<UI_InGame>().SetBlackholeCooldown();
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        } else
        {
            rb.velocity = new Vector2(0, -.1f);
            if (!skillused)
            {
                SkillManager.instance.blackholeSkill.UseSkill();
                skillused = true;
            }
        }

        if (SkillManager.instance.blackholeSkill.BlackholeFinished())
            stateMachine.ChangeState(player.airState);
    }
}
