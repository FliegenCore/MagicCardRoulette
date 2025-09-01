using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.CardSystem.EffectVariants;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.RoundsSystem;
using _Game.Scripts.TableSystem;
using DG.Tweening;
using Game.ServiceLocator;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Game.Scripts.CardSystem
{
    public class DeckController : IService
    {
        public event Action OnDeckDeal;
        public event Action OnTurnEnd;
        
        private Sprite _attackSprite;
        private Sprite _emptySprite;
        
        private List<Card> _cards = new List<Card>();

        private Card _prefab;

        private bool _isFirstTurn = true;
        private TutorHand _tutorHand;
        
        //confings
        private Dictionary<int, int[]> _secondRoundCards = new Dictionary<int, int[]>
        { 
            { 0, new int[] { 2, 2 } },
            { 1, new int[] { 1, 3 } },
            { 2, new int[] { 2, 3 } },
            { 3, new int[] { 3, 2 } },
            { 4, new int[] { 3, 1 } },
            { 5, new int[] { 4, 1 } },
        };
        
        private Dictionary<int, int[]> _thirdRoundCards = new Dictionary<int, int[]>
        { 
            { 0, new int[] { 4, 4 } },
            { 1, new int[] { 4, 3 } },
            { 2, new int[] { 3, 4 } },
            { 3, new int[] { 6, 2 } },
            { 4, new int[] { 7, 1 } },
            { 5, new int[] { 5, 3 } },
            { 6, new int[] { 5, 2 } },
        };
        
        //---------
        
        public void Initialize()
        {
            _tutorHand = Object.FindObjectOfType<TutorHand>();
            _prefab = Resources.Load<Card>("Prefabs/Card");
            _attackSprite = Resources.Load<Sprite>("Sprites/AttackCard");
            _emptySprite = Resources.Load<Sprite>("Sprites/EmptyCard");

            CreateFirstDeck();
        }
        
        public Card GetTopCard()
        {
            return _cards.Last();
        }

        public bool IsEmpty()
        {
            if (_cards.Count == 0)
            {
                return true;
            }
            
            return false;
        }
        
        public void DestroyAllCards()
        {
            foreach (var card in _cards)
            {
                G.Get<RoutineStarter>().StartCoroutine(card.JustDissolve());
            }
            
            _cards.Clear();
        }

        public void SwitchCardEffect(Card card)
        {
            if (card.Effect is AttackEffect)
            {
                card.Initialize(new EmptyEffect(), _emptySprite);
            }
            else
            {
                card.Initialize(new AttackEffect(), _attackSprite);
            }

            //card.StartSwitchEffect();
        }

        public bool OnlyAttackCards()
        {
            if(_cards.Count == 0) return false;
            
            foreach (var card in _cards)
            {
                if (card.Effect is EmptyEffect)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        public bool OnlyEmptyCards()
        {
            if(_cards.Count == 0) return false;
            
            foreach (var card in _cards)
            {
                if (card.Effect is AttackEffect)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        public void CreateFirstDeck()
        {
            int attackCards = 2;
            int emptyCards = 1;

            SpawnCard(attackCards, emptyCards);
        }
        
        public void CreateDeck()
        {
            RoundController roundController = G.Get<RoundController>();

            if (roundController.CurrentRound == 1)
            {
                int randIndex = Random.Range(0, _secondRoundCards.Keys.Count) ;
                int[] count = _secondRoundCards[randIndex];
                SpawnCard(count[0], count[1]);
            }
            else
            {
                int randIndex = Random.Range(0, _thirdRoundCards.Keys.Count) ;
                int[] count = _thirdRoundCards[randIndex];
                SpawnCard(count[0], count[1]);
            }
        }
        
        private void SpawnCard(int attackCount, int emptyCardCount)
        {
            for (int i = 0; i < attackCount; i++)
            {
                Card card = Object.Instantiate(_prefab);
                _cards.Add(card);
                card.Initialize(new AttackEffect(), _attackSprite);
            }

            for (int i = 0; i < emptyCardCount; i++)
            {
                Card card = Object.Instantiate(_prefab);
                _cards.Add(card);
                card.Initialize(new EmptyEffect(), _emptySprite);
            }

            foreach (var card in _cards)
            {
                card.Hide();
                card.transform.DORotate(new Vector3(0, 180, 0), 0f);
            }
            
            ArrangeCardsWithSpacing(_cards);

            InitCards();
        }

        private void InitCards()
        {
            foreach (var card in _cards)
            {
                G.Get<RoutineStarter>().StartCoroutine(card.SpawnCardAnimation());
                card.OnCardDissolved += OnCardUsed;
            }

            G.Get<RoutineStarter>().StartCoroutine(DealCards());
        }

        private IEnumerator DealCards()
        {
            yield return new WaitForSeconds(2f);
    
            foreach (var card in _cards)
            {
                card.ShowBack();
            }
    
            yield return new WaitForSeconds(0.5f);

            ShuffleCards(_cards);

            float x = 0;
            float y = 0;
            float z = 0f; 
    
            foreach (var card in _cards)
            {
                float angle = Random.Range(-6f, 6f);
                card.PlayDealSound();
                card.transform.DOMove(new Vector3(x, y, z), 0.25f);
                card.transform.DORotate(new Vector3(0, 180, angle), 0.25f);
                x += 0.025f;
                y -= 0.025f;
                z -= 0.001f; 
            }

            G.Get<RoutineStarter>().StartCoroutine(OnCardsDeal());
        }

        private void ShuffleCards(List<Card> cards)
        {
            System.Random rng = new System.Random();
            int n = cards.Count;
    
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                // Меняем карты местами
                Card temp = cards[k];
                cards[k] = cards[n];
                cards[n] = temp;
            }
        }

        private IEnumerator OnCardsDeal()
        {
            yield return new WaitForSeconds(0.4f);
            if (_isFirstTurn)
            {
                _tutorHand.transform.position = Vector3.zero;
                Table talbe = Object.FindObjectOfType<Table>();
                _tutorHand.transform.DOMove(talbe.EnemyTakeCardZone.transform.position, 1f)
                    .SetLoops(-1, LoopType.Yoyo);
            }
            
            OnDeckDeal?.Invoke();
            _cards.Last().EnableCollider();
        }
        
        private void OnCardUsed(Card card)
        {
            if (_isFirstTurn)
            {
                DOTween.Kill(_tutorHand.transform);
                Object.Destroy(_tutorHand.gameObject);
                _isFirstTurn = false;
            }
            Debug.Log("remove card");
            _cards.Remove(card);
            card.OnCardDissolved -= OnCardUsed;
            Object.Destroy(card.gameObject);
            if(_cards.Count > 0)
                _cards.Last().EnableCollider();
            
            if (_cards.Count == 0)
            {
                G.Get<TurnController>().EndTurn();
            }
        }
        
        private void ArrangeCardsWithSpacing(List<Card> cards)
        {
            if (cards == null || cards.Count == 0) return;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
    
            float cardWidth = 100f;
            float cardHeight = 150f;
    
            float spacing = 45f; 
            float totalWidth = (cards.Count * cardWidth) + ((cards.Count - 1) * spacing);
    
            float startX = (screenWidth - totalWidth) / 2f;
            float centerY = screenHeight / 2f;
    
            for (int i = 0; i < cards.Count; i++)
            {
                float xPos = startX + (i * (cardWidth + spacing)) + (cardWidth / 2f);
        
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(
                    new Vector3(xPos, centerY, Camera.main.nearClipPlane + 1f));
                worldPosition.z = 0f;
                cards[i].transform.position = worldPosition;
            }
        }
    }
}