using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [Header("Level Detail")]
    [SerializeField] private int level;
    [Range(0f, 1f)]
    [SerializeField] private float levelPercentage;

    [SerializeField] private Stat soulDropAmount;

    private Enemy enemy;
    private ItemDrop myDropSystem => GetComponent<ItemDrop>();

    protected override void Start()
    {
        enemy = GetComponent<Enemy>();
        soulDropAmount.SetDefaultValue(100);
        ApplyLevelModify();

        base.Start();
    }

    private void ApplyLevelModify()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critPower);
        Modify(critChance);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistence);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(soulDropAmount);
    }

    public void Modify(Stat _stat)
    {
        for(int i=1; i<=level; i++)
        {
            _stat.AddModifier(Mathf.RoundToInt(_stat.GetValue() * levelPercentage));
        }
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.currency += soulDropAmount.GetValue();
        myDropSystem.GenerateDrops();

        Destroy(gameObject, 5f);
    }
}
