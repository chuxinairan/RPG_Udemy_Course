using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot BlackholeButton;
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [Space]
    [SerializeField] private int attackAmount;
    [SerializeField] private float cloneAttackCooldown;
    public bool blackholeUnlocked;

    private Blackhole_Skill_Controller currentBlackhole;

    public override bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            return true;
        }
        Debug.Log("Skill is cooldown");
        return false;
    }

    public override void UseSkill()
    {
        GameObject blackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        currentBlackhole = blackhole.GetComponent<Blackhole_Skill_Controller>();
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, attackAmount, cloneAttackCooldown, blackholeDuration);

        AudioManager.instance.PlaySFX(3, null);
        AudioManager.instance.PlaySFX(6, null);
    }

    protected override void Start()
    {
        base.Start();
        BlackholeButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void CheckUnlock()
    {
        UnlockBlackhole();
    }

    public void UnlockBlackhole()
    {
        if (BlackholeButton.unlocked)
            blackholeUnlocked = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool BlackholeFinished()
    {
        if (!currentBlackhole)
            return false;

        if (currentBlackhole.playerCanExit)
        {
            currentBlackhole = null;
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }
}
