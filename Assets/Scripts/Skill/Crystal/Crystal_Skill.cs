using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] public GameObject crystalPrefab;

    [SerializeField] public float crystalDuration;

    [Header("Crystal Mirage")]
    [SerializeField] public bool CloneInsteadOfCrystal;

    [Header("Crystal Explore")]
    [SerializeField] public bool canExplore;
    [SerializeField] public float growSpeed;

    [Header("Crystal Moving")]
    [SerializeField] public bool canMove;
    [SerializeField] public float moveSpeed;

    [Header("MultiCrystal")]
    [SerializeField] public bool canMultiCrystal;
    [SerializeField] public int amoutOfStack = 3;
    [SerializeField] public float multiCryCooldown;
    [SerializeField] public float useTimeWindow;

    private float useTimer;
    private List<GameObject> crystalLeft = new List<GameObject>();
    private GameObject currentCrystal;

    protected override void Update()
    {
        base.Update();
        useTimer -= Time.deltaTime;
        if (useTimer <= 0)
            RefillCrystal();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (canMultiCrystal)
        {
            if (crystalLeft.Count <= 0)
            {
                cooldown = multiCryCooldown;
                RefillCrystal();
            } else
            {
                cooldown = 0;
                CreateCrystal(crystalLeft[crystalLeft.Count - 1], player.transform);
                crystalLeft.RemoveAt(crystalLeft.Count - 1);
                useTimer = useTimeWindow;
            }
            return;
        }

        if(currentCrystal == null)
        {
            CreateCrystal(crystalPrefab, player.transform);
        }
        else
        {
            if (canMove)
                return;

            Vector3 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (CloneInsteadOfCrystal)
            {
                SkillManager.instance.cloneSkill.CreateClone(currentCrystal.transform, Vector2.zero);
                Destroy(currentCrystal.gameObject);
            }
            else
            {
                Crystal_Skill_Controller crystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
                crystalScript.CrystalFinished();
            }
        }
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
