using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerinputSet input;
    

    public PlayerState(Player player, StateMachine stateMachine, string amimBoolName) : base(stateMachine, amimBoolName)
    {
        this.player = player;
        
        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public override void Update()
    {
        base.Update();
        anim.SetFloat("yVelocity", rb.velocity.y);

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
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
