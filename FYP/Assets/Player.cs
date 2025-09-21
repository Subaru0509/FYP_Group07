using UnityEngine;

public class Player : MonoBehaviour
{
    //public Animator anim {  get; private set; }

    private PlayerinputSet input;
    private StateMachine stateMachine;
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }

    public Vector2 moveInput {  get; private set; }

    private void Awake()
    {   
        //anim = GetComponentInChildren<Animator>();

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
}
