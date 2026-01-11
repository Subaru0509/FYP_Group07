using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private Player_Combat combat;
    private bool counteredSomebody;
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        counteredSomebody = combat.CounterAttackPerformed();
        anim.SetBool("counterAttackPerformed", counteredSomebody);
        stateTimer = combat.GetCounterRecoveryDuration();
    }
    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, rb.velocity.y);

        //if (counteredSomebody)
        //    anim.SetBool("counterAttackPerformed", true);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0 && counteredSomebody == false)
            stateMachine.ChangeState(player.idleState);
    }
}