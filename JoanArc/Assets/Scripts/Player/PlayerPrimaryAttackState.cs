using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter {  get; private set; }

    private float lastTimeAttacked; //record the last attack time in order to cooperate with comboWindow
    private float comboWindow = 0.4f; //the input window to release the next combo attack

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(2, player.transform);

        if (stateMachine.currentState != player.primaryAttackState)
        {
            return;
        }
       
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDirection = player.facingDirection;
        
        //reassign value to xInput to prevent bugs that the value of xInput is not updated in time
        xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0)
        {
            attackDirection = xInput;
        }

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);

        stateTimer = 0.1f; //to keep the inertia a bit when attacking in move state
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(0.05f));

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
