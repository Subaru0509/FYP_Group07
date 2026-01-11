using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public static event Action OnPlayerDeath;
    public PlayerinputSet input { get; private set; }
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlidedState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_HealState healState { get; private set; }

    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;

    public float moveSpeed;
    public float jumpForce = 5;
    public Vector2 wallJumpForce;
    [Range(0, 1)]
    public float inAirMoveMultiplier = .7f;
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = .7f;
    public float dashDuration = .25f;
    public float dashSpeed = 20;
    public Vector2 moveInput { get; private set; }

    public PlayerStamina stamina { get; private set; }
    private Entity_Health health;
    private InputAction usePotionAction;

    protected override void Awake()
    {
        base.Awake();
        input = new PlayerinputSet();
        stamina = GetComponent<PlayerStamina>();
        health = GetComponent<Entity_Health>();

        usePotionAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/r");
        usePotionAction.performed += ctx => UsePotion();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlidedState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        healState = new Player_HealState(this, stateMachine, "heal");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);
        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += contex => moveInput = contex.ReadValue<Vector2>();
        input.Player.Movement.canceled += contex => moveInput = Vector2.zero;
        usePotionAction.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
        usePotionAction.Disable();
    }

    private void UsePotion()
    {
        // 检查是否可以使用药水（不在治疗状态、死亡状态等）
        if (stateMachine.currentState == healState || 
            stateMachine.currentState == deadState ||
            stateMachine.currentState == dashState)
            return;

        bool used = UIManager.Instance.UsePotion();
        if (used && health != null)
        {
            // 切换到治疗状态播放动画
            stateMachine.ChangeState(healState);
            health.IncreaseHealth(20);
        }
    }
}
