using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SlimeType
{
    big,
    medium,
    small
}

public class Slime : Enemy
{
    [Header("Slime specification")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int amoutOfSlimeToSpawnAfterDeath;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minSlimeSpawnSpeed;
    [SerializeField] private Vector2 maxSlimeSpawnSpeed;

    #region States
    public SlimeIdleState idleState {  get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deathState = new SlimeDeathState(this, stateMachine, "Idle", this);

        SetupDefaultFacingDirection(-1);
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

        //small slime will not spawn any more slimes
        if (slimeType == SlimeType.small)
        {
            return;
        }

        SpawnSlime(amoutOfSlimeToSpawnAfterDeath, slimePrefab);
    }

    public override void GetIntoBattleState()
    {
        //if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        //{
        //    return;
        //}

        //player's attack will not interrupt big slime's attack
        if (slimeType == SlimeType.big && (stateMachine.currentState == battleState || stateMachine.currentState == attackState))
        {
            return;
        }

        if (stateMachine.currentState != battleState && stateMachine.currentState != stunnedState && stateMachine.currentState != deathState)
        {
            stateMachine.ChangeState(battleState);
        }
    }

    private void SpawnSlime(int _amountOfSlimeToSpawn, GameObject _slimePrefab)
    {
        for (int i = 0; i < _amountOfSlimeToSpawn; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<Slime>()?.SetupSpawnedSlime(facingDirection);
        }
    }

    public void SetupSpawnedSlime(int _facingDirection)
    {
        //if the spawned slime's facing direction
        //is not equal to its parent's facing direction,
        //flip it
        if (facingDirection != _facingDirection)
        {
            Flip();
        }

        float xVelocity = Random.Range(minSlimeSpawnSpeed.x, maxSlimeSpawnSpeed.x);
        float yVelocity = Random.Range(minSlimeSpawnSpeed.y, maxSlimeSpawnSpeed.y);

        //to prevent the slime spawn speed being interrupted
        isKnockbacked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -facingDirection, yVelocity);

        Invoke("CancelKnockback", 1.5f);
    }

    private void CancelKnockback()
    {
        isKnockbacked = false;
    }

    protected override void InitializeLastTimeInfo()
    {
        lastTimeAttacked = 0;
    }
}
