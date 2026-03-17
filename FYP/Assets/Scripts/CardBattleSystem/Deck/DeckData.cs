using System.Collections.Generic;
using UnityEngine;

namespace CardBattleSystem
{
    [System.Serializable]
    public class DeckData
    {
        public List<int> cardIDs = new List<int>();

        public DeckData()
        {
            // 默认起始卡组: 5张Strike(ID=1) + 5张Defend(ID=2)
            for (int i = 0; i < 5; i++)
            {
                cardIDs.Add(1);  // Strike
                cardIDs.Add(2);  // Defend
            }
        }

        public void AddCard(int cardID)
        {
            cardIDs.Add(cardID);
        }

        public void RemoveCard(int cardID)
        {
            cardIDs.Remove(cardID);
        }

        public int GetCardCount()
        {
            return cardIDs.Count;
        }
    }
}
