using UnityEngine;

public class Player_DashState : EntityState
{

    private float originalGravity;
    private int dashDir;
    public Player_DashState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
    }



    public override void Enter()
    {
        base.Enter();
        dashDir = player.facingDir;

        stateTimer = player.dashDuration;

        originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }
    public override void Update()
    {
        base.Update();
        CanceDashIfNeended();

        player.SetVelocity(dashDir * player.dashSpeed, 0);

        if (stateTimer < 0)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravity;
    }

    private void CanceDashIfNeended()
    {
        if (player.wallDectected)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlidedState);

        }
    }
}
