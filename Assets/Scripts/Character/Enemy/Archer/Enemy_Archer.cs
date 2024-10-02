using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Archer specify")]
    public Vector2 jumpForce;
    public GameObject arrowPrefab;
    public float safeDistance;
    public float jumpCooldown;
    public float lastJumpTime = -10f;

    #region States
    public ArcherIdleState idleState;
    public ArcherMoveState moveState;
    public ArcherBattleState battleState;
    public ArcherAttackState attackState;
    public ArcherStunnedState stunnedState;
    public ArcherDeadState deadState;
    public ArcherJumpState jumpState;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ArcherBattleState(this, stateMachine, "Move", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        stunnedState = new ArcherStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ArcherDeadState(this, stateMachine, "Dead", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanStunned()
    {
        if (base.CanStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(this.deadState);
    }

    public void CreateArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        arrow.GetComponent<Arrow_Controller>().SetupArrow(stats, facingDir == 1);
    }
}
