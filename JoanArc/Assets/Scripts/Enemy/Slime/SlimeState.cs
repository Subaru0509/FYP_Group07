using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeState : EnemyState
{
    protected Slime enemy;
    public SlimeState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Slime _slime) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _slime;
    }
}
