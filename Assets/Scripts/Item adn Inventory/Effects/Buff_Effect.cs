using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] public StatType statType;
    [SerializeField] public float duration;
    [SerializeField] public int buffAmount;

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, duration, stats.GetStatBy(statType));
    }
}
