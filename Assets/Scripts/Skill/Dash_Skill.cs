using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashButton;
    public bool dashUnlocked { get; private set; }

    [Header("Clone on Dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on Arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalButton;
    public bool cloneOnArrivalUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        dashButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }

    public void UnlockDash()
    {
        if (dashButton.unlocked)
            dashUnlocked = true;
    }

    public void UnlockCloneOnDash()
    {
        if (cloneOnDashButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    public void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }

    public void CloneOnStart(Transform _newPosition)
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(_newPosition, Vector2.zero);
    }
    public void CloneOnArrival(Transform _newPosition)
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(_newPosition, Vector2.zero);
    }
}
