using UnityEngine;

public class Boss_StunnedState : BossState
{
    public Boss_StunnedState(Boss boss, StateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        boss.EnableCounterWindow(false);

        stateTimer = boss.stunnedDuration;
        
        // 被击退效果
        rb.velocity = new Vector2(boss.stunnedVelocity.x * -boss.facingDir, boss.stunnedVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(boss.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
