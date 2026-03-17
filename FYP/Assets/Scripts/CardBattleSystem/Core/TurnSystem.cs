using System.Collections;
using UnityEngine;

namespace CardBattleSystem
{
    public class TurnSystem : MonoBehaviour
    {
        private BattleManager battleManager;
        private EnergySystem energySystem;

        public System.Action OnPlayerTurnStart;
        public System.Action OnPlayerTurnEnd;
        public System.Action OnEnemyTurnStart;
        public System.Action OnEnemyTurnEnd;

        public void Initialize(BattleManager manager, EnergySystem energy)
        {
            battleManager = manager;
            energySystem = energy;
        }

        public void StartPlayerTurn()
        {
            if (battleManager == null) return;

            battleManager.CurrentPhase = TurnPhase.PlayerTurn;

            // 1. 恢复能量
            energySystem.RestoreEnergy();

            // 2. 清空玩家格挡
            if (battleManager.PlayerEntity != null)
            {
                battleManager.PlayerEntity.ClearBlock();
            }

            // 3. 通知其他系统（HandManager会监听这个事件来抽牌）
            OnPlayerTurnStart?.Invoke();

            Debug.Log("=== 玩家回合开始 ===");
        }

        public void EndPlayerTurn()
        {
            if (battleManager == null) return;

            battleManager.CurrentPhase = TurnPhase.PlayerTurnEnd;

            // 1. 通知其他系统（HandManager会监听这个事件来弃牌）
            OnPlayerTurnEnd?.Invoke();

            Debug.Log("=== 玩家回合结束 ===");

            // 2. 开始敌人回合
            StartCoroutine(StartEnemyTurnDelayed());
        }

        private IEnumerator StartEnemyTurnDelayed()
        {
            // 延迟0.5秒让弃牌动画播放完毕
            yield return new WaitForSeconds(0.5f);
            StartEnemyTurn();
        }

        public void StartEnemyTurn()
        {
            if (battleManager == null) return;

            battleManager.CurrentPhase = TurnPhase.EnemyTurn;
            OnEnemyTurnStart?.Invoke();

            Debug.Log("=== 敌人回合开始 ===");

            // 执行所有敌人的行动
            StartCoroutine(ExecuteAllEnemyActions());
        }

        private IEnumerator ExecuteAllEnemyActions()
        {
            if (battleManager.EnemiesInBattle == null || battleManager.EnemiesInBattle.Count == 0)
                yield break;

            foreach (var enemy in battleManager.EnemiesInBattle)
            {
                if (enemy == null) continue;

                // 清空敌人格挡
                enemy.ClearBlock();

                // 执行敌人行动
                ExecuteEnemyAction(enemy);

                // 延迟等待动画
                yield return new WaitForSeconds(1f);
            }

            // 敌人回合结束
            EndEnemyTurn();
        }

        private void ExecuteEnemyAction(Enemy enemy)
        {
            // TODO: 从敌人的行动模式获取下一个行动
            // 目前简化为固定攻击
            int damage = 10;

            Debug.Log($"{enemy.name} 对玩家造成 {damage} 点伤害");

            if (battleManager.PlayerEntity != null)
            {
                var playerHealth = battleManager.PlayerEntity.GetComponent<Entity_Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage, enemy.transform);
                }
            }
        }

        private void EndEnemyTurn()
        {
            if (battleManager == null) return;

            battleManager.CurrentPhase = TurnPhase.EnemyTurnEnd;
            OnEnemyTurnEnd?.Invoke();

            Debug.Log("=== 敌人回合结束 ===");

            // 检查胜负
            CheckBattleEnd();
        }

        private void CheckBattleEnd()
        {
            // 检查玩家是否死亡
            if (battleManager.PlayerEntity != null)
            {
                var playerHealth = battleManager.PlayerEntity.GetComponent<Entity_Health>();
                if (playerHealth != null && playerHealth.IsDead)
                {
                    battleManager.EndBattle(false);
                    return;
                }
            }

            // 检查所有敌人是否死亡
            bool allEnemiesDead = true;
            foreach (var enemy in battleManager.EnemiesInBattle)
            {
                if (enemy != null)
                {
                    var enemyHealth = enemy.GetComponent<Entity_Health>();
                    if (enemyHealth != null && !enemyHealth.IsDead)
                    {
                        allEnemiesDead = false;
                        break;
                    }
                }
            }

            if (allEnemiesDead)
            {
                battleManager.EndBattle(true);
                return;
            }

            // 继续下一个玩家回合
            StartCoroutine(StartPlayerTurnDelayed());
        }

        private IEnumerator StartPlayerTurnDelayed()
        {
            yield return new WaitForSeconds(0.5f);
            StartPlayerTurn();
        }
    }
}
