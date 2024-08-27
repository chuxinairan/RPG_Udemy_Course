using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private float crystalExistTimer;

    private bool canExplore;
    private bool canGrow;
    private float growSpeed = 3;
    private bool canMove;
    private float moveSpeed;
    private Vector3 moveDir;
    private Transform closestTarget;
    public LayerMask whatIsEnemy;
    public void SetupCrystal(float _crystalDuration, bool _canExpolre, float _growSpeed, bool _canMove, float _moveSpeed)
    {
        crystalExistTimer = _crystalDuration;
        canExplore = _canExpolre;
        growSpeed = _growSpeed;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        moveDir = PlayerManager.instance.player.transform.right;
        closestTarget = SkillManager.instance.crystalSkill.FindClosestTarget(transform);
    }

    void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if(crystalExistTimer <=0)
        {
            CrystalFinished();
        }
        if (canMove)
        {
            if(closestTarget != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(closestTarget.position, transform.position) < 1f)
                    CrystalFinished();
            } else
            {
                transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    public void SetRandomClosestTarget(float _radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius, whatIsEnemy);
        closestTarget = colliders[Random.Range(0, colliders.Length)]?.transform;
    }

    public void CrystalFinished()
    {
        if (canExplore)
        {
            canGrow = true;
            anim.SetBool("Explore", true);
        }
        else
            Destroy(this.gameObject);
    }

    public void AnimationExploreTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    public void AnimationFinishedTrigger()
    {
        Destroy(this.gameObject);
    }
}
