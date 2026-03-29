using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArcherBattleState : ArcherState
{
    private Transform player;

    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //entering battleState will set the default enemy aggressiveTime
        //To prevent the case where if player approaching enemy from behind
        //enemy will get stuck in switching between idleState and battleState
        stateTimer = enemy.aggressiveTime;

        player = PlayerManager.instance.player.transform;

        //if player is attacking enemy from behind,
        //enemy will turn to player's side immediately
        FacePlayer();

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        //AudioManager.instance.StopSFX(24);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.stateMachine.currentState != this)
        {
            return;
        }


        //AudioManager.instance.PlaySFX(24, enemy.transform);

        //AudioManager.instance.PlaySFX(14, enemy.transform);

        //enemy always faces player in battle state
        //to prevent enemy from getting stuck in edge of ground
        FacePlayer();

        if (enemy.IsPlayerDetected())
        {
            //If enemy can see player, then it's always in aggreesive mode
            stateTimer = enemy.aggressiveTime;
            //Debug.Log("I see player");

            //if player is too close to archer, archer will jump if available
            if (Vector2.Distance(player.transform.position, enemy.transform.position) < enemy.jumpJudgeDistance)
            {
                if (CanJump())
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Move", false);
                    stateMachine.ChangeState(enemy.jumpState);
                    return;
                }
            }

            //if player is inside archer's attack range, archer will attack player
            if (enemy.IsPlayerDetected() && Vector2.Distance(player.transform.position, enemy.transform.position) < enemy.attackDistance)
            {

                if (CanAttack())
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Move", false);
                    stateMachine.ChangeState(enemy.attackState);
                    return;
                }
            }
        }
        else  //If enemy can't see player, 
        {
            //If enemy can't see player or player is out of enemy's scan range, enemy will switch back to patrol mode
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.playerScanDistance)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }

        //this will make enemy move towards player only when player is far from enemy's attack distance
        //or player is behind enemy
        //when enemy is close to player it'll be stopped
        if (enemy.IsPlayerDetected() && Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.attackDistance)
        {
            ChangeToIdleAnimation();
            return;
        }

    }

    private bool CanAttack()
    {
        if (Time.time - enemy.lastTimeAttacked >= enemy.attackCooldown && !enemy.isKnockbacked)
        {
            //enemy's lastTimeAttacked is set in attackState
            //enemy's attack frequency will be random
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            return true;
        }

        return false;
    }

    private bool CanJump()
    {
        //if there's a pit behind archer, or there's a wall behind acher,
        //archer will not jump away
        if (!enemy.GroundBehindCheck() || enemy.WallBehindCheck())
        {
            return false;
        }

        if (Time.time - enemy.lastTimeJumped >= enemy.jumpCooldown && !enemy.isKnockbacked)
        {
            //enemy.lastTimeJumped is set in jumpState
            return true;
        }

        return false;
    }

    private void ChangeToIdleAnimation()
    {
        anim.SetBool("Move", false);
        anim.SetBool("Idle", true);
    }

    private void FacePlayer()
    {
        if (player.transform.position.x < enemy.transform.position.x)
        {
            if (enemy.facingDirection != -1)
            {
                enemy.Flip();
            }
        }

        if (player.transform.position.x > enemy.transform.position.x)
        {
            if (enemy.facingDirection != 1)
            {
                enemy.Flip();
            }
        }
    }
}
