using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] public List<KeyCode> hotKeyList;
    [SerializeField] public GameObject hotKeyPrefab;

    private float maxSize;
    private float growSpeed;
    private bool canGrow = true;
    private float shrinkSpeed;
    private bool canShrink;

    private int attackAmount = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;
    private bool canAttack;
    private float blackHoleTimer;

    private bool canPlayerDispear = true;

    public List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKeys = new List<GameObject>();

    public bool playerCanExit { get; private set; }
    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _attackAmount, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        attackAmount = _attackAmount;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackHoleTimer = _blackholeDuration;

        if (SkillManager.instance.cloneSkill.crystalInsteadOfClone)
            canPlayerDispear = false;
    }

    private void Update()
    {
        blackHoleTimer -= Time.deltaTime;

        if (blackHoleTimer <= 0)
        {
            blackHoleTimer = Mathf.Infinity;
            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }
        if (canAttack)
        {
            if(targets.Count <= 0)
            {
                FinishBlackholeAbility();
                return;
            }
            cloneAttackTimer -= Time.deltaTime;
            if(cloneAttackTimer <= 0)
            {
                cloneAttackTimer = cloneAttackCooldown;
                // ¹¥»÷·½Ê½Ñ¡Ôñ
                if (SkillManager.instance.cloneSkill.crystalInsteadOfClone)
                {
                    SkillManager.instance.crystalSkill.CreateCrystal();
                    SkillManager.instance.crystalSkill.CurrentCrystalRandomTarget(maxSize/2);
                }
                else
                {
                    int randomIndex = Random.Range(0, targets.Count);
                    int offsetX = Random.Range(0, 2) == 0 ? -2 : 2;
                    SkillManager.instance.cloneSkill.CreateClone(targets[randomIndex], new Vector2(offsetX, 0), targets[randomIndex]);
                }

                attackAmount--;
                if(attackAmount <= 0)
                {
                    Invoke("FinishBlackholeAbility", .5f);
                }
            }
        }

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0)
                Destroy(this.gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        DestroyHotKeys();
        canAttack = true;
        cloneAttackTimer = cloneAttackCooldown;
        if (canPlayerDispear)
        {
            canPlayerDispear = false;
            PlayerManager.instance.player.fx.MakeTransparent(true);
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();
        playerCanExit = true;
        canAttack = false;
        canShrink = true;
        PlayerManager.instance.player.fx.MakeTransparent(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null || targets.Contains(collision.transform))
            return;
        collision.GetComponent<Enemy>().FreezeTimer(true);
        CreateHotKey(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTimer(false);
    }

    private void DestroyHotKeys()
    {
        if(createdHotKeys.Count > 0)
        {
            foreach(GameObject hk in createdHotKeys)
            {
                Destroy(hk);
            }
        }
    }

    private void CreateHotKey(Transform _enemyTransform)
    {
        if(hotKeyList.Count <= 0)
        {
            Debug.Log("No enough hotkey in hotkey list");
            return;
        }
        if (canAttack)
        {
            return;
        }
        GameObject hotKey = Instantiate(hotKeyPrefab, _enemyTransform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKeys.Add(hotKey);
        Blackhole_HotKey_Controller hotKetScript = hotKey.GetComponent<Blackhole_HotKey_Controller>();
        KeyCode keycode = hotKeyList[Random.Range(0, hotKeyList.Count)];
        hotKeyList.Remove(keycode);
        hotKetScript.SetuoHotKey(keycode, _enemyTransform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}

