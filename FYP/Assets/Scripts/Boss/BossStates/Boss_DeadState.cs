using UnityEngine;

public class Boss_DeadState : BossState
{
    public Boss_DeadState(Boss boss, StateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        boss.SetVelocity(0, 0);
        
        // 锁定状态，不能再切换
        stateMachine.SwitchCanChangeState();

        // 可以在这里触发Boss死亡事件、掉落奖励等
    }

    public override void Update()
    {
        base.Update();
        
        // Boss死亡后不做任何事
    }

    public override void Exit()
    {
        base.Exit();
    }
}
