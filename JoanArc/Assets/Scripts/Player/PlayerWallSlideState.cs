using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpcount = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != player.wallSlideState)
        {
            return;
        }

        if (!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        //if pressing down while in wallslide, speeding up the slide speed
        if (yInput < 0)
        {
            player.SetVelocity(0, rb.velocity.y);
        }
        else
        {
            //default wallslide speed
            player.SetVelocity(0, rb.velocity.y * 0.2f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if (xInput != 0 && player.facingDirection != xInput)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
