using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEnemyState : EnemyState
{
    protected TextEnemy enemy;

    public TextEnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, TextEnemy _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        triggerCalled = false;

        rb = enemyBase.rb;
    }

    public override void Exit()
    {

    }
}
