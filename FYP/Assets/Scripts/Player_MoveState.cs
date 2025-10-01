using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string statename) : base(player, stateMachine, statename)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == 0 || player.wallDectected)
        
            stateMachine.ChangeState(player.idleState);
        

        player.SetVelocity(player.moveInput.x * player.moveSpeed, player.rb.velocity.y);
    }
}
