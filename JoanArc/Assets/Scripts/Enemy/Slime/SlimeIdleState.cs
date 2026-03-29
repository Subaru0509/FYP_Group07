using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState : SlimeGroundedState
{
    public SlimeIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Slime _slime) : base(_enemyBase, _stateMachine, _animBoolName, _slime)
    {
    }


    public override void Enter()
    {
        base.Enter();

        //AudioManager.instance.StopSFX(14);
        stateTimer = enemy.patrolStayTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if enemy is already not in idleState, it's not gonna execute the remaining code
        if (stateMachine.currentState != enemy.idleState)
        {
            return;
        }

        enemy.SetVelocity(0, rb.velocity.y);

        if (stateTimer < 0)
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
