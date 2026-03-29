using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackState : ArcherState
{
    public ArcherAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //set time to make enemy move ahead a bit in the beginning of attack
        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            //if enemy is knockbacked then its not gonna move ahead any more
            if (enemy.isKnockbacked)
            {
                stateTimer = 0;
                return;
            }

            //enemy will move ahead a bit in the beginning of attack
            //enemy.SetVelocity(enemy.battleMoveSpeed * enemy.facingDirection, rb.velocity.y);
        }
        else
        {
            enemy.SetVelocity(0, rb.velocity.y);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }

    }
}
