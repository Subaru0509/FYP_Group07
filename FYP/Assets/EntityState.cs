using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string statename;

    public EntityState(Player player, StateMachine stateMachine, string statename)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.statename = statename;
    }

    protected EntityState(StateMachine stateMachine, string statename)
    {
        
    }

    public virtual void Enter()
    {
        Debug.Log("I enter" + statename);

    }


    public virtual void Update()
    {
        Debug.Log("I run update" + statename);
    }

    public virtual void Exit()
    {
        Debug.Log("I exit" + statename);

    }              
}
