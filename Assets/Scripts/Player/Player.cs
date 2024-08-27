using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Detail")]
    [SerializeField] public Vector2[] attackMovement;
    [SerializeField] public float attackStepScale = 3f;
    [SerializeField] public float attackSpeed = 1.3f;
    [SerializeField] public float counterAttackDuration;

    [Header("Move Info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce;
    

    [Header("Dash Info")]
    [SerializeField] public float dashCooldown;
    private float dashCoolTimer;
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashDuration;
    [HideInInspector] public float dashDir;

    [Header("Skill Info")]
    [SerializeField] public float catchSwordSpeed;

    public SkillManager skillMgr { get; private set; }
    public GameObject sword { get; private set; }

    #region States
    public PlayerStateMachine stateMachine;

    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerJumpState jumpState;
    public PlayerAirState airState;
    public PlayerDashState dashState;
    public PlayerWallSlideState wallSlideState;
    public PlayerWallJumpState wallJumpState;

    public PlayerPrimaryAttackState attackState;
    public PlayerCounterAttackState counterAttackState;

    public PlayerCatchSwordState catchSwordState;
    public PlayerAimSwordState aimSwordState;

    public PlayerBlackholeState blackholeState;
    #endregion

    public bool isBusy { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        this.stateMachine   =   new PlayerStateMachine();

        this.idleState      =   new PlayerIdleState(this, stateMachine, "Idle");
        this.moveState      =   new PlayerMoveState(this, stateMachine, "Move");
        this.jumpState      =   new PlayerJumpState(this, stateMachine, "Jump");
        this.airState       =   new PlayerAirState(this, stateMachine, "Jump");
        this.dashState      =   new PlayerDashState(this, stateMachine, "Dash");
        this.wallSlideState =   new PlayerWallSlideState(this, stateMachine, "WallSlide");
        this.wallJumpState  =   new PlayerWallJumpState(this, stateMachine, "Jump");

        this.attackState    =   new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        this.counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        this.catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        this.aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        this.blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
    }

    protected override void Start()
    {
        base.Start();
        skillMgr = SkillManager.instance;
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F))
        {
            skillMgr.crystalSkill.CanUseSkill();
        }
    }

    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void AssignSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _second)
    {
        // aimSword、counterAttack
        isBusy = true;
        yield return new WaitForSeconds(_second);
        isBusy = false;
    }

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        dashCoolTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dashSkill.CanUseSkill())
        {
            dashCoolTimer = dashCooldown;

            dashDir = Input.GetAxisRaw("Horizontal");  // 在jump状态的时候改变dash方向
            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }
}