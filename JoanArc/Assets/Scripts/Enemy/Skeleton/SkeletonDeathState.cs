using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathState : EnemyState
{
    private Enemy_Skeleton enemy;
    private bool canBeFliedUP = true;
    public SkeletonDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //enemy is gonna fly up before he dies
        if (stateTimer > 0 && canBeFliedUP)
        {
            enemy.SetVelocity(0, 10);
            canBeFliedUP = false;
        }
    }
}
