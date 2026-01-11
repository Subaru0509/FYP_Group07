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

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            // 消耗精力进行冲刺
            if (player.stamina != null)
                player.stamina.UseStaminaForDash();
            
            stateMachine.ChangeState(player.dashState);
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.velocity.y);
    }



    private bool CanDash()
    {   
        if(player.wallDectected)
            return false;

        if (stateMachine.currentState == player.dashState)
            return false;

        // 检查是否有足够的精力进行冲刺
        if (player.stamina != null && !player.stamina.HasEnoughStaminaToDash())
            return false;

        return true;
    }
}
