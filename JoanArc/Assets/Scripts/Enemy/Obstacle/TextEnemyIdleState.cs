using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEnemyIdleState : TextEnemyState
{
    public TextEnemyIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, TextEnemy _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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
    }
}
