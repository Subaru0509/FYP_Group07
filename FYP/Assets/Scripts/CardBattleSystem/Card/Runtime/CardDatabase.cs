using System.Collections.Generic;
using UnityEngine;

namespace CardBattleSystem
{
    [CreateAssetMenu(fileName = "Card Database", menuName = "Card Battle/Card Database")]
    public class CardDatabase : ScriptableObject
    {
        private static CardDatabase instance;
        public static CardDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<CardDatabase>("CardDatabase");
                    if (instance == null)
                    {
                        Debug.LogError("CardDatabase not found in Resources folder!");
                    }
                }
                return instance;
            }
        }

        [SerializeField] private List<CardData> allCards = new List<CardData>();

        public CardData GetCardByID(int id)
        {
            foreach (var card in allCards)
            {
                if (card.cardID == id)
                    return card;
            }

            Debug.LogWarning($"Card with ID {id} not found!");
            return null;
        }

        public List<CardData> GetCardsByRarity(CardRarity rarity)
        {
            List<CardData> cards = new List<CardData>();
            foreach (var card in allCards)
            {
                if (card.rarity == rarity)
                    cards.Add(card);
            }
            return cards;
        }

        public List<CardData> GetCardsByType(CardType type)
        {
            List<CardData> cards = new List<CardData>();
            foreach (var card in allCards)
            {
                if (card.type == type)
                    cards.Add(card);
            }
            return cards;
        }

        public List<CardData> GetAllCards()
        {
            return new List<CardData>(allCards);
        }

        public void AddCard(CardData card)
        {
            if (!allCards.Contains(card))
            {
                allCards.Add(card);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Assign IDs")]
        private void AssignIDs()
        {
            for (int i = 0; i < allCards.Count; i++)
            {
                allCards[i].cardID = i + 1;
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
