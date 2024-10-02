using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();

    public void AnimationFinishTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats playerStats = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(playerStats);
            }
        }
    }

    public void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    public void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
