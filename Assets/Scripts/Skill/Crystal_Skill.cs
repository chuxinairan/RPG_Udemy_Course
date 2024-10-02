using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] public GameObject crystalPrefab;

    [SerializeField] public float crystalDuration;

    [Header("Crystal Simple")]
    [SerializeField] private float defaultCooldown;
    [SerializeField] private UI_SkillTreeSlot crystalButton;
    public bool crystalUnlocked;

    [Header("Crystal Mirage")]
    [SerializeField] private UI_SkillTreeSlot cloneInsteadOfCrystalButton;
     public bool cloneInsteadOfCrystal;

    [Header("Crystal Explore")]
    [SerializeField] private UI_SkillTreeSlot canExploreUnlockButton;
    [SerializeField] private float growSpeed;
    public bool canExplore;

    [Header("Crystal Moving")]
    [SerializeField] private UI_SkillTreeSlot canMoveUnlockButton;
    [SerializeField] private float moveSpeed;
    public bool canMove;

    [Header("MultiCrystal")]
    [SerializeField] private UI_SkillTreeSlot canMultiCrystalUnlockButton;
    [SerializeField] private int amoutOfStack = 3;
    [SerializeField] private float multiCryCooldown;
    [SerializeField] private float useTimeWindow;
    public bool canMultiCrystal;

    private float useTimer;
    private List<GameObject> crystalLeft = new List<GameObject>();
    private GameObject currentCrystal;

    public override void UseSkill()
    {
        base.UseSkill();

        if (canMultiCrystal)
        {
            if(crystalLeft.Count == 1)
            {
                cooldown = multiCryCooldown;
                CreateCrystal(crystalLeft[crystalLeft.Count - 1], player.transform);
                crystalLeft.RemoveAt(crystalLeft.Count - 1);
                RefillCrystal();
            }
            else
            {
                cooldown = 0;
                CreateCrystal(crystalLeft[crystalLeft.Count - 1], player.transform);
                crystalLeft.RemoveAt(crystalLeft.Count - 1);
                useTimer = useTimeWindow;
            }
            return;
        }

        if (currentCrystal == null)
        {
            cooldown = 0;
            CreateCrystal(crystalPrefab, player.transform);
        }
        else
        {
            if (canMove)
                return;

            Vector3 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.cloneSkill.CreateClone(currentCrystal.transform, Vector2.zero);
                Destroy(currentCrystal.gameObject);
            }
            else
            {
                Crystal_Skill_Controller crystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
                crystalScript.CrystalFinished();
            }
            cooldown = defaultCooldown;
        }
    }

    protected override void Start()
    {
        base.Start();

        crystalButton.GetComponent<Button>().onClick.AddListener(UnlockCryStal);
        cloneInsteadOfCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCloneInsteadOfCrystal);
        canExploreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockExplore);
        canMoveUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMove);
        canMultiCrystalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlocknMultiCrystal);
    }

    protected override void CheckUnlock()
    {
        UnlockCryStal();
        UnlockCloneInsteadOfCrystal();
        UnlockExplore();
        UnlockMove();
        UnlocknMultiCrystal();
    }

    #region Unlock region
    public void UnlockCryStal()
    {
        if (crystalButton.unlocked)
            crystalUnlocked = true;
    }

    public void UnlockCloneInsteadOfCrystal()
    {
        if (cloneInsteadOfCrystalButton.unlocked)
            cloneInsteadOfCrystal = true;
    }

    public void UnlockExplore()
    {
        if (canExploreUnlockButton.unlocked)
            canExplore = true;
    }

    public void UnlockMove()
    {
        if (canMoveUnlockButton.unlocked)
            canMove = true;
    }

    public void UnlocknMultiCrystal()
    {
        if (canMultiCrystalUnlockButton.unlocked)
            canMultiCrystal = true;
    }
    #endregion

    protected override void Update()
    {
        base.Update();
        useTimer -= Time.deltaTime;
        if (useTimer <= 0)
            RefillCrystal();
    }

    public void CreateCrystal(GameObject _prefab = null, Transform _newPosition = null)
    {
        if (_prefab == null)
            _prefab = crystalPrefab;
        if (_newPosition == null)
            _newPosition = player.transform;
        currentCrystal = Instantiate(_prefab, _newPosition.position, Quaternion.identity);
        Crystal_Skill_Controller crystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        crystalScript.SetupCrystal(crystalDuration, canExplore, growSpeed, canMove, moveSpeed);
    }
    
    // ÖØÐÂÌî³äË®¾§µ¯Ï»
    private void RefillCrystal()
    {
        while(crystalLeft.Count != amoutOfStack)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    public void CurrentCrystalRandomTarget(float _radius) => currentCrystal.GetComponent<Crystal_Skill_Controller>().SetRandomClosestTarget(_radius);
}
