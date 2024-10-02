using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    [Header("Stunned Info")]
    [SerializeField] public float stunnedDuration;
    [SerializeField] GameObject counterImage;
    protected bool canBeStunned;

    [Header("Player Info")]
    [SerializeField] public LayerMask whatIsPlayer;
    [SerializeField] public float playerDetectDistance;

    [Header("Move Info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float idleTime;
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    [SerializeField] public float attackDistance;
    [SerializeField] public float attackCoolDown;
    [HideInInspector] public float lastAttackTime;
    [SerializeField] public float battleTime;
    [SerializeField] public float playerAwayDiatance;

    [SerializeField] public bool isInitialLeft;

    #region States
    public EnemyStateMachinde stateMachine;
    #endregion

    public string lastAnimBoolName { get; private set; }

    public EntityFX fx { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        this.stateMachine = new EnemyStateMachinde();
    }

    protected override void Start()
    {
        base.Start();
        if (isInitialLeft)
            Flip();
        defaultMoveSpeed = moveSpeed;

        fx = GetComponent<EntityFX>();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        base.SlowEntityBy(_slowPercentage, _slowDuration);
        moveSpeed *= (1 - _slowPercentage);
        anim.speed *= (1 - _slowPercentage);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }

    public void SetLastAnimBoolName(string _lastAnimBoolName)
    {
        this.lastAnimBoolName = _lastAnimBoolName;
    }

    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public void FreezeTimer(bool _isFreezing)
    {
        if(_isFreezing)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public void FreezeTimerFor(float _second) => StartCoroutine("FreezeTimeCoroutine", _second);

    public IEnumerator FreezeTimeCoroutine(float _second)
    {
        FreezeTimer(true);
        yield return new WaitForSeconds(_second);
        FreezeTimer(false);
    }

    public virtual bool CanStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, playerDetectDistance, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(wallCheck.position.x, wallCheck.position.y + 0.2f), new Vector3(wallCheck.position.x + IsPlayerDetected().distance * facingDir, wallCheck.position.y + 0.2f));

        // ¹¥»÷·¶Î§¼ì²â
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + attackDistance * facingDir, wallCheck.position.y));
    }
}
