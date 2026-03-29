using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowSwordState : PlayerState
{
    public PlayerThrowSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (stateMachine.currentState != player.throwSwordState)
        {
            return;
        }

        player.SetVelocity(0, rb.velocity.y);

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
