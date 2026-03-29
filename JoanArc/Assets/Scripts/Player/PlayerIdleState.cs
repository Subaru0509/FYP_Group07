using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.StopSFX(14);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if player is already not in ilde state,
        //its not gonna execute the remaining code in idleState update function
        if (stateMachine.currentState != player.idleState)
        {
            return;
        }

        player.SetVelocity(0, rb.velocity.y);

        //player cannot move while in the busy condition after attack
        if (xInput != 0 && !player.isBusy)
        {
            //player cannot move towards the wall while is next to the wall
            if (!(player.IsWallDetected() && xInput == player.facingDirection))
            {
                stateMachine.ChangeState(player.moveState);
            }
        }

    }
}
