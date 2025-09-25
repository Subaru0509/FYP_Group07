using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;

    public EntityState(Player player, StateMachine stateMachine, string amimBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = amimBoolName;
        
        anim = player.anim;
        rb = player.rb;
        input = player.input;
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
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);

    }              
}
