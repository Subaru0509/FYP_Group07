using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerState : EnemyState
{
    protected DeathBringer enemy;

    public DeathBringerState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
}
