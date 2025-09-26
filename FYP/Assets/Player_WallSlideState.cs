using Unity.VisualScripting;
using UnityEngine;

public class Player_WallSlideState : EntityState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
    }


    public override void Update()
    {
        base.Update();
        HandleWallSlide();



        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            player.Flip();
        }
        
        
        if (player.wallDectected == false)
            stateMachine.ChangeState(player.fallState);

    }

    private void HandleWallSlide()
    {
        if (player.moveInput.y < 0)
            player.SetVelocity(player.moveInput.x, rb.velocity.y);
        else
            player.SetVelocity(player.moveInput.x, rb.velocity.y * player.wallSlideSlowMultiplier);
    }
}
