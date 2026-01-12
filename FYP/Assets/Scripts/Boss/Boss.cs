using UnityEngine;

public class Boss : Entity
{
    public Boss_IdleState idleState { get; private set; }
    public Boss_MoveState moveState { get; private set; }
    public Boss_EntryState entryState { get; private set; }
    public Boss_AttackState attackState { get; private set; }
    public Boss_StunnedState stunnedState { get; private set; }
    public Boss_TiredState tiredState { get; private set; }
    public Boss_DeadState deadState { get; private set; }

    [Header("Battle Details")]
    public float battleMoveSpeed = 4f;
    public float attackDistance = 2.5f;
    public float battleTimeDuration = 8f;
    public float minRetreatDistance = 1.5f;
    public Vector2 retreatVelocity = new Vector2(5, 3);

    [Header("Attack Details")]
    public float attackCooldown = 1f;

    [Header("Attack Probability & Damage")]
    [Range(0, 100)] public float attack1Chance = 50f;
    [Range(0, 100)] public float attack2Chance = 35f;
    [Range(0, 100)] public float attack3Chance = 15f;
    public int attack1Damage = 1;
    public int attack2Damage = 2;
    public int attack3Damage = 3;
    [HideInInspector] public int currentAttackIndex = 0;
    [HideInInspector] public int currentAttackDamage = 1;

    [Header("Stunned Details")]
    public float stunnedDuration = 2f;
    public Vector2 stunnedVelocity = new Vector2(5, 5);
    [SerializeField] protected bool canBeStunned;

    [Header("Tired Details")]
    public float tiredDuration = 3f;
    public int attacksBeforeTired = 6;
    [HideInInspector] public int attackCounter = 0;

    [Header("Movement Details")]
    public float moveSpeed = 2f;
    public float idleTime = 1.5f;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 15f;

    [Header("Phase System")]
    public int maxPhase = 2;
    [HideInInspector] public int currentPhase = 1;
    public float phase2HealthThreshold = 0.5f;

    public Transform player { get; private set; }
    public bool battleStarted { get; private set; } = false;

    public void EnableCounterWindow(bool enable) => canBeStunned = enable;
    public bool CanBeStunned => canBeStunned;

    protected override void Awake()
    {
        base.Awake();

        idleState = new Boss_IdleState(this, stateMachine, "idle");
        moveState = new Boss_MoveState(this, stateMachine, "walk");
        entryState = new Boss_EntryState(this, stateMachine, "entry");
        attackState = new Boss_AttackState(this, stateMachine, "attack");
        stunnedState = new Boss_StunnedState(this, stateMachine, "stun");
        tiredState = new Boss_TiredState(this, stateMachine, "tire");
        deadState = new Boss_DeadState(this, stateMachine, "dead");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public void StartBossBattle()
    {
        if (battleStarted) return;

        battleStarted = true;
        player = FindObjectOfType<Player>()?.transform;
        stateMachine.ChangeState(entryState);
    }

    public void EnterBattleDirectly()
    {
        if (battleStarted) return;

        battleStarted = true;
        player = FindObjectOfType<Player>()?.transform;
        stateMachine.ChangeState(moveState);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);

        // 触发胜利 UI
        FindObjectOfType<VictoryUIManager>()?.ShowVictory();
    }

    public bool ShouldEnterTiredState()
    {
        return attackCounter >= attacksBeforeTired;
    }

    public void ResetAttackCounter()
    {
        attackCounter = 0;
    }

    public void IncrementAttackCounter()
    {
        attackCounter++;
    }

    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    public Transform GetPlayerReference()
    {
        if (player == null)
        {
            var playerHit = PlayerDetected();
            if (playerHit.collider != null)
                player = playerHit.transform;
            else
                player = FindObjectOfType<Player>()?.transform;
        }

        return player;
    }

    private void HandlePlayerDeath()
    {
        battleStarted = false;
        stateMachine.ChangeState(idleState);
    }

    public void HandleCounter()
    {
        if (!canBeStunned) return;

        stateMachine.ChangeState(stunnedState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (playerCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * playerCheckDistance), playerCheck.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * attackDistance), playerCheck.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * minRetreatDistance), playerCheck.position.y));
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
