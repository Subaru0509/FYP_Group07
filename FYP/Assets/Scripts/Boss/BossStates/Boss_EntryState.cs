using UnityEngine;

/// <summary>
/// Boss入场状态 - 播放入场动画
/// </summary>
public class Boss_EntryState : BossState
{
    public Boss_EntryState(Boss boss, StateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        boss.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        // 等待入场动画结束（通过动画事件触发）
        if (triggerCalled)
        {
            stateMachine.ChangeState(boss.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
