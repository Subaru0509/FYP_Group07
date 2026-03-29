using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEnemyDeathState : TextEnemyState
{
    private bool canBeFliedUP = true;

    public TextEnemyDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, TextEnemy _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

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

        if (stateTimer > 0 && canBeFliedUP)
        {
            //enemy.SetVelocity(0, 10);
            enemy.rb.velocity = new Vector2(0, 10);
            enemy.textCollider.enabled = false;
            canBeFliedUP = false;
        }
    }
}
