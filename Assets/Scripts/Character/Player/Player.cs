using System.Collections;
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
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash Info")]
    [SerializeField] public float dashCooldown;
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashDuration;
    [HideInInspector] public float dashDir;
    private float dashCoolTimer;
    private float defaultDashSpeed;

    [Header("Skill Info")]
    [SerializeField] public float catchSwordSpeed;

    public PlayerFX fx { get; private set; }
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

    public PlayerDeadState deadState;
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

        this.deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();
        skillMgr = SkillManager.instance;
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

        fx = GetComponent<PlayerFX>();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F) && skillMgr.crystalSkill.crystalUnlocked)
        {
            skillMgr.crystalSkill.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        base.SlowEntityBy(_slowPercentage, _slowDuration);
        moveSpeed *= (1 - _slowPercentage);
        jumpForce *= (1 - _slowPercentage);
        dashSpeed *= (1 - _slowPercentage);
        anim.speed *= (1 - _slowPercentage);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
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

        if (SkillManager.instance.dashSkill.dashUnlocked == false)
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

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(this.deadState);
    }
}