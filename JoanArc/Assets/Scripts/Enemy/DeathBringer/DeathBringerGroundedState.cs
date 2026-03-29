using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerGroundedState : DeathBringerState
{
    protected Transform player;

    public DeathBringerGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.CloseBossHPAndName();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            stateMachine.ChangeState(enemy.teleportState);
        }

        //if enemy can see player inside its scan range
        //or player is behind enemy
        //but he's too close to the enemy
        //enemy will hear the player's footsteps
        //and also enter battleState
        if ((enemy.IsPlayerDetected() || Vector2.Distance(player.position, enemy.transform.position) < enemy.playerHearDistance) && !player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.battleState);
            return;
        }
    }
}
