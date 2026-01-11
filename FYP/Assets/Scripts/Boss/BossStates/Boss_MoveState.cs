using UnityEngine;

public class Boss_MoveState : BossState
{
    public Boss_MoveState(Boss boss, StateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (boss.player == null)
        {
            boss.GetPlayerReference();
            if (boss.player == null)
            {
                stateMachine.ChangeState(boss.idleState);
                return;
            }
        }

        // 如果撞墙，改变方向
        if (boss.wallDectected)
        {
            boss.Flip();
        }

        // 检查是否在攻击范围内
        if (WithinAttackRange())
        {
            stateMachine.ChangeState(boss.attackState);
            return;
        }

        // 检查是否太近需要后退
        if (ShouldRetreat())
        {
            rb.velocity = new Vector2(boss.retreatVelocity.x * -DirectionToPlayer(), boss.retreatVelocity.y);
            boss.HandleFlip(DirectionToPlayer());
            return;
        }

        // 向玩家移动
        boss.SetVelocity(boss.battleMoveSpeed * DirectionToPlayer(), rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
