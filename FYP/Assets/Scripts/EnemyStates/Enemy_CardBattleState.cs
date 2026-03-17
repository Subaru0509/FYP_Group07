using UnityEngine;
using CardBattleSystem;

public class Enemy_CardBattleState : EnemyState
{
    public Enemy_CardBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 停止所有移动
        enemy.SetVelocity(0, 0);

        Debug.Log($"{enemy.name} 进入卡牌战斗状态");

        // TODO: 设置敌人意图（Phase 2实现）
        // enemy.SetNextIntent();
    }

    public override void Update()
    {
        base.Update();

        // 在卡牌战斗中，敌人不自主更新，由TurnSystem控制行动
        // 保持静止
        enemy.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();

        // 战斗结束，清理格挡值
        enemy.ClearBlock();

        Debug.Log($"{enemy.name} 退出卡牌战斗状态");
    }
}
