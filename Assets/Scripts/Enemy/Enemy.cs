using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Stunned Info")]
    [SerializeField] public Vector2 stunnedForce;
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

    [SerializeField] public LayerMask whatIsEnemy;
    #region States
    public EnemyStateMachinde stateMachine;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        this.stateMachine = new EnemyStateMachinde();
    }

    protected override void Start()
    {
        base.Start();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
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

    public IEnumerator FreezeTimerFor(float _second)
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
        // Íæ¼Ò¼ì²â
        Gizmos.color = Color.white;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + playerDetectDistance * facingDir, wallCheck.position.y));
        // Íæ¼Ò¾àÀë¼ì²â
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(wallCheck.position.x, wallCheck.position.y + 0.1f), new Vector3(wallCheck.position.x + IsPlayerDetected().distance * facingDir, wallCheck.position.y + 0.1f));
        // ¹¥»÷·¶Î§¼ì²â
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + attackDistance * facingDir, wallCheck.position.y));
    }
}
