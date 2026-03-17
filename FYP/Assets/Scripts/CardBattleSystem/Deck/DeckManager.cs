using System.Collections.Generic;
using UnityEngine;

namespace CardBattleSystem
{
    public class DeckManager : MonoBehaviour
    {
        public static DeckManager Instance { get; private set; }

        [Header("Deck Data")]
        [SerializeField] private DeckData masterDeck;  // 主卡组（所有拥有的卡牌）

        [Header("Piles")]
        private List<CardData> drawPile = new List<CardData>();        // 抽牌堆
        private List<CardData> discardPile = new List<CardData>();    // 弃牌堆
        private List<CardData> exhaustPile = new List<CardData>();    // 消耗堆

        public int DrawPileCount => drawPile.Count;
        public int DiscardPileCount => discardPile.Count;
        public int ExhaustPileCount => exhaustPile.Count;

        public System.Action<int> OnDrawPileChanged;
        public System.Action<int> OnDiscardPileChanged;

        private void Awake()
        {
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

            // 如果没有数据，创建默认卡组
            if (masterDeck == null)
            {
                masterDeck = new DeckData();
            }
        }

        public void InitializeDeck()
        {
            // 清空所有牌堆
            drawPile.Clear();
            discardPile.Clear();
            exhaustPile.Clear();

            // 从主卡组加载卡牌到抽牌堆
            foreach (int cardID in masterDeck.cardIDs)
            {
                CardData card = CardDatabase.Instance.GetCardByID(cardID);
                if (card != null)
                {
                    drawPile.Add(card);
                }
            }

            // 洗牌
            ShuffleDrawPile();

            OnDrawPileChanged?.Invoke(drawPile.Count);
            OnDiscardPileChanged?.Invoke(discardPile.Count);

            Debug.Log($"卡组初始化完成: 抽牌堆 {drawPile.Count} 张");
        }

        public CardData DrawCard()
        {
            // 抽牌堆为空，重新洗牌
            if (drawPile.Count == 0)
            {
                ReshuffleDiscardPile();
            }

            // 还是没有牌，返回null
            if (drawPile.Count == 0)
            {
                Debug.LogWarning("没有卡牌可抽了！");
                return null;
            }

            // 抽取第一张牌
            CardData card = drawPile[0];
            drawPile.RemoveAt(0);

            OnDrawPileChanged?.Invoke(drawPile.Count);

            return card;
        }

        public void MoveToDiscardPile(CardData card)
        {
            discardPile.Add(card);
            OnDiscardPileChanged?.Invoke(discardPile.Count);
        }

        public void MoveToExhaustPile(CardData card)
        {
            exhaustPile.Add(card);
        }

        private void ShuffleDrawPile()
        {
            // Fisher-Yates洗牌算法
            for (int i = drawPile.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                CardData temp = drawPile[i];
                drawPile[i] = drawPile[randomIndex];
                drawPile[randomIndex] = temp;
            }

            Debug.Log("抽牌堆已洗牌");
        }

        private void ReshuffleDiscardPile()
        {
            if (discardPile.Count == 0)
            {
                Debug.LogWarning("弃牌堆为空，无法重新洗牌");
                return;
            }

            Debug.Log($"抽牌堆为空，将弃牌堆 {discardPile.Count} 张卡牌重新洗入抽牌堆");

            // 将弃牌堆的牌移动到抽牌堆
            drawPile.AddRange(discardPile);
            discardPile.Clear();

            // 洗牌
            ShuffleDrawPile();

            OnDrawPileChanged?.Invoke(drawPile.Count);
            OnDiscardPileChanged?.Invoke(discardPile.Count);
        }

        // 添加卡牌到主卡组（战斗奖励）
        public void AddCardToMasterDeck(int cardID)
        {
            masterDeck.AddCard(cardID);
            Debug.Log($"卡牌 ID:{cardID} 已添加到主卡组");
        }

        // 从主卡组移除卡牌
        public void RemoveCardFromMasterDeck(int cardID)
        {
            masterDeck.RemoveCard(cardID);
        }

        public DeckData GetMasterDeck()
        {
            return masterDeck;
        }
    }
}
