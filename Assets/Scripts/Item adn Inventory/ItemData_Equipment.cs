using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmengType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmengType equipmengtType;

    public ItemEffect[] itemEffects;
    private int descriptionLine;

    [Header("Major stats")]
    [SerializeField] private int strength;
    [SerializeField] private int agility;
    [SerializeField] private int intelligence;
    [SerializeField] private int vitality;

    [Header("Defensive stats")]
    [SerializeField] private int damage;
    [SerializeField] private int critPower;
    [SerializeField] private int critChance;

    [Header("Defensive stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int armor;
    [SerializeField] private int evasion;
    [SerializeField] private int magicResistence;

    [Header("Magic stats")]
    [SerializeField] private int fireDamage;
    [SerializeField] private int iceDamage;
    [SerializeField] private int lightingDamage;

    [Header("Craft required material")]
    [SerializeField] public List<InventoryItem> requiredMaterials;

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.stats as PlayerStats;
        playerStats.strength.modifiers.Add(strength);
        playerStats.agility.modifiers.Add(agility);
        playerStats.intelligence.modifiers.Add(intelligence);
        playerStats.vitality.modifiers.Add(vitality);

        playerStats.vitality.modifiers.Add(vitality);
        playerStats.critPower.modifiers.Add(critPower);
        playerStats.critChance.modifiers.Add(critChance);

        playerStats.maxHealth.modifiers.Add(maxHealth);
        playerStats.armor.modifiers.Add(armor);
        playerStats.evasion.modifiers.Add(evasion);
        playerStats.magicResistence.modifiers.Add(magicResistence);

        playerStats.fireDamage.modifiers.Add(fireDamage);
        playerStats.iceDamage.modifiers.Add(iceDamage);
        playerStats.lightingDamage.modifiers.Add(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.stats as PlayerStats;
        playerStats.strength.modifiers.Remove(strength);
        playerStats.agility.modifiers.Remove(agility);
        playerStats.intelligence.modifiers.Remove(intelligence);
        playerStats.vitality.modifiers.Remove(vitality);

        playerStats.vitality.modifiers.Remove(vitality);
        playerStats.critPower.modifiers.Remove(critPower);
        playerStats.critChance.modifiers.Remove(critChance);

        playerStats.maxHealth.modifiers.Remove(maxHealth);
        playerStats.armor.modifiers.Remove(armor);
        playerStats.evasion.modifiers.Remove(evasion);
        playerStats.magicResistence.modifiers.Remove(magicResistence);

        playerStats.fireDamage.modifiers.Remove(fireDamage);
        playerStats.iceDamage.modifiers.Remove(iceDamage);
        playerStats.lightingDamage.modifiers.Remove(lightingDamage);
    }

    public void Effect(Transform _transform)
    {
        for(int i=0; i<itemEffects.Length; i++)
        {
            itemEffects[i].ExecuteEffect(_transform);
        }
    }

    public override string GetDescription(bool _isCraft)
    {
        sb.Length = 0;
        descriptionLine = 0;
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "damage");
        AddItemDescription(critPower, "crit.Power");
        AddItemDescription(critChance, "crit.Chance");

        AddItemDescription(maxHealth, "Max.Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistence, "Magic.Resist");

        AddItemDescription(fireDamage, "fire.Dmg");
        AddItemDescription(iceDamage, "ice.Dmg");
        AddItemDescription(lightingDamage, "lighting.Dmg");

        if(descriptionLine < 5 && descriptionLine != 0)
        {
            for(int i=0; i < 5 - descriptionLine; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        if (!_isCraft)
        {
            if (itemEffectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.Append(itemEffectDescription);
            }
        }
        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            sb.Append( "+ " + _value + " " +_name);
            descriptionLine++;
        }
    }
}
