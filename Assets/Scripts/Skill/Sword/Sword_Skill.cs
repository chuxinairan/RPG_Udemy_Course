using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] public int bounceAmount = 10;
    [SerializeField] public float bounceRadius = 5f;
    [SerializeField] public float bounceGravity;
    [SerializeField] public float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] public int pierceAmount = 10;
    [SerializeField] public float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] public float maxTravelDistance;
    [SerializeField] public float spinDuration;
    [SerializeField] public float hitCooldown;
    [SerializeField] public float spinGravity;

    [Header("Sword Info")]
    [SerializeField] public GameObject swordPrefab;
    [SerializeField] public Vector2 launchForce;
    [SerializeField] public float returnSpeed;
    [SerializeField] public float swordGravity;
    [SerializeField] public float freezeDuration;
    

    private Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] public int dotsNumbers;
    [SerializeField] public float spaceBetweenDots;
    [SerializeField] public GameObject dotPrefab;
    [SerializeField] public Transform dotsParent;
    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }

    protected override void Update()
    {
        base.Update();
        if(player.stateMachine.currentState is PlayerAimSwordState)
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
            DotsPosition(spaceBetweenDots);
            
        }
        SetGravityScale(swordType);
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
