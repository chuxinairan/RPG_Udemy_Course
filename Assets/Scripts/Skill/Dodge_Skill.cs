using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot dodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage On Dodge")]
    [SerializeField] private UI_SkillTreeSlot mirageOnDodgeButton;
    public bool mirageOnDodgeUnlocked;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        dodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        mirageOnDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageOnDodge);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageOnDodge();
    }

    public void UnlockDodge()
    {
        if (dodgeButton.unlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatUI();
            dodgeUnlocked = true;
        }
    }

    public void UnlockMirageOnDodge()
    {
        if (mirageOnDodgeButton.unlocked)
            mirageOnDodgeUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector2.zero);
    }
}
