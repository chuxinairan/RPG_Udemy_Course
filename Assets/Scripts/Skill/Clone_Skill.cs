using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] public GameObject clonePrefab;
    [SerializeField] public float cloneDuration;
    [SerializeField] public float cloneFadingSpeed;
    [SerializeField] private float attackMultiplier;

    [Header("Clone Attack")]
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private UI_SkillTreeSlot canAttackButton;
    public bool canAttack;

    [Header("Aggressive Clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect;

    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multipleCloneButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [Range(0,1)]
    [SerializeField] private float duplicateProbility;
    public bool multipleCloneUnlocked;

    [Header("Crystal Instead of Clone")]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private UI_SkillTreeSlot crystalInsteadOfCloneButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        canAttackButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleCloneButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        crystalInsteadOfCloneButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
    }

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggressiveClone();
        UnlockMultipleClone();
        UnlockCrystalInsteadOfClone();
    }

    #region Unlock region
    public void UnlockCloneAttack()
    {
        if (canAttackButton.unlocked)
        {
            attackMultiplier = cloneAttackMultiplier;
            canAttack = true;
        }
    }

    public void UnlockAggressiveClone()
    {
        if (aggressiveCloneButton.unlocked)
        {
            attackMultiplier = aggressiveCloneAttackMultiplier;
            canApplyOnHitEffect = true;
        }
    }

    public void UnlockMultipleClone()
    {
        if (multipleCloneButton.unlocked)
        {
            attackMultiplier = multiCloneAttackMultiplier;
            multipleCloneUnlocked = true;
        }
    }

    public void UnlockCrystalInsteadOfClone()
    {
        if (crystalInsteadOfCloneButton.unlocked)
            crystalInsteadOfClone = true;
    }
    #endregion

    public void CreateClone(Transform _newPosition, Vector2 _offset, Transform _target = null)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystalSkill.CreateCrystal(SkillManager.instance.crystalSkill.crystalPrefab, player.transform);
            return;
        }
        GameObject clone = Instantiate(clonePrefab);
        clone.GetComponent<Clone_Skill_Controller>().SetupClone(_newPosition, cloneDuration, cloneFadingSpeed, canAttack, _offset, _target, multipleCloneUnlocked, duplicateProbility, attackMultiplier);
    }

    public void CreateCloneOnDelay(Transform _newPosition)
    {
        StartCoroutine(CloneOnDelayCoroutine(_newPosition, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CloneOnDelayCoroutine(Transform _newPosition, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_newPosition, _offset);
    }
}
