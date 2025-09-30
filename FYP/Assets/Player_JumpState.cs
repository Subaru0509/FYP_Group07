using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
    }

    public override void Enter()
    {
        player.SetVelocity(rb.velocity.x, player.jumpForce);
        
    }

    public override void Update()
    {
        base.Update();
        
        if (rb.velocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
        
            stateMachine.ChangeState(player.fallState);
        
    }
}
