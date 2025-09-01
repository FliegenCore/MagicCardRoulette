using System;
using UnityEngine;

namespace _Game.Scripts.CharactersSystem
{
    public class Health
    {
        public event Action<int> OnDamaged;
        public event Action<int> OnHealthAdd;
        public event Action<int> OnHealthInited;
        public event Action OnDeath;

        private const int MAX_HEALTH = 6;
        
        private int _currentHealth;

        public void Init(int health)
        {
            _currentHealth = health;
            OnHealthInited?.Invoke(_currentHealth);
        }

        public bool HasMaxHealth()
        {
            if(_currentHealth >= MAX_HEALTH)
                return true;
            
            return false;
        }
        
        public void AddOneHealth()
        {
            if (_currentHealth >= MAX_HEALTH)
            {
                return;
            }
            
            _currentHealth++;
            OnHealthAdd?.Invoke(1);
        }
        
        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            OnDamaged?.Invoke(damage);
            if (_currentHealth <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            OnDeath?.Invoke();
        }
    }
}