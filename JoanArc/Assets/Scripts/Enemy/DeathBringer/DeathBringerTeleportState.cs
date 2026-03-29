using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : DeathBringerState
{
    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.chanceToTeleport = enemy.defaultChanceToTeleport;
        enemy.stats.BecomeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.stats.BecomeInvincible(false);
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
            if (enemy.CanCastSpell())
            {
                stateMachine.ChangeState(enemy.castState);
            }
            else
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}
