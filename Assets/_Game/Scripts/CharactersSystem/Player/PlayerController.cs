using _Game.Scripts.CharactersSystem.Enemy;
using _Game.Scripts.Items;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.CharactersSystem.Player
{
    public class PlayerController :CharacterController, IService 
    {
        private Health _health;
        private PlayerView _playerView;
        private HealthView _healthView;
        public override CharacterController Opponent => G.Get<EnemyController>();

        public PlayerView PlayerView => _playerView;
        public Health Health => _health;
        
        public void Initialize()
        {
            _healthView = Object.FindObjectOfType<HealthPanel>().PlayerHealthView;
            _healthView.Init();
            _health = new Health();
            _health.OnHealthInited += _healthView.ActiveHealth;
            _health.OnHealthAdd += _healthView.AddHealth;
            _health.OnDamaged += _healthView.RemoveHealth;
            _health.Init(2);
            
            _playerView = Object.FindObjectOfType<PlayerView>();
        }

        public override void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            _playerView.TakeDamage();
        }

        public override void AddHealth()
        {
            _health.AddOneHealth();
        }

        public override void AddItem(Item item)
        {
            
        }

        public override void LockNextTurn()
        {
            _playerView.EnableLock();
            _canDoTurn = false;
        }

        private int _tryUnlockCount; 
        
        public override void TryUnlockTurn()
        {
            _tryUnlockCount++;

            if (_tryUnlockCount >= 2)
            {
                _tryUnlockCount = 0;
                _canDoTurn = true;
                _playerView.DisableLock();
            }
        }

        public override void EnablePanickAnimation()
        {
              
        }

        public override void DisablePanickAnimation()
        {
            
        }
    }
}