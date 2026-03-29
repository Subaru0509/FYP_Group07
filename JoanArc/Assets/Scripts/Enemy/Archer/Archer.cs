using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    [Header("Archer Specification")]
    [SerializeField] private GameObject arrowPrefab;
    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float jumpJudgeDistance; //how close player should be to archer to make archer decide to jump away
    public float lastTimeJumped { get; set; }
    [SerializeField] private float arrowFlySpeed;

    [Header("Pit check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;


    #region States
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    public ArcherStunnedState stunnedState { get; private set; }
    public ArcherDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
        stunnedState = new ArcherStunnedState(this, stateMachine, "Stunned", this);
        deathState = new ArcherDeathState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();

        InitializeLastTimeInfo();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

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

    }

    public override void GetIntoBattleState()
    {
        //if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        //{
        //    return;
        //}

        if (stateMachine.currentState != battleState && stateMachine.currentState != stunnedState && stateMachine.currentState != deathState)
        {
            stateMachine.ChangeState(battleState);
        }
    }


    //archer's special attack is shooting arrow
    public override void SpecialAttackTrigger()
    {
        //Debug.Log("shoot arrow");
        
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);

        Vector2 flyDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        Vector2 finalFlySpeed = new Vector2(flyDirection.normalized.x * arrowFlySpeed, flyDirection.normalized.y * arrowFlySpeed);
        newArrow.GetComponent<Arrow_Controller>()?.SetupArrow(finalFlySpeed, stats);
    }

    protected override void InitializeLastTimeInfo()
    {
        lastTimeAttacked = 0;
        lastTimeJumped = 0;
    }

    public bool GroundBehindCheck()
    {
        return Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }

    public bool WallBehindCheck()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDirection, wallCheckDistance + 2, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
    }

    public override RaycastHit2D IsPlayerDetected()
    {
        return Physics2D.CircleCast(wallCheck.position, playerScanDistance, Vector2.right * facingDirection, 0, whatIsPlayer);
    }
}
