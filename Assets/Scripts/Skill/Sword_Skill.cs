using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin,
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce Info")]
    [SerializeField] private UI_SkillTreeSlot bounceButton;
    [SerializeField] public int bounceAmount = 10;
    [SerializeField] public float bounceRadius = 5f;
    [SerializeField] public float bounceGravity;
    [SerializeField] public float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private UI_SkillTreeSlot pierceButton;
    [SerializeField] public int pierceAmount = 10;
    [SerializeField] public float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private UI_SkillTreeSlot spinButton;
    [SerializeField] public float maxTravelDistance;
    [SerializeField] public float spinDuration;
    [SerializeField] public float hitCooldown;
    [SerializeField] public float spinGravity;

    [Header("Sword Info")]
    [SerializeField] private UI_SkillTreeSlot swordButton;
    [SerializeField] public GameObject swordPrefab;
    [SerializeField] public Vector2 launchForce;
    [SerializeField] public float returnSpeed;
    [SerializeField] public float swordGravity;
    [SerializeField] public float freezeDuration;
    public bool swordUnlocked;

    [Header("Passive Info")]
    [SerializeField] private UI_SkillTreeSlot timeStopButton;
    public bool timeStopUnlocked;
    [SerializeField] private UI_SkillTreeSlot vulnerableButton;
    public bool vulnerableUnlocked;

    private Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] public int dotsNumbers;
    [SerializeField] public float spaceBetweenDots;
    [SerializeField] public GameObject dotPrefab;
    [SerializeField] public Transform dotsParent;
    private GameObject[] dots;

    public override bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            return true;
        }
        Debug.Log("Skill is cooldown");
        return false;
    }

    public void SetSwordCooldownTimer()
    {
        cooldownTimer = cooldown;
    }

    protected override void Start()
    {
        base.Start();

        GenerateDots();
        SetGravityScale(swordType);

        swordButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        timeStopButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerablility);
        bounceButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        spinButton.GetComponent<Button>().onClick.AddListener(UnlockSpin);
    }

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockTimeStop();
        UnlockVulnerablility();
        UnlockBounce();
        UnlockPierce();
        UnlockSpin();
    }

    #region Unlock region
    public void UnlockSword()
    {
        if (swordButton.unlocked)
            swordUnlocked = true;
    }

    public void UnlockTimeStop()
    {
        if (timeStopButton.unlocked)
            timeStopUnlocked = true;
    }

    public void UnlockVulnerablility()
    {
        if (vulnerableButton.unlocked)
            vulnerableUnlocked = true;
    }

    public void UnlockBounce()
    {
        if (bounceButton.unlocked)
        {
            swordType = SwordType.Bounce;
            SetGravityScale(swordType);
        }
    }

    public void UnlockPierce()
    {
        if (pierceButton.unlocked)
        {
            swordType = SwordType.Pierce;
            SetGravityScale(swordType);
        }
    }

    public void UnlockSpin()
    {
        if (spinButton.unlocked)
        {
            swordType = SwordType.Spin;
            SetGravityScale(swordType);
        }
    }
    #endregion

    protected override void Update()
    {
        base.Update();
        if(player.stateMachine.currentState is PlayerAimSwordState)
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
            DotsPosition(spaceBetweenDots);
        }
    }

    public void CreateSword()
    {
        GameObject sword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        sword.GetComponent<Sword_Skill_Controller>().SetupSword(finalDir, swordGravity, player, freezeDuration, returnSpeed);

        player.AssignSword(sword);

        switch (swordType)
        {
            case SwordType.Regular:
                break;
            case SwordType.Bounce:
                sword.GetComponent<Sword_Skill_Controller>().SetBounce(bounceAmount, bounceRadius, bounceSpeed,true);
                break;
            case SwordType.Pierce:
                sword.GetComponent<Sword_Skill_Controller>().SetPierce(pierceAmount, true);
                break;
            case SwordType.Spin:
                sword.GetComponent<Sword_Skill_Controller>().SetSpin(maxTravelDistance, spinDuration, hitCooldown, true);
                break;
        }
    }

    private void SetGravityScale(SwordType type)
    {
        switch (swordType)
        {
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
        }
    }

    private Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition - playerPosition;
    }

    public void GenerateDots()
    {
        dots = new GameObject[dotsNumbers];
        for(int i=0; i<dotsNumbers; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    public void DotsActive(bool _isActive)
    {
        for(int i = 0; i < dotsNumbers; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    public void DotsPosition(float t)
    {
        for (int i = 0; i < dotsNumbers; i++)
        {
            float dotTime = i * t; 
            dots[i].transform.position = (Vector2)player.transform.position + new Vector2(finalDir.x * dotTime, 
                finalDir.y * dotTime + .5f * Physics2D.gravity.y * swordGravity * dotTime * dotTime);
        }
    }
}
