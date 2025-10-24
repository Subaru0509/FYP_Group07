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
    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        float battleAnimSpeedMultipller = enemy.battleMoveSpeed / enemy.moveSpeed;

        anim.SetFloat("battleAnimSpeedMultipller", battleAnimSpeedMultipller);
        anim.SetFloat("moveAnimSpeedMultipller", enemy.moveAnimSpeedMultipller);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
