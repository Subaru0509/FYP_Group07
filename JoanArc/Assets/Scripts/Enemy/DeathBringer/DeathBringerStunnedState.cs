using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerStunnedState : DeathBringerState
{
    private float moveTimer;

    public DeathBringerStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);

        stateTimer = enemy.stunDuration;
        moveTimer = 0.1f;

        rb.velocity = new Vector2(enemy.stunMovement.x * -enemy.facingDirection, enemy.stunMovement.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();

        moveTimer -= Time.deltaTime;

        if (moveTimer < 0)
        {
            enemy.SetVelocity(0, rb.velocity.y);
        }


        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
