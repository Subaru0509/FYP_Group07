using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : SlimeState
{
    private float moveTimer;

    public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Slime _slime) : base(_enemyBase, _stateMachine, _animBoolName, _slime)
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
    }

    public override void Update()
    {
        base.Update();

        moveTimer -= Time.deltaTime;

        if (moveTimer < 0)
        {
            enemy.SetVelocity(0, rb.velocity.y);
        }

        if (rb.velocity.y < 0.1f && enemy.IsGroundDetected())
        {
            //StunTrigger only makes it play the animation that the slime becomes a puddle of water on ground 
            enemy.anim.SetTrigger("StunTrigger");
            enemy.fx.Invoke("CancelColorChange", 0);
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
