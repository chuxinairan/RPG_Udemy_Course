using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,

    damage,
    critPower,
    critChance,

    maxHealth,
    armor,
    evasion,
    magicResistence,

    fireDamage,
    iceDamage,
    lightingDamage,
}

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    [SerializeField] public Stat strength;
    [SerializeField] public Stat agility;
    [SerializeField] public Stat intelligence;
    [SerializeField] public Stat vitality;

    [Header("Defensive stats")]
    [SerializeField] public Stat damage;
    [SerializeField] public Stat critPower;
    [SerializeField] public Stat critChance;

    [Header("Defensive stats")]
    [SerializeField] public Stat maxHealth;
    [SerializeField] public Stat armor;
    [SerializeField] public Stat evasion;
    [SerializeField] public Stat magicResistence;

    [Header("Magic stats")]
    [SerializeField] public Stat fireDamage;
    [SerializeField] public Stat iceDamage;
    [SerializeField] public Stat lightingDamage;

    [Header("Aliments")]
    [SerializeField] private float alimentDuration = 4f;
    [SerializeField] private float igniteDamageCooldown;
    [SerializeField] public GameObject shockStrikePrefab;

    public bool isDead { get; private set; } = false;
    private bool isVulnerable;
    public bool Invincible;
    public int currentHealth;
    public System.Action onHealthChanged;

    private bool isIgnited;
    private bool isChilled;
    private bool isShocked;

    private float igniteTimer;
    private float chillTimer;
    private float shockTimer;

    private float igniteDamageTimer;
    private int igniteDamage;
    private int shockDamage;

    private EntityFX fx;

    protected virtual void Start()
    {
        currentHealth = GetMaxHealthValue();
        critPower.SetDefaultValue(150);
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        igniteTimer -= Time.deltaTime;
        chillTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if (igniteTimer < 0)
            isIgnited = false;

        if (chillTimer < 0)
            isChilled = false;

        if (shockTimer < 0)
            isShocked = false;

        if(isIgnited)
            ApplyIgniteDamage();
    }

    public void DoDamage(CharacterStats _targetStats)
    {
       if(CanTargetAvoid(_targetStats) || _targetStats.Invincible)
            return;

        _targetStats.GetComponent<Entity>().SetupKnockBackDir(transform);
        int totalDamgae = damage.GetValue() + strength.GetValue();

        bool isCritical = CanCritical();
        if (isCritical)
        {
            totalDamgae = CalCriticalDamage(totalDamgae);
        }
        fx.CreateHitFX(_targetStats.transform, isCritical);

        totalDamgae = CheckTargetArmor(totalDamgae, _targetStats);
        if (isCritical)
            fx.CreatePopupText(_targetStats.transform, "-" + totalDamgae.ToString(), Color.yellow);
        else
            fx.CreatePopupText(_targetStats.transform, "-" + totalDamgae.ToString(), Color.red);

        _targetStats.TakeDamage(totalDamgae);
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
            Die();
    }

    public void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }

    public void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();
        onHealthChanged?.Invoke();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;
        onHealthChanged?.Invoke();
    }

    #region Magical damage and aliments
    public void DoMagicDamage(CharacterStats _targetStats)
    {
        int fireDamage = this.fireDamage.GetValue();
        int iceDamage = this.iceDamage.GetValue();
        int lightingDamage = this.lightingDamage.GetValue();

        if (Mathf.Max(fireDamage, iceDamage, lightingDamage) <= 0)
        {
            Debug.Log("Magic Damage is 0");
            return;
        }

        int totalMagicDamage = fireDamage + iceDamage + lightingDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistence(totalMagicDamage, _targetStats);

        _targetStats.TakeDamage(totalMagicDamage);

        AttemptToApplyAliment(_targetStats, fireDamage, iceDamage, lightingDamage);
    }

    private static void AttemptToApplyAliment(CharacterStats _targetStats, int fireDamage, int iceDamage, int lightingDamage)
    {
        bool canApplyIgnite = fireDamage > iceDamage && fireDamage > lightingDamage;
        bool canApplyChill = iceDamage > fireDamage && iceDamage > lightingDamage;
        bool canApplyShock = lightingDamage > iceDamage && lightingDamage > fireDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .333f && fireDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetIgniteDamage(Mathf.RoundToInt(fireDamage * .2f));

        if (canApplyShock)
            _targetStats.SetShockDamage(Mathf.RoundToInt(lightingDamage * .4f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = true;
            igniteTimer = alimentDuration;
            fx.InvokeIgniteFor(alimentDuration);
        }
        if (_chill && canApplyChill)
        {
            isChilled = true;
            chillTimer = alimentDuration;
            fx.InvokeChillFor(alimentDuration);

            Entity entity = GetComponent<Entity>();
            float slowPercentage = .2f;
            entity.SlowEntityBy(slowPercentage, alimentDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock();
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;
                HitNearestTargetWithShockStrike();
            }
        }

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
    }
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);
            if (currentHealth <= 0 && !isDead)
                Die();
            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public void ApplyShock()
    {
        if (isShocked)
            return;

        isShocked = true;
        shockTimer = alimentDuration;
        fx.InvokeShockFor(alimentDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closeestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && hit.transform.position != transform.position)
            {
                float distance = Vector2.Distance(hit.gameObject.transform.position, transform.position);
                if (closeestDistance > distance)
                {
                    closeestDistance = distance;
                    closestTarget = hit.transform;
                }
            }

            if (closestTarget == null)
                closestTarget = transform;
        }
        if (closestTarget != null)
        {
            GameObject shockStrike = Instantiate(shockStrikePrefab, transform.position, transform.rotation);
            shockStrike.GetComponent<ShockStrike_Controller>().
                Setup(shockDamage, closestTarget.GetComponent<CharacterStats>());
        }
    }

    private void SetIgniteDamage(int _damage) => igniteDamage = _damage;
    private void SetShockDamage(int _damage) => shockDamage = _damage;
    #endregion

    #region Stats calcultions
    private int CheckTargetResistence(int _damage, CharacterStats _targetStats)
    {
        int totalResistence = _targetStats.magicResistence.GetValue() + _targetStats.intelligence.GetValue() * 3;
        return Mathf.Clamp(_damage - totalResistence, 0, int.MaxValue);
    }
    public virtual bool OnEvasion()
    {
        return true;
    }

    public bool CanTargetAvoid(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (_targetStats.isChilled)
        {
            totalEvasion = Mathf.RoundToInt(totalEvasion * .8f);
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            return _targetStats.OnEvasion();
        }
        return false;
    }

    public bool CanCritical()
    {
        if (Random.Range(0, 100) < critChance.GetValue() + agility.GetValue())
        {
            return true;
        }
        return false;
    }

    public int CalCriticalDamage(int _damage)
    {
        float totalCriticalPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        return Mathf.RoundToInt(_damage * totalCriticalPower);
    }

    public int CheckTargetArmor(int _damage, CharacterStats _targetStats)
    {
        int totalArmor = _targetStats.armor.GetValue();
        if (_targetStats.isChilled)
        {
            totalArmor = Mathf.RoundToInt(totalArmor * .8f);
        }
        return Mathf.Clamp(_damage - totalArmor, 0, int.MaxValue);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion

    protected virtual void Die()
    {
        isDead = true;
    }

    public void Killentity() => Die();

    public void MakeInvincible(bool _invincible) => Invincible = _invincible;

    public void MakeVulnerableFor(float _second) => StartCoroutine(VulnerableCoroutine(_second));

    private IEnumerator VulnerableCoroutine(float _second)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_second);
        isVulnerable = false;
    }

    public Stat GetStatBy(StatType _type)
    {
        switch (_type)
        {
            case StatType.strength: return strength;
            case StatType.agility: return agility;
            case StatType.intelligence: return intelligence;
            case StatType.vitality: return vitality;

            case StatType.damage: return damage;
            case StatType.critPower: return critPower;
            case StatType.critChance: return critChance;

            case StatType.maxHealth: return maxHealth;
            case StatType.armor: return armor;
            case StatType.evasion: return evasion;
            case StatType.magicResistence: return magicResistence;

            case StatType.fireDamage: return fireDamage;
            case StatType.iceDamage: return iceDamage;
            case StatType.lightingDamage: return lightingDamage;
        }
        return null;
    }
}
