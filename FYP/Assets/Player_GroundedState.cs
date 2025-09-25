using UnityEngine;

public class Player_GroundedState : EntityState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
    }


    public override void Update()
    {
        base.Update();
        {
            base.Update();

            if(rb.velocity.y < 0)

                stateMachine.ChangeState(player.fallState);

            if (input.Player.Jump.WasPerformedThisFrame())
            
                stateMachine.ChangeState(player.jumpState);
            
        }
    }
}
