using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGroundedState : PlayerState
{
    public playerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        // �ڶ�����
        if (Input.GetKeyDown(KeyCode.R) && player.skillMgr.blackholeSkill.blackholeUnlocked && SkillManager.instance.blackholeSkill.CanUseSkill())
            stateMachine.ChangeState(player.blackholeState);

        // Ͷ��
        if (Input.GetKeyDown(KeyCode.Q) && HasNoSword() && player.skillMgr.swordSkill.swordUnlocked && SkillManager.instance.swordSkill.CanUseSkill())
            stateMachine.ChangeState(player.aimSwordState);

        // �񵲷���
        if (Input.GetMouseButton(1) && !player.isBusy && player.skillMgr.parrySkill.parryUnlocked && SkillManager.instance.parrySkill.CanUseSkill())
                stateMachine.ChangeState(player.counterAttackState);

        // ����
        if (Input.GetMouseButton(0))
            stateMachine.ChangeState(player.attackState);

        // ��ؼ��
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        // ��Ծ
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }

    private bool HasNoSword()
    {
        if(!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
