using System;
using System.Collections;
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
    public class RoundController : IService, IDisposable
    {
        private RoundsView _roundsView;
        private int _playerWinCount = 0;
        
        public int CurrentRound => _playerWinCount;

        public bool GameOver;
        
        public void Initialize()
        {
            _roundsView = Object.FindObjectOfType<RoundsView>();
            G.Get<PlayerController>().Health.OnDeath += OnPlayerDeath;
            G.Get<EnemyController>().Health.OnDeath += OnEnemyDeath;

            PostInit();
        }

        private void PostInit()
        {
            G.Get<InputRoot>().Enable();   
        }
        
        private void OnPlayerDeath()
        {
            GameOver = true;
            G.Get<InputRoot>().HardDisable();
            G.Get<PlayerController>().PlayerView.Death(SpawnLoseCanvas);
        }

        private void SpawnLoseCanvas()
        {
            var asset = Resources.Load<GameObject>("Prefabs/UI/[Canvas] LoseCanvas");
            Object.Instantiate(asset, Vector3.zero, Quaternion.identity);
        }
        
        private void SpawnWinCanvas()
        {
            var asset = Resources.Load<GameObject>("Prefabs/UI/[Canvas] WinCanvas");
            Object.Instantiate(asset, Vector3.zero, Quaternion.identity);
        }
        
        private void OnEnemyDeath()
        {
            Debug.Log("enemy death");
            G.Get<DeckController>().DestroyAllCards();
            _playerWinCount++;
            G.Get<InputRoot>().Disable();
            _roundsView.EnableCrests(_playerWinCount);
            if (_playerWinCount >= 3)
            {
                G.Get<EnemyController>().EnemyView.Death(SpawnWinCanvas);
                GameOver = true;
            }
            else
            {
                G.Get<RoutineStarter>().StartCoroutine(NextRound());
            }
        }

        private IEnumerator NextRound()
        {
            yield return new WaitForSeconds(2f);
            G.Get<ItemsController>().EnableItems();
            G.Get<RoutineStarter>().StartCoroutine(G.Get<TurnController>().SwitchTurnToPlayer());
            
            G.Get<EnemyController>().Health.Init(6);
            G.Get<PlayerController>().Health.Init(6);
        }

        public void Dispose()
        {
            G.Get<PlayerController>().Health.OnDeath -= OnPlayerDeath;
            G.Get<EnemyController>().Health.OnDeath -= OnEnemyDeath;
        }
    }
}