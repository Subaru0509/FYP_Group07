using UnityEngine;

public class Boss_IdleState : BossState
{
    public Boss_IdleState(Boss boss, StateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = boss.idleTime;
        boss.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        // 如果战斗已开始，检测玩家并进入移动状态
        if (boss.battleStarted)
        {
            if (stateTimer < 0)
            {
                stateMachine.ChangeState(boss.moveState);
            }
        }
        // 如果战斗未开始，检测玩家来触发战斗
        else if (boss.PlayerDetected())
        {
            boss.StartBossBattle();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
