using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpAttackState : EntityState
{   

    private bool touchedGround;
    public Player_JumpAttackState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        touchedGround = false;

        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.groundDetected && touchedGround == false)
        {
            touchedGround = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0,rb.velocity.y);
        }

        if(triggerCalled && player.groundDetected)
            stateMachine.ChangeState(player.idleState);
        }
}
