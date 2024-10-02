using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    [SerializeField] public Transform attackCheck;
    [SerializeField] public float attackCheckRadius;

    private SpriteRenderer sp;
    private Animator anim;
    private Player player;

    private float cloneFadingSpeed;
    private float fadingTimer;

    private Transform closestTarget;
    private Transform target;

    private bool canDuplicateClone;
    private float duplicateProbility;
    private float attackMultiplier;

    private float facingDir = 1;
    private void Awake()
    {
        sp = this.GetComponentInChildren<SpriteRenderer>();
        anim = this.GetComponentInChildren<Animator>();
        player = PlayerManager.instance.player.GetComponent<Player>();
    }

    public void SetupClone(Transform _newPosition, float _cloneDuration, float _cloneFadingSpeed, bool _canAttack, 
        Vector2 _offset, Transform _target, bool _canDuplicateClone, float _duplicateProbility, float _attackMultiplier)
    {
        transform.position = new Vector2(_newPosition.position.x, _newPosition.position.y) + _offset;
        fadingTimer = _cloneDuration;
        cloneFadingSpeed = _cloneFadingSpeed;
        canDuplicateClone = _canDuplicateClone;
        duplicateProbility = _duplicateProbility;
        attackMultiplier = _attackMultiplier;
        target = _target;

        if (target != null)
            FacingTarget(target);
        else
            FacingClosestEnemy();

        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }
    }

    private void Update()
    {
        fadingTimer -= Time.deltaTime;
        if(fadingTimer <= 0)
        {
            sp.color = new Color(1.0f, 1.0f, 1.0f, sp.color.a - Time.deltaTime * cloneFadingSpeed);
        }
        if(sp.color.a <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void AnimationFinishTrigger()
    {
        fadingTimer = -.1f;
    }

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                // Damage
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                playerStats.CloneDoDamage(enemyStats, attackMultiplier, transform);

                // Effects
                if (player.skillMgr.cloneSkill.canApplyOnHitEffect)
                    Inventory.instance.GetEquipment(EquipmengType.Weapon)?.Effect(hit.transform);

                // Duplicate
                if (canDuplicateClone)
                {
                    if(Random.Range(0,100) < 100 * duplicateProbility)
                    {
                        SkillManager.instance.cloneSkill.CreateClone(hit.gameObject.transform, new Vector3(2.0f * facingDir, 0));
                    }
                }
            }
        }
    }

    public void FacingClosestEnemy()
    {
        closestTarget = SkillManager.instance.cloneSkill.FindClosestTarget(transform);

        FacingTarget(closestTarget);
    }

    private void FacingTarget(Transform _target)
    {
        if (_target != null)
        {
            if (_target.position.x < transform.position.x && transform.rotation.y == 0)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
            else if (_target.position.x > transform.position.x && transform.rotation.y != 0)
            {
                facingDir = 1;
                transform.Rotate(0, 180, 0);
            }
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}
