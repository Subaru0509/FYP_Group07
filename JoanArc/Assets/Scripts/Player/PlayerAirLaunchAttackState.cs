using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirLaunchAttackState : PlayerState
{
    public bool airLaunchJumpTrigger { get; private set; } = false;

    private bool hasJumped = false;

    public PlayerAirLaunchAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        airLaunchJumpTrigger = false;
        hasJumped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.stateMachine.currentState != this)
        {
            return;
        }

        if(airLaunchJumpTrigger && !hasJumped)
        {
            player.SetVelocity(0, 17);
            hasJumped = true;
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.airState);
        }
    }

    public void SetAirLaunchJumpTrigger()
    {
        airLaunchJumpTrigger = true;
    }
}
