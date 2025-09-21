using UnityEngine;

public class EntityState
{
    protected StateMachine stateMachine;
    protected string statename;

    public EntityState(StateMachine stateMachine, string statename)
    {
        this.stateMachine = stateMachine;
        this.statename = statename;
    }

    public virtual void Enter()
    {
        Debug.Log("enter" + statename);

    }


    public virtual void Update()
    {
        Debug.Log("update" + statename);
    }

    public virtual void Exit()
    {
        Debug.Log("exit" + statename);

    }              
}
