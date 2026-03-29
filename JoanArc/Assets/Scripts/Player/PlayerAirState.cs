using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != player.airState)
        {
            return;
        }

        if (player.jumpcount < player.maxjumpcount)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.jumpcount++;
                rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
            }

        }


        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.velocity.y);
        }

        if (player.IsWallDetected() && !player.isOnPlatform)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyBindManager.instance.keybindsDictionary["Attack"]))
        {
            stateMachine.ChangeState(player.downStrikeState);
        }

        if (player.IsGroundDetected())
        {
            //fix the bug where player will get a bit stuttered when falling onto ground
            //even keeping pressing moving button
            xInput = Input.GetAxisRaw("Horizontal");

            if (xInput != 0)
            {
                stateMachine.ChangeState(player.moveState);
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}
