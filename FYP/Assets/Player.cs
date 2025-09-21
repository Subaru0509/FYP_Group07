using UnityEngine;

public class Player : MonoBehaviour
{
    private StateMachine stateMachine;
    private EntityState idleState;

    private void Awake()
    {
        stateMachine = new StateMachine();

        idleState = new EntityState(stateMachine, "idle");
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }
}
