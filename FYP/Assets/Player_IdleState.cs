using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string statename) : base(player, stateMachine, statename)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x !=0)
            stateMachine.ChangeState(player.moveState);

              
        
    }

}
