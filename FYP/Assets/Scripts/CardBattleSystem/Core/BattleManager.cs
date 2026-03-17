using System.Collections.Generic;
using UnityEngine;

namespace CardBattleSystem
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance { get; private set; }

        [Header("Components")]
        [SerializeField] private TurnSystem turnSystem;
        [SerializeField] private EnergySystem energySystem;
        private DeckManager deckManager;
        private HandManager handManager;

        // 战斗状态
        public bool IsInBattle { get; private set; }
        public TurnPhase CurrentPhase { get; set; }

        // 战斗实体
        public Player PlayerEntity { get; private set; }
        public List<Enemy> EnemiesInBattle { get; private set; }

        private void Awake()
        {
            // 单例模式
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            EnemiesInBattle = new List<Enemy>();

            // 获取组件引用
            deckManager = DeckManager.Instance;
            handManager = HandManager.Instance;

            // 初始化子系统
            if (turnSystem != null && energySystem != null)
            {
                turnSystem.Initialize(this, energySystem);
            }
        }

        public void StartBattle(Player player, Enemy enemy)
        {
            if (IsInBattle)
            {
                Debug.LogWarning("已经在战斗中，无法开始新战斗");
                return;
            }

            if (deckManager == null || handManager == null)
            {
                Debug.LogError("DeckManager 或 HandManager 未初始化！请确保在场景中创建了 DeckSystemManager GameObject");
                return;
            }

            Debug.Log($"=== 战斗开始: {player.name} vs {enemy.name} ===");

            IsInBattle = true;
            PlayerEntity = player;
            EnemiesInBattle.Clear();
            EnemiesInBattle.Add(enemy);

            // 1. 切换玩家和敌人状态
            player.EnterCardBattleState();
            enemy.EnterCardBattleState();

            // 2. 初始化战斗数据
            energySystem.ResetEnergy();
            deckManager.InitializeDeck();
            handManager.ClearHand();

            // 3. TODO: 显示战斗UI
            // BattleUI.Instance.Show();

            // 4. 开始第一个玩家回合
            turnSystem.StartPlayerTurn();
        }

        public void EndPlayerTurn()
        {
            if (!IsInBattle) return;
            turnSystem.EndPlayerTurn();
        }

        public void EndBattle(bool playerWon)
        {
            if (!IsInBattle) return;

            Debug.Log($"=== 战斗结束: {(playerWon ? "玩家胜利" : "玩家失败")} ===");

            IsInBattle = false;

            if (playerWon)
            {
                // TODO: 显示奖励UI
                Debug.Log("战斗胜利！显示奖励...");
            }
            else
            {
                // 玩家死亡，触发死亡逻辑
                if (PlayerEntity != null)
                {
                    PlayerEntity.EntityDeath();
                }
            }

            // TODO: 隐藏战斗UI
            // BattleUI.Instance.Hide();

            // 恢复探索状态
            if (PlayerEntity != null)
            {
                PlayerEntity.ExitCardBattleState();
            }

            foreach (var enemy in EnemiesInBattle)
            {
                if (enemy != null)
                {
                    var enemyHealth = enemy.GetComponent<Entity_Health>();
                    if (enemyHealth == null || !enemyHealth.IsDead)
                    {
                        enemy.ExitCardBattleState();
                    }
                }
            }

            // 清理战斗数据
            PlayerEntity = null;
            EnemiesInBattle.Clear();
        }
    }
}
