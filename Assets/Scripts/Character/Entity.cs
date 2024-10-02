using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Knocked Info")]
    [SerializeField] private Vector2 knockedForce;
    [SerializeField] private Vector2 knockbackRandomRange;
    [SerializeField] private float knockDuration;
    private int knockBackDir;
    protected bool isKnocked;

    [Header("Collision Info")]
    [SerializeField] public Transform attackCheck;
    [SerializeField] public float attackCheckRadius;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public float groundCheckDistance;
    [SerializeField] public Transform wallCheck;
    [SerializeField] public float wallCheckDistance;
    [SerializeField] public LayerMask whatIsGround;

    #region Components
    public SpriteRenderer sr { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [HideInInspector] public int facingDir = 1;
    public System.Action onFlipped;
    protected bool facingRight = true;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        sr = this.GetComponentInChildren<SpriteRenderer>();
        anim = this.GetComponentInChildren<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        stats = this.GetComponent<CharacterStats>();
        cd = this.GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public void SetupKnockBackDir(Transform _soureceTransform)
    {
        knockBackDir = _soureceTransform.position.x > transform.position.x ? -1 : 1;
    }

    public virtual void DamageImpact() => StartCoroutine("KnockBack");

    private IEnumerator KnockBack()
    {
        isKnocked = true;
        Vector2 knockbackOffset = new Vector2(Random.Range(knockbackRandomRange.x, knockbackRandomRange.y), Random.Range(knockbackRandomRange.x, knockbackRandomRange.y));
        Vector2 knockbackVelocity = knockedForce + knockbackOffset;
        rb.velocity = new Vector2(knockbackVelocity.x * knockBackDir, knockbackVelocity.y);
        yield return new WaitForSeconds(knockDuration);
        isKnocked = false;
    }

    #region Velocity
    public virtual void SetZeroVelocity() 
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(0, 0);
    } 

    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(wallCheck.position.x, wallCheck.position.y + 0.1f), new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y + 0.1f));

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        this.transform.Rotate(0, 180, 0);

        onFlipped?.Invoke();
    }

    public void FlipController(float _xVelocity)
    {
        if (_xVelocity > 0 && !facingRight)
            Flip();
        else if (_xVelocity < 0 && facingRight)
            Flip();
    }
    #endregion

    public virtual void Die()
    {
    }
}
