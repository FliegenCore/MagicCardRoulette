using System;
using System.Collections;
using System.ComponentModel.Design;
using _Game.Scripts.CardSystem;
using _Game.Scripts.CharactersSystem.Enemy;
using _Game.Scripts.CharactersSystem.Player;
using _Game.Scripts.Items;
using _Game.Scripts.PlayerInput;
using Game.ServiceLocator;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Game.Scripts.RoundsSystem
{
    public class TurnController : IService, IDisposable
    {
        private TurnView _playerTurnView;
        private TurnView _enemyTurnView;
        private int _itemCount = 1;

        private bool _isPlayerTurn;

        private Coroutine _switchTurnWalker;
        
        public void Initialize()
        {
            _playerTurnView = GameObject.Find("PlayerLamp").GetComponent<TurnView>();
            _enemyTurnView = GameObject.Find("EnemyLamp").GetComponent<TurnView>();
            _isPlayerTurn = false;
            G.Get<DeckController>().OnDeckDeal += OnDeckDeal;
            G.Get<RoutineStarter>().StartCoroutine(SwitchTurnWalker());
        }

        public void EndTurn()
        {
            if (G.Get<RoundController>().GameOver)
            {
                return;
            }
            
            G.Get<InputRoot>().Disable();
            G.Get<EnemyController>().StopAction();

            G.Get<RoutineStarter>().StartCoroutine(NextTurn());
        }

        private void OnDeckDeal()
        {
            if (!_isPlayerTurn)
            {
                G.Get<EnemyController>().DoTurn();
            }
            else
            {
                G.Get<InputRoot>().Enable();
            }
        }

        public IEnumerator SwitchTurnToPlayer()
        {
            StopSwitchTurn();
            yield return new WaitForSeconds(2f);
            _playerTurnView.EnableGreen();
            _enemyTurnView.EnableRed();
            _isPlayerTurn = true;
            G.Get<InputRoot>().Enable();
        }
        
        private IEnumerator NextTurn()
        {
            StopSwitchTurn();
            yield return new WaitForSeconds(2f);

            if (G.Get<RoundController>().CurrentRound == 0)
            {
                G.Get<DeckController>().CreateFirstDeck();
                
                yield break;
            }
            
            G.Get<RoutineStarter>().StartCoroutine(G.Get<ItemsController>().GiveItems(_itemCount));
            G.Get<DeckController>().CreateDeck();
            
            if(_itemCount < 3)
                _itemCount++;
        }

        public void SwitchWalker()
        {
            _switchTurnWalker = G.Get<RoutineStarter>().StartCoroutine(SwitchTurnWalker());
        }

        private void StopSwitchTurn()
        {
            if (_switchTurnWalker != null)
            {
                G.Get<RoutineStarter>().StopCoroutine(_switchTurnWalker);
                _switchTurnWalker = null;
            }
            G.Get<EnemyController>().StopWaitDoTurn();
        }
        
        private IEnumerator SwitchTurnWalker()
        {
            yield return new WaitForSeconds(1f);
            Debug.Log(_isPlayerTurn);
            Debug.Log("Turn switched");
            if (_isPlayerTurn)
            {
                if(!G.Get<EnemyController>().CanDoTurn)
                    G.Get<EnemyController>().TryUnlockTurn();
                
                if (G.Get<EnemyController>().CanDoTurn)
                {
                    _enemyTurnView.EnableGreen();
                    _playerTurnView.EnableRed();
                    G.Get<InputRoot>().Disable();
                    _isPlayerTurn = false;
                    G.Get<EnemyController>().DoTurn();
                    Debug.Log("IS ai turn");
                    Debug.Log(_isPlayerTurn);
                }
            }
            else
            {
                if(!G.Get<PlayerController>().CanDoTurn)
                    G.Get<PlayerController>().TryUnlockTurn();

                if (G.Get<PlayerController>().CanDoTurn)
                {
                    _enemyTurnView.EnableRed();
                    _playerTurnView.EnableGreen();
                    _isPlayerTurn = true;
                    G.Get<InputRoot>().Enable();
                    Debug.Log("IS player turn");
                    Debug.Log(_isPlayerTurn);
                }
                else
                {
                    G.Get<EnemyController>().DoTurn();
                }
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}