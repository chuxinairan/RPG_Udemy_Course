using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    [Header("Teleport Info")]
    [SerializeField] private BoxCollider2D teleportRegion;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport;
    private float defaultChanceToTeleport = 90;

    [Header("Spell Cast Info")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private float spellCastStateCooldown;
    public float lastTimeEnterSpellCastState;
    public int castAmount;
    public float castCooldown;

    #region States
    public DeathBringerIdleState idleState;
    public DeathBringerBattleState battleState;
    public DeathBringerAttackState attackState;
    public DeathBringerDeadState deadState;
    public DeathBringerTeleportState teleportState;
    public DeathBringerSpellCastState spellCastState;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "Idle", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
        spellCastState = new DeathBringerSpellCastState(this, stateMachine, "SpellCast", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(this.deadState);
    }

    public void FindTeleportPosition()
    {
        float x = Random.Range(teleportRegion.bounds.min.x + 3, teleportRegion.bounds.max.x - 3);
        float y = Random.Range(teleportRegion.bounds.min.y + 3, teleportRegion.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - HasGroundBelow().distance + (cd.size.y / 2));

        if (!HasGroundBelow() || HasSomethingSurrounded())
        {
            Debug.Log("Need to find new teleport position");
            FindTeleportPosition();
        }
    }

    private RaycastHit2D HasGroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);

    private RaycastHit2D HasSomethingSurrounded() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) < chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }
        return false;
    }

    public bool CanSpellCast()
    {
        if (Time.time > lastTimeEnterSpellCastState + spellCastStateCooldown)
        {
            lastTimeEnterSpellCastState = Time.time;
            return true;
        }
        return false;
    }

    public void CreateSpellCast()
    {
        Player player = PlayerManager.instance.player;
        Vector2 position;
        if (player.rb.velocity.x == 0)
            position = new Vector2(player.transform.position.x, player.transform.position.y + 1.5f);
        else
            position = new Vector2(player.transform.position.x + player.facingDir * 3, player.transform.position.y + 1.5f);
        GameObject newCast = Instantiate(spellPrefab, position, Quaternion.identity);
        newCast.GetComponent<DeathBringerSpell_Controller>().SetupSpell(stats);
    }
}
