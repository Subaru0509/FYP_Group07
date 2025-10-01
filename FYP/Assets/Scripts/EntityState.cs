using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerinputSet input;

    protected float stateTimer;
    protected bool triggerCalled;

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
        triggerCalled = false;

    }


    public virtual void Update()
    {   
        stateTimer -= Time.deltaTime;
        anim.SetFloat("yVelocity", rb.velocity.y);

        if(input.Player.Dash.WasPressedThisFrame())
            stateMachine.ChangeState(player.dashState);
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);

    }

    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }
    
    private bool CanDash()
    {   
        if(player.wallDectected)
            return false;

        if (stateMachine.currentState == player.dashState)
            return true;


        return true;
    }
}
