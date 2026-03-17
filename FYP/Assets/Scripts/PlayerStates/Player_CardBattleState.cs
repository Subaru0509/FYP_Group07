using UnityEngine;
using CardBattleSystem;

public class Player_CardBattleState : PlayerState
{
    public Player_CardBattleState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 停止所有移动
        player.SetVelocity(0, 0);

        Debug.Log($"{player.name} 进入卡牌战斗状态 - 移动已禁用");

        // 禁用玩家输入（卡牌战斗期间不响应移动输入）
        // moveInput会在战斗UI中被忽略
    }

    public override void Update()
    {
        base.Update();

        // 在卡牌战斗中，玩家不能移动
        // 强制保持速度为0
        player.SetVelocity(0, 0);

        // 所有输入由BattleUI和HandManager处理
        // 不处理跳跃、攻击等输入
    }

    public override void Exit()
    {
        base.Exit();

        // 战斗结束，清理格挡值
        player.ClearBlock();

        Debug.Log($"{player.name} 退出卡牌战斗状态 - 移动恢复");
    }
}
