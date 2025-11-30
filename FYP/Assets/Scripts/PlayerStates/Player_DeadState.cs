using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.input.Disable();
        rb.simulated = false;
    }
}
