using UnityEngine;

public class Boss : Entity
{
    // Boss States
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
    public float attackCooldown = 1f;          // 攻击冷却
    
    [Header("Attack Probability & Damage")]
    [Range(0, 100)] public float attack1Chance = 50f;   // 攻击1概率 (普通攻击)
    [Range(0, 100)] public float attack2Chance = 35f;   // 攻击2概率 (中等攻击)
    [Range(0, 100)] public float attack3Chance = 15f;   // 攻击3概率 (重击)
    public int attack1Damage = 1;              // 攻击1伤害
    public int attack2Damage = 2;              // 攻击2伤害
    public int attack3Damage = 3;              // 攻击3伤害
    [HideInInspector] public int currentAttackIndex = 0;
    [HideInInspector] public int currentAttackDamage = 1;

    [Header("Stunned Details")]
    public float stunnedDuration = 2f;
    public Vector2 stunnedVelocity = new Vector2(5, 5);
    [SerializeField] protected bool canBeStunned;

    [Header("Tired Details")]
    public float tiredDuration = 3f;           // 疲劳持续时间
    public int attacksBeforeTired = 6;         // 多少次攻击后进入疲劳
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
    public float phase2HealthThreshold = 0.5f; // 血量低于50%进入第二阶段

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

    /// <summary>
    /// 开始Boss战
    /// </summary>
    public void StartBossBattle()
    {
        if (battleStarted) return;
        
        battleStarted = true;
        player = FindObjectOfType<Player>()?.transform;
        stateMachine.ChangeState(entryState);
    }

    /// <summary>
    /// 直接进入战斗（跳过入场动画）
    /// </summary>
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
    }

    /// <summary>
    /// 检查是否应该进入疲劳状态
    /// </summary>
    public bool ShouldEnterTiredState()
    {
        return attackCounter >= attacksBeforeTired;
    }

    /// <summary>
    /// 重置攻击计数器
    /// </summary>
    public void ResetAttackCounter()
    {
        attackCounter = 0;
    }

    /// <summary>
    /// 增加攻击计数
    /// </summary>
    public void IncrementAttackCounter()
    {
        attackCounter++;
    }

    /// <summary>
    /// 检测玩家
    /// </summary>
    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    /// <summary>
    /// 获取玩家引用
    /// </summary>
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

    /// <summary>
    /// 处理玩家死亡
    /// </summary>
    private void HandlePlayerDeath()
    {
        battleStarted = false;
        stateMachine.ChangeState(idleState);
    }

    /// <summary>
    /// 处理被反击
    /// </summary>
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
