using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shady : Enemy
{
    [Header("Shady Specification")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionMaxSize;
    [SerializeField] private float explosionGrowSpeed;

    #region States
    public ShadyIdleState idleState {  get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyAttackState attackState { get; private set; }
    public ShadyExplosionState explosionState { get; private set; }
    //public ShadyStunnedState stunnedState { get; private set; }
    public ShadyDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);
        battleState = new ShadyBattleState(this, stateMachine, "BattleMove", this);
        attackState = new ShadyAttackState(this, stateMachine, "Attack", this);
        explosionState = new ShadyExplosionState(this, stateMachine, "Explosion", this);
        //stunnedState = new ShadyStunnedState(this, stateMachine, "Stunned", this);
        deathState = new ShadyDeathState(this, stateMachine, "Death", this);
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
        //if (stateMachine.currentState != attackState)
        //{
        //    CloseCounterAttackWindow();
        //}
    }

    //public override bool CanBeStunnedByCounterAttack()
    //{
    //    if (base.CanBeStunnedByCounterAttack())
    //    {
    //        stateMachine.ChangeState(stunnedState);
    //        return true;
    //    }

    //    return false;
    //}

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

        if (stateMachine.currentState != battleState && stateMachine.currentState != deathState)
        {
            stateMachine.ChangeState(battleState);
        }
    }

    protected override void InitializeLastTimeInfo()
    {
        lastTimeAttacked = 0;
    }

    public override void SpecialAttackTrigger()
    {
        GameObject newExplosion = Instantiate(explosionPrefab, attackCheck.position, Quaternion.identity);
        newExplosion.GetComponent<ShadyExplosion_Controller>()?.SetupExplosion(stats, explosionGrowSpeed, explosionMaxSize, attackCheckRadius);

        cd.enabled = false;
        rb.gravityScale = 0;
        
        //make shady HP returns 0 when explosion damage triggers
        EnemyStats myStats = stats as EnemyStats;
        myStats.ZeroHP();
        myStats.DropCurrencyAndItem();
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
