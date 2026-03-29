using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherState : EnemyState
{
    protected Archer enemy;

    public ArcherState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
}
