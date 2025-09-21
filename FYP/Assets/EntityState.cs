using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected Animator anim;

    public EntityState(Player player, StateMachine stateMachine, string amimBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = amimBoolName;
        anim = player.anim;
    }

    protected EntityState(StateMachine stateMachine, string statename)
    {
        
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);

    }


    public virtual void Update()
    {
        Debug.Log("I run update" + animBoolName);
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);

    }              
}
