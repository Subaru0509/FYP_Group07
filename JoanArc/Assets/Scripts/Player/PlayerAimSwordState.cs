using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //make player slide a bit while
        //if is running before entering AimSwordState
        stateTimer = 0.1f;

        //show aim dots while entering AimSword State
        player.skill.sword.ShowDots(true);
    }

    public override void Exit()
    {
        base.Exit();

        //player can't move immediately after throwing the sword
        player.StartCoroutine(player.BusyFor(0.1f));
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != player.aimSwordState)
        {
            return;
        }

        //make player slide a bit while
        //if is running before entering AimSwordState
        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        //get mouse position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //flip player according to mouse position
        if (mousePosition.x < player.transform.position.x && player.facingDirection == 1)
        {
            player.Flip();
        }
        else if (mousePosition.x > player.transform.position.x && player.facingDirection == -1)
        {
            player.Flip();
        }


        if (Input.GetKeyUp(/*KeyCode.Mouse1*/ KeyBindManager.instance.keybindsDictionary["Aim"]))
        {
            //Not showing dots after exiting AimSwordState
            player.skill.sword.ShowDots(false);
            stateMachine.ChangeState(player.idleState);
        }

        if (Input.GetKeyDown(/*KeyCode.Mouse0*/ KeyBindManager.instance.keybindsDictionary["Attack"]))
        {
            //in ThrowSword animation will call ThrowSword()
            //which will call CreateSword()
            //which will call ShowDots(false)
            stateMachine.ChangeState(player.throwSwordState);
        }
    }
}
