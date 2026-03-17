using System.Collections.Generic;
using UnityEngine;

namespace CardBattleSystem
{
    [System.Serializable]
    public struct CardEffectData
    {
        public CardEffectType effectType;
        public int value;
        public StatusEffectType statusType;  // 如果effectType是ApplyStatus
        public int duration;  // 状态持续回合数（0=永久）

        public CardEffectData(CardEffectType type, int val)
        {
            effectType = type;
            value = val;
            statusType = StatusEffectType.Strength;
            duration = 0;
        }
    }

    [CreateAssetMenu(fileName = "New Card", menuName = "Card Battle/Card Data")]
    public class CardData : ScriptableObject
    {
        [Header("Basic Info")]
        public int cardID;
        public string cardName;
        [TextArea(3, 5)]
        public string description;
        public Sprite cardArt;

        [Header("Card Type")]
        public CardType type = CardType.Attack;
        public CardRarity rarity = CardRarity.Common;

        [Header("Cost")]
        public int energyCost = 1;

        [Header("Effects")]
        public List<CardEffectData> effects = new List<CardEffectData>();

        [Header("Target")]
        public TargetType targetType = TargetType.SingleEnemy;

        // 是否可升级
        [Header("Upgrade")]
        public bool isUpgraded = false;
        public CardData upgradedVersion;  // 升级后的版本

        // 生成描述文本（带数值）
        public string GetDescription()
        {
            string desc = description;

            // 替换占位符为实际数值
            for (int i = 0; i < effects.Count; i++)
            {
                desc = desc.Replace($"{{value{i}}}", effects[i].value.ToString());
            }

            return desc;
        }
    }
}
