using UnityEngine;

public class StateMachine
{
    public EntityState currentState { get; private set; }

    public void Initialize(EntityState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }



    public void ChangeState(EntityState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
        
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }
}

