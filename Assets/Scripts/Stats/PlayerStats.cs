using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    protected override void Die()
    {
        base.Die();

        AudioManager.instance.PlaySFX(36, null, false);
        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>().GenerateDrops();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        if (!isDead)
        {
            int index = Random.Range(31, 32);
            AudioManager.instance.PlaySFX(index, null, false);
        }
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmengType.Armor);
        currentArmor?.Effect(transform);
    }

    public override bool OnEvasion()
    {
        if (!player.skillMgr.dodgeSkill.dodgeUnlocked)
            return false;

        if (player.skillMgr.dodgeSkill.mirageOnDodgeUnlocked)
            SkillManager.instance.dodgeSkill.CreateMirageOnDodge();
        return true;
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier, Transform _cloneTrans)
    {
        if (CanTargetAvoid(_targetStats))
            return;

        int totalDamgae = damage.GetValue() + strength.GetValue();

        totalDamgae = Mathf.RoundToInt(totalDamgae * _multiplier);

        if (CanCritical())
            totalDamgae = CalCriticalDamage(totalDamgae);

        totalDamgae = CheckTargetArmor(totalDamgae, _targetStats);
        _targetStats.TakeDamage(totalDamgae);
    }
}
