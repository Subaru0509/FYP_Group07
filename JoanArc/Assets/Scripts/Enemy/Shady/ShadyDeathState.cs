using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeathState : ShadyState
{

    public ShadyDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
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

        if (triggerCalled)
        {
            enemy.SelfDestroy();
        }
    }
}
