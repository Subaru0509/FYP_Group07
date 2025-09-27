using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
    }
    
    public override void Update()
    {
        base.Update();

        if (player.groundDetected)
        
            stateMachine.ChangeState(player.idleState);

        if (player.wallDectected)
            stateMachine.ChangeState(player.wallSlidedState);
        
    }


}
