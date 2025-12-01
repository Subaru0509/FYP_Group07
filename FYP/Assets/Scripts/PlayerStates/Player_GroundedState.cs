using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
    }


    public override void Update()
    {
        base.Update();
        {
            base.Update();

            if(rb.velocity.y < 0 && player.groundDetected == false)
                stateMachine.ChangeState(player.fallState);

            if (input.Player.Jump.WasPressedThisFrame())
                stateMachine.ChangeState(player.jumpState);

            if (input.Player.Attack.WasPressedThisFrame())
                stateMachine.ChangeState(player.basicAttackState);

            if(input.Player.CounterAttack.WasPressedThisFrame())
                stateMachine.ChangeState(player.counterAttackState);


        }
    }
}
