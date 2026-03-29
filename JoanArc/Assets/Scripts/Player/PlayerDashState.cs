using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CloneOnDashStart(player.transform.position);

        stateTimer = player.dashDuration;
        
        //player is invincible when dashing
        player.stats.BecomeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.velocity.y);

        player.skill.dash.CloneOnDashEnd(player.transform.position);

        //player is invincible when dashing
        player.stats.BecomeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != player.dashState)
        {
            return;
        }

        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }

        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);

        player.fx.CreateAfterimage();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
