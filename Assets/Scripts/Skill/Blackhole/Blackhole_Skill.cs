using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] public GameObject blackholePrefab;
    [SerializeField] public float blackholeDuration;
    [Space]
    [SerializeField] public float maxSize;
    [SerializeField] public float growSpeed;
    [SerializeField] public float shrinkSpeed;
    [Space]
    [SerializeField] public int attackAmount;
    [SerializeField] public float cloneAttackCooldown;
    

    private Blackhole_Skill_Controller currentBlackhole;
    public override void UseSkill()
    {
        base.UseSkill();
        GameObject blackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        currentBlackhole = blackhole.GetComponent<Blackhole_Skill_Controller>();
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, attackAmount, cloneAttackCooldown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
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
            return true;
        }
        return false;
    }
}
