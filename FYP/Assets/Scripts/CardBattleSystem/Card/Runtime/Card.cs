using UnityEngine;

namespace CardBattleSystem
{
    public class Card : MonoBehaviour
    {
        public CardData data { get; private set; }
        // TODO: CardUI组件引用
        // private CardUI ui;

        public void Initialize(CardData cardData)
        {
            data = cardData;
            // TODO: ui = GetComponent<CardUI>();
            // TODO: ui.UpdateDisplay(data);
        }

        public bool CanPlay()
        {
            if (EnergySystem.Instance == null)
                return false;

            return EnergySystem.Instance.CanSpend(data.energyCost);
        }

        public void Play(Entity target)
        {
            if (!CanPlay())
            {
                Debug.LogWarning($"能量不足，无法打出 {data.cardName}");
                return;
            }

            // 扣除能量
            var energySystem = FindObjectOfType<EnergySystem>();
            if (energySystem != null)
            {
                energySystem.SpendEnergy(data.energyCost);
            }

            Debug.Log($"=== 打出卡牌: {data.cardName} ===");

            // 执行所有效果
            foreach (var effectData in data.effects)
            {
                ExecuteEffect(effectData, target);
            }

            // TODO: 播放卡牌特效和音效
            // PlayCardEffects();
        }

        private void ExecuteEffect(CardEffectData effectData, Entity target)
        {
            switch (effectData.effectType)
            {
                case CardEffectType.Damage:
                    ApplyDamage(target, effectData.value);
                    break;

                case CardEffectType.Block:
                    ApplyBlock(effectData.value);
                    break;

                case CardEffectType.Draw:
                    DrawCards(effectData.value);
                    break;

                case CardEffectType.Heal:
                    Heal(effectData.value);
                    break;

                case CardEffectType.ApplyStatus:
                    // TODO: 实现状态效果系统
                    Debug.Log($"施加状态效果: {effectData.statusType} x{effectData.value}");
                    break;
            }
        }

        private void ApplyDamage(Entity target, int damage)
        {
            if (target == null)
            {
                Debug.LogWarning("目标为空，无法造成伤害");
                return;
            }

            var targetHealth = target.GetComponent<Entity_Health>();
            if (targetHealth != null)
            {
                Debug.Log($"{data.cardName} 对 {target.name} 造成 {damage} 点伤害");
                targetHealth.TakeDamage(damage, null);
            }
        }

        private void ApplyBlock(int blockAmount)
        {
            var player = BattleManager.Instance?.PlayerEntity;
            if (player != null)
            {
                player.AddBlock(blockAmount);
                Debug.Log($"{data.cardName} 获得 {blockAmount} 点格挡");
            }
        }

        private void DrawCards(int count)
        {
            var handManager = HandManager.Instance;
            if (handManager != null)
            {
                handManager.DrawCards(count);
                Debug.Log($"{data.cardName} 抽取 {count} 张牌");
            }
        }

        private void Heal(int healAmount)
        {
            var player = BattleManager.Instance?.PlayerEntity;
            if (player != null)
            {
                var playerHealth = player.GetComponent<Entity_Health>();
                if (playerHealth != null)
                {
                    playerHealth.IncreaseHealth(healAmount);
                    Debug.Log($"{data.cardName} 恢复 {healAmount} 点生命");
                }
            }
        }

        // TODO: 播放卡牌特效
        // private void PlayCardEffects() { }
    }
}
