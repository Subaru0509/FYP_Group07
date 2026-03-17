using System.Collections.Generic;
using UnityEngine;

namespace CardBattleSystem
{
    public class HandManager : MonoBehaviour
    {
        public static HandManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private int maxHandSize = 10;
        [SerializeField] private int cardsToDrawPerTurn = 5;

        [Header("Current Hand")]
        private List<Card> cardsInHand = new List<Card>();

        public List<Card> CardsInHand => cardsInHand;
        public int HandSize => cardsInHand.Count;

        public System.Action<Card> OnCardDrawn;          // 抽到卡牌时
        public System.Action<Card> OnCardPlayed;         // 打出卡牌时
        public System.Action<Card> OnCardDiscarded;      // 弃置卡牌时
        public System.Action OnHandCleared;              // 清空手牌时

        private DeckManager deckManager;
        private TurnSystem turnSystem;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            deckManager = DeckManager.Instance;

            // 监听回合开始事件来抽牌
            turnSystem = FindObjectOfType<TurnSystem>();
            if (turnSystem != null)
            {
                turnSystem.OnPlayerTurnStart += DrawCardsAtTurnStart;
                turnSystem.OnPlayerTurnEnd += DiscardHandAtTurnEnd;
            }
        }

        private void OnDestroy()
        {
            if (turnSystem != null)
            {
                turnSystem.OnPlayerTurnStart -= DrawCardsAtTurnStart;
                turnSystem.OnPlayerTurnEnd -= DiscardHandAtTurnEnd;
            }
        }

        private void DrawCardsAtTurnStart()
        {
            DrawCards(cardsToDrawPerTurn);
        }

        private void DiscardHandAtTurnEnd()
        {
            DiscardHand();
        }

        public void DrawCard()
        {
            if (deckManager == null)
            {
                Debug.LogError("DeckManager not found!");
                return;
            }

            // 检查手牌是否已满
            if (cardsInHand.Count >= maxHandSize)
            {
                Debug.LogWarning("手牌已满，无法抽牌");
                return;
            }

            // 从抽牌堆抽取卡牌
            CardData cardData = deckManager.DrawCard();
            if (cardData == null)
            {
                Debug.LogWarning("没有卡牌可抽");
                return;
            }

            // 创建卡牌实例（TODO: 使用对象池优化）
            GameObject cardObj = new GameObject($"Card_{cardData.cardName}");
            Card card = cardObj.AddComponent<Card>();
            card.Initialize(cardData);

            // 添加到手牌
            cardsInHand.Add(card);

            // 触发事件
            OnCardDrawn?.Invoke(card);

            Debug.Log($"抽到卡牌: {cardData.cardName}");
        }

        public void DrawCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                DrawCard();
            }
        }

        public void PlayCard(Card card, Entity target)
        {
            if (!cardsInHand.Contains(card))
            {
                Debug.LogWarning("尝试打出不在手中的卡牌");
                return;
            }

            // 打出卡牌
            card.Play(target);

            // 从手牌移除
            cardsInHand.Remove(card);

            // 移动到弃牌堆
            deckManager.MoveToDiscardPile(card.data);

            // 触发事件
            OnCardPlayed?.Invoke(card);

            // 销毁卡牌GameObject（TODO: 使用对象池优化）
            Destroy(card.gameObject);
        }

        public void DiscardCard(Card card)
        {
            if (!cardsInHand.Contains(card))
            {
                return;
            }

            cardsInHand.Remove(card);
            deckManager.MoveToDiscardPile(card.data);

            OnCardDiscarded?.Invoke(card);

            Destroy(card.gameObject);
        }

        public void DiscardHand()
        {
            Debug.Log($"弃置手牌: {cardsInHand.Count} 张");

            // 弃置所有手牌
            while (cardsInHand.Count > 0)
            {
                DiscardCard(cardsInHand[0]);
            }

            OnHandCleared?.Invoke();
        }

        public void ClearHand()
        {
            // 清空手牌（不移动到弃牌堆，用于战斗开始时）
            foreach (var card in cardsInHand)
            {
                if (card != null)
                    Destroy(card.gameObject);
            }
            cardsInHand.Clear();

            OnHandCleared?.Invoke();
        }
    }
}
