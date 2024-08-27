using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] public GameObject clonePrefab;
    [SerializeField] public float cloneDuration;
    [SerializeField] public float cloneFadingSpeed;
    [Space]
    [SerializeField] public bool canAttack;
    [SerializeField] public bool createCloneOnDashStart;
    [SerializeField] public bool createCloneOnDashEnd;
    [SerializeField] public bool createCloneOnCounterAttack;

    [Header("Clone Duplicate")]
    [SerializeField] public bool canDuplicateClone;
    [SerializeField, Range(0,1)] public float duplicateProbility;

    [Header("Crystal Instead of Clone")]
    [SerializeField] public bool crystalInsteadOfClone;

    public void CreateClone(Transform _newPosition, Vector2 _offset, Transform _target = null)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystalSkill.CreateCrystal(SkillManager.instance.crystalSkill.crystalPrefab, player.transform);
            return;
        }
        GameObject clone = Instantiate(clonePrefab);
        clone.GetComponent<Clone_Skill_Controller>().SetupClone(_newPosition, cloneDuration, cloneFadingSpeed, canAttack, _offset, _target, canDuplicateClone, duplicateProbility);
    }

    public void CreateCloneOnDashStart(Transform _newPosition)
    {
        if (createCloneOnDashStart)
            CreateClone(_newPosition, Vector2.zero);
    }
    public void CreateCloneOnDashEnd(Transform _newPosition)
    {
        if (createCloneOnDashEnd)
            CreateClone(_newPosition, Vector2.zero);
    }

    public void CreateCloneOnCounterAttack(Transform _newPosition)
    {
        if (createCloneOnCounterAttack)
            StartCoroutine(CreateCloneOnDelay(_newPosition, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneOnDelay(Transform _newPosition, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_newPosition, _offset);
    }
}
