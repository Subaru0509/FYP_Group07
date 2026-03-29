using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);

        //to ensure every time entering counter attack state
        //player is able to create clone
        canCreateClone = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != this)
        {
            return;
        }

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //throw back enemy's range attack
            if(hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                SuccessfulCounterAttack();
            }

            //parry enemy's attack
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();

                if (enemy.CanBeStunnedByCounterAttack())
                {
                    SuccessfulCounterAttack();

                    //parry recover hp/fp
                    player.skill.parry.RecoverHPFPInSuccessfulParry();

                    //if availabe, will spawn clone behind enemy and attack enemy
                    if (canCreateClone)
                    {
                        //player.skill.clone.CreateCloneWithDelay(new Vector3(enemy.transform.position.x - 1.5f * enemy.facingDirection, enemy.transform.position.y), 0.1f);
                        player.skill.parry.MakeMirageInSuccessfulParry(new Vector3(enemy.transform.position.x - 1.5f * enemy.facingDirection, enemy.transform.position.y));
                        canCreateClone = false;  //can only create 1 clone every time of counter attack
                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void SuccessfulCounterAttack()
    {
        //make a random big value here,
        //cuz this state will be exited by triggerCalled if successfully counter attacked
        stateTimer = 10;

        player.anim.SetBool("SuccessfulCounterAttack", true);
        player.fx.ScreenShake(player.fx.shakeDirection_medium);
    }
}
