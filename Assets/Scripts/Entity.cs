using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Knocked Info")]
    [SerializeField] public Vector2 knockedForce;
    [SerializeField] public float knockDuration;
    private bool isKnocked;

    [Header("Collision Info")]
    [SerializeField] public Transform attackCheck;
    [SerializeField] public float attackCheckRadius;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public float groundCheckDistance;
    [SerializeField] public Transform wallCheck;
    [SerializeField] public float wallCheckDistance;
    [SerializeField] public LayerMask whatIsGround;

    #region Components
    public SpriteRenderer sp { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX  entityFX { get; private set; }
    #endregion

    [HideInInspector] public float facingDir = 1;
    protected bool facingRight = true;

    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
        sp = this.GetComponentInChildren<SpriteRenderer>();
        anim = this.GetComponentInChildren<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        entityFX = this.GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Damage()
    {
        entityFX.StartCoroutine("FlashFX");
        StartCoroutine("KnockBack");
    }

    private IEnumerator KnockBack()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockedForce.x * -facingDir, knockedForce.y);
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

        Gizmos.color = Color.green;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));

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
    }

    public void FlipController(float _xVelocity)
    {
        if (_xVelocity > 0 && !facingRight)
            Flip();
        else if (_xVelocity < 0 && facingRight)
            Flip();
    }
    #endregion

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sp.color = Color.clear;
        else
            sp.color = Color.white;
    }
}
