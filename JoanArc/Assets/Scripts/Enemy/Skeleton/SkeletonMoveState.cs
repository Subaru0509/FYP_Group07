using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(24);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.stateMachine.currentState != this)
        {
            return;
        }

        AudioManager.instance.PlaySFX(24, enemy.transform);
        //AudioManager.instance.PlaySFX(14, enemy.transform);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        enemy.SetVelocity(enemy.patrolMoveSpeed * enemy.facingDirection, rb.velocity.y);
    }


}
