using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemy effect", menuName = "Data /Item effect/Freeze enemy effect")]
public class FreezeEnemy_Effect : ItemEffect
{
    [SerializeField] public float duration;
    [SerializeField] public float triggerThreshold;
    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (stats.currentHealth > stats.GetMaxHealthValue() * triggerThreshold)
            return;

        if (!Inventory.instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);
        foreach(Collider2D hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimerFor(duration);
        }
    }
}
