using UnityEngine;

public class EnemyState : EntityState
{

    protected Enemy enemy;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;
        anim = enemy.anim;
        rb = enemy.rb;
    }

    public override void Enter()
    {
        base.Enter();

        anim.SetFloat("moveAnimSpeedMultipller", enemy.moveAnimSpeedMultipller);
    }
}
