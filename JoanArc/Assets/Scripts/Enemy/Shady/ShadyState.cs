using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyState : EnemyState
{
    protected Shady enemy;
    public ShadyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
}
