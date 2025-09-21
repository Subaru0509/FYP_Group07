using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim {  get; private set; }
    public Rigidbody2D rb { get; private set; }

    private PlayerinputSet input;
    private StateMachine stateMachine;
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }

    public Vector2 moveInput {  get; private set; }

    [Header("Movement details")]
    public float moveSpeed;


    private bool isFacingRight = true;

    private void Awake()
    {   
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        input = new PlayerinputSet();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += contex => moveInput = contex.ReadValue<Vector2>();
        input.Player.Movement.canceled += contex => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }


    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.UpdateActiveState();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    private void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && isFacingRight == false)
            Flip();
        else if (xVelocity < 0 && isFacingRight == true)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }
}
