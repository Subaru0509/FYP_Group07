using UnityEngine;

public class Player_HealState : PlayerState
{
    public Player_HealState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // 停止移动
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        // 保持静止
        player.SetVelocity(0, rb.velocity.y);

        // 等待动画触发器（动画事件调用）
        if (triggerCalled)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
