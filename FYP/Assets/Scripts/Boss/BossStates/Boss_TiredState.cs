using UnityEngine;

/// <summary>
/// Boss疲劳状态 - 多次攻击后进入的虚弱状态，给玩家反击机会
/// </summary>
public class Boss_TiredState : BossState
{
    public Boss_TiredState(Boss boss, StateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        boss.SetVelocity(0, 0);
        stateTimer = boss.tiredDuration;

        // 疲劳状态下可以被反击
        boss.EnableCounterWindow(true);
    }

    public override void Update()
    {
        base.Update();

        // 保持静止
        boss.SetVelocity(0, 0);

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(boss.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        // 退出疲劳状态时重置攻击计数
        boss.ResetAttackCounter();
        boss.EnableCounterWindow(false);
    }
}
