using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    #region Skills
    public Dash_Skill dashSkill { get; private set; }
    public Clone_Skill cloneSkill { get; private set; }
    public Sword_Skill swordSkill { get; private set; }
    public Blackhole_Skill blackholeSkill { get; private set; }
    public Crystal_Skill crystalSkill { get; private set; }
    #endregion

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dashSkill = GetComponent<Dash_Skill>();
        cloneSkill = GetComponent<Clone_Skill>();
        swordSkill = GetComponent<Sword_Skill>();
        blackholeSkill = GetComponent<Blackhole_Skill>();
        crystalSkill = GetComponent<Crystal_Skill>();
    }
}
