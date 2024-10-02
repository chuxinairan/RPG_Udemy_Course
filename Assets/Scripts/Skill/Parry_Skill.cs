using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreButton;
    [Range(0f, 1f)]
    [SerializeField] private float restorePercentage;
    public bool restoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageButton;
    public bool parryWithMirageUnlocked { get; private set; }
    public override void UseSkill()
    {
        base.UseSkill();
        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restorePercentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryButton.GetComponent<Button>().onClick.AddListener(() => UnlockParry());
        restoreButton.GetComponent<Button>().onClick.AddListener(() => UnlockParryRestore());
        parryWithMirageButton.GetComponent<Button>().onClick.AddListener(() => UnlockParryWithMirage());
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirage();
    }

    public void UnlockParry()
    {
        if (parryButton.unlocked)
            parryUnlocked = true;
    }

    public void UnlockParryRestore()
    {
        if (restoreButton.unlocked)
            restoreUnlocked = true;
    }

    public void UnlockParryWithMirage()
    {
        if (parryWithMirageButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirage(Transform _transform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.instance.cloneSkill.CreateCloneOnDelay(_transform);
    }
}
