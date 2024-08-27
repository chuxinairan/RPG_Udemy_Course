using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [Header("Return Info")]
    private float returnSpeed;
    private bool isReturning;

    [Header("Bounce Info")]
    private float bounceSpeed;
    private int bounceAmount = 10;
    private float bounceRadius = 5f;
    private List<GameObject> bounceTargets;
    private int targetIndex = 0;
    private bool isBouncing;

    [Header("Pierce Info")]
    private int pierceAmount = 2;
    private bool isPiercing;

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float hitCooldown;
    private Vector2 startPosition;
    private float spinTimer;
    private float hitTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float spinDirection;

    private float freezeDuration;

    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cb;
    private Player player;

    private bool canRotate = true;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cb = GetComponent<CircleCollider2D>();
    }
    private void DestroyMe()
    {
        if (isReturning || isPiercing || isBouncing || isSpinning)
            return;
        Destroy(this.gameObject);
    }

    public void SetupSword(Vector2 _dir, float _gravity, Player _player, float _freezeDuration, float _returnSpeed)
    {
        rb.velocity = _dir;
        rb.gravityScale = _gravity;
        player = _player;
        freezeDuration = _freezeDuration;
        returnSpeed = _returnSpeed;
        Invoke("DestroyMe", 10);
    }
    public void SetBounce(int _bounceAmount, float _bounceRadius, float _bounceSpeed, bool _isBouncing)
    {
        bounceAmount = _bounceAmount;
        bounceRadius = _bounceRadius;
        bounceSpeed = _bounceSpeed;
        isBouncing = _isBouncing;

        bounceTargets = new List<GameObject>();
        anim.SetBool("Rotation", true);
    }
    public void SetPierce(int _pierceAmount, bool _isPiercing)
    {
        pierceAmount = _pierceAmount;
        isPiercing = _isPiercing;
    }
    public void SetSpin(float _maxTravelDistance, float _spinDuration, float _hitCooldown, bool _isSpinning)
    {
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
        isSpinning = _isSpinning;
        startPosition = player.transform.position;
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
        anim.SetBool("Rotation", true);
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        // sword·µ»Ø
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 2.5f)
            {
                player.CatchSword();
            }
        }

        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(startPosition, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }
            if (wasStopped)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                hitTimer -= Time.deltaTime;
                if (hitTimer <= 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cb.radius);
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.GetComponent<Enemy>() != null)
                            collider.GetComponent<Enemy>().Damage();
                    }
                }

                spinTimer -= Time.deltaTime;
                if (spinTimer <= 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }
            }
        }
    }
    private void BounceLogic()
    {
        if (isBouncing && bounceTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, bounceTargets[targetIndex].transform.position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, bounceTargets[targetIndex].transform.position) < .1f)
            {
                bounceTargets[targetIndex].GetComponent<Enemy>().Damage();
                targetIndex++;
                bounceAmount--;
                if (targetIndex == bounceTargets.Count)
                {
                    targetIndex = 0;
                }
            }
            if (bounceAmount == 0)
            {
                isBouncing = false;
                isReturning = true;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
            return;

        Enemy enemyScript = other.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            if (isPiercing)
            {
                enemyScript.Damage();
                pierceAmount--;
                if (pierceAmount <= 0)
                {
                    isPiercing = false;
                }
                return;
            }

            if (isSpinning)
            {
                StopWhenSpinning();
                return;
            }

            if (isBouncing && bounceTargets.Count == 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(other.transform.position, bounceRadius);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.GetComponent<Enemy>() != null)
                        bounceTargets.Add(collider.gameObject);
                }
                canRotate = false;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                cb.enabled = false;
                isReturning = false;
                return;
            }
        }
        StuckInto(other);
    }

    private void StuckInto(Collider2D other)
    {
        Enemy enemyScript = other.GetComponent<Enemy>();
        if (isPiercing && enemyScript != null)
            return;

        enemyScript?.StartCoroutine("FreezeTimerFor", freezeDuration);

        canRotate = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        cb.enabled = false;
        transform.parent = other.transform;
        anim.SetBool("Rotation", false);
    }

    private void StopWhenSpinning()
    {
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        cb.enabled = false;
        spinTimer = spinDuration;
        wasStopped = true;
    }

    public void ReturnSword()
    {
        if (isBouncing)
            isBouncing = false;

        isReturning = true;
        cb.enabled = false;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.None;
        transform.parent = null;
        anim.SetBool("Rotation", true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, bounceRadius);
    }
}
