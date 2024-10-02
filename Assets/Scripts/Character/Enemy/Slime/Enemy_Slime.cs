using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType { big, medium, small }

public class Enemy_Slime : Enemy
{

    [Header("Slime specify")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private int childSlimeAmount;
    [SerializeField] private Vector2 maxCreateVelocity;
    [SerializeField] private Vector2 minCreateVelocity;

    #region States
    public SlimeIdleState idleState;
    public SlimeMoveState moveState;
    public SlimeBattleState battleState;
    public SlimeAttackState attackState;
    public SlimeStunnedState stunnedState;
    public SlimeDeadState deadState;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Idle", this);
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

        if (slimeType == SlimeType.small)
            return;

        CreateSlime(childSlimeAmount);
    }

    private void CreateSlime(int _slimeAmount)
    {
        for(int i=0; i<_slimeAmount; i++)
        {
            GameObject slime = Instantiate(slimePrefab, transform.position, Quaternion.identity);
            slime.GetComponent<Enemy_Slime>().SetupSlime(facingDir);
        }
    }

    public void SetupSlime(int _facingDir)
    {
        if (facingDir != _facingDir)
            Flip();

        float xVelocity = Random.Range(minCreateVelocity.x, maxCreateVelocity.x);
        float yVelocity = Random.Range(minCreateVelocity.y, maxCreateVelocity.y);

        isKnocked = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(-facingDir * xVelocity, yVelocity);

        Invoke("CancelKnockback", 0.5f);
    }

    private void CancelKnockback() => isKnocked = false;
}
