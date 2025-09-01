using _Game.Scripts.CharactersSystem.Enemy;
using _Game.Scripts.CharactersSystem.Player;
using _Game.Scripts.TableSystem;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.LevelSystem
{
    public class LevelInitializer : IService
    {
        private Table _table;
        
        public void Initialize()
        {
            _table = Object.FindObjectOfType<Table>();
            SetCharactersOnPlacements();
        }

        private void SetCharactersOnPlacements()
        {
            _table.EnemyTakeCardZone.Initialize(G.Get<EnemyController>());
            _table.PlayerTakeCardZone.Initialize(G.Get<PlayerController>());
        }
    }
}