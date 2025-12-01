using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
