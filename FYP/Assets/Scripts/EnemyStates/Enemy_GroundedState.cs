using UnityEngine;
using CardBattleSystem;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected() == true)
        {
            // 切换为卡牌战斗模式
            Transform playerTransform = enemy.GetPlayerReference();
            if (playerTransform != null)
            {
                Player player = playerTransform.GetComponent<Player>();
                if (player != null && BattleManager.Instance != null)
                {
                    BattleManager.Instance.StartBattle(player, enemy);
                }
                else
                {
                    // BattleManager未初始化，回退到原有战斗模式
                    Debug.LogWarning("BattleManager未找到，使用原有战斗模式");
                    stateMachine.ChangeState(enemy.battleState);
                }
            }
        }
    }
}
