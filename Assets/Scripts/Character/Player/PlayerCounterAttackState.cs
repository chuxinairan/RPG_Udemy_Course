using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach(Collider2D hit in colliders)
        {
            if(hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                SuccessfulCounterAttack();
            }

            if (hit.GetComponent<Enemy>() != null)
            {
                if(hit.GetComponent<Enemy>().CanStunned())
                {
                    SuccessfulCounterAttack();

                    // restore on parry
                    player.skillMgr.parrySkill.UseSkill();

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skillMgr.parrySkill.MakeMirage(hit.transform);
                    }
                }
            }
        }

        if(stateTimer <= 0 || animationFinishedCall)
        {
            // 一段时间不能格挡
            player.StartCoroutine("BusyFor", .5f);
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void SuccessfulCounterAttack()
    {
        stateTimer = 10;
        player.anim.SetBool("SuccessfulCounterAttack", true);
    }
}
