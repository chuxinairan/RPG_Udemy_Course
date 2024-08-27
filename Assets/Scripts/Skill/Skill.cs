using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] public float cooldown;

    private float cooldownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public bool CanUseSkill()
    {
        if(cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        Debug.Log("Skill is cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
    }

    public virtual Transform FindClosestTarget(Transform _checkPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkPosition.position, 25);
        float closeestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distance = Vector2.Distance(hit.gameObject.transform.position, _checkPosition.position);
                if (closeestDistance > distance)
                {
                    closeestDistance = distance;
                    closestTarget = hit.transform;
                }
            }
        }
        return closestTarget;
    }
}
