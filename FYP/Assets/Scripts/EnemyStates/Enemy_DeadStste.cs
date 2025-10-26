using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadStste : EnemyState
{
    public Enemy_DeadStste(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Enemy Dead State Entered");
    }
}
