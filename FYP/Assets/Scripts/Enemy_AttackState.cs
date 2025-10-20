using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {


        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
