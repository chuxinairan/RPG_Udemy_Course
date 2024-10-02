using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    public void AnimationFinishedTrigger() => player.AnimationFinishTrigger();
    public void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(2, null);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach(Collider2D hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(enemyStats);

                WeaponEffect(hit);
            }
        }
    }

    private static void WeaponEffect(Collider2D hit)
    {
        Inventory.instance.GetEquipment(EquipmengType.Weapon)?.Effect(hit.transform);
    }

    public void ThrowSwordTrigger()
    {
        SkillManager.instance.swordSkill.CreateSword();
    }
}
