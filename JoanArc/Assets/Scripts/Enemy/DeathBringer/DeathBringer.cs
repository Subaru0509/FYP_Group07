using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringer : Enemy
{
    [Header("Teleport Info")]
    [SerializeField] private BoxCollider2D teleportRegion;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float defaultChanceToTeleport;
    public float chanceToTeleport { get; set; }

    [Header("Spell Cast Info")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private float spellCastStateCooldown;
    public float lastTimeEnterSpellCastState { get; set; }
    public int castAmount;
    public float castCooldown;

    [Header("Boss Fight Info")]
    [SerializeField] private GameObject bossNameAndHPUI;
    public int stage { get; set; } = 1;

    #region States
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerMoveState moveState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    public DeathBringerCastState castState { get; private set; }
    public DeathBringerStunnedState stunnedState { get; private set; }
    public DeathBringerDeathState deathState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        moveState = new DeathBringerMoveState(this, stateMachine, "Move", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
        castState = new DeathBringerCastState(this, stateMachine, "Cast", this);
        stunnedState = new DeathBringerStunnedState(this, stateMachine, "Idle", this);
        deathState = new DeathBringerDeathState(this, stateMachine, "Idle", this);

        SetupDefaultFacingDirection(-1);
    }

    protected override void Start()
    {
        base.Start();

        InitializeLastTimeInfo();
        chanceToTeleport = defaultChanceToTeleport;
        stage = 1;

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        //if boss's current HP is below 60%, entering stage 2
        if (stats.currentHP <= stats.getMaxHP() * 0.6f)
        {
            stage = 2;
        }

        //to prevent counter image from always showing when skeleton's attack got interrupted
        if (stateMachine.currentState != attackState)
        {
            CloseCounterAttackWindow();
        }
    }

    public override bool CanBeStunnedByCounterAttack()
    {
        if (base.CanBeStunnedByCounterAttack())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deathState);

        //english
        if (LanguageManager.instance.localeID == 0)
        {
            UI.instance.SwitchToThankYouForPlaying("Achieved ending - Breaking the 4th wall");

        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            UI.instance.SwitchToThankYouForPlaying("达成结局 — 打破第四面墙");
        }
    }

    public override void GetIntoBattleState()
    {
        //player cannot interrunt deathbringer's attack or spell
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState || stateMachine.currentState == castState)
        {
            return;
        }

        if (stateMachine.currentState != battleState && stateMachine.currentState != stunnedState && stateMachine.currentState != deathState)
        {
            stateMachine.ChangeState(battleState);
        }
    }

    protected override void InitializeLastTimeInfo()
    {
        lastTimeAttacked = 0;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - HasGroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

    private RaycastHit2D HasGroundBelow()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    }

    private RaycastHit2D HasSomethingSurrounded()
    {
        return Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);
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

    public bool CanTeleport()
    {
        if (stage == 1)
        {
            return false;
        }

        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            return true;
        }

        return false;
    }

    public bool CanCastSpell()
    {
        if (stage == 1)
        {
            return false;
        }

        if (Time.time - lastTimeEnterSpellCastState >= spellCastStateCooldown)
        {
            return true;
        }

        return false;
    }

    public void CastSpell()
    {
        Vector3 spellSpawnPosition;

        //spell will predict player's movement to decide its spawn position
        //if player is moving, spell will spawn in front of player;
        if (player.rb.velocity.x != 0)
        {
            spellSpawnPosition = new Vector3(player.transform.position.x + player.facingDirection * 3, player.transform.position.y + 1.65f);
        }
        //if player is not moving, spell will spawn right above player
        else
        {
            spellSpawnPosition = new Vector3(player.transform.position.x, player.transform.position.y + 1.65f);
        }

        GameObject newSpell = Instantiate(spellPrefab, spellSpawnPosition, Quaternion.identity);
        newSpell.GetComponent<SpellController>()?.SetupSpell(stats);
    }

    public void ShowBossHPAndName()
    {
        bossNameAndHPUI.SetActive(true);
    }

    public void CloseBossHPAndName()
    {
        bossNameAndHPUI.SetActive(false);
    }
}
