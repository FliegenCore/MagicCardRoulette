using UnityEngine;

namespace _Game.Scripts.CharactersSystem
{
    public class HealthPanel : MonoBehaviour
    {
        [SerializeField] private HealthView _playerHealthView;
        [SerializeField] private HealthView _enemyHealthView;
        
        public HealthView PlayerHealthView => _playerHealthView;
        public HealthView EnemyHealthView => _enemyHealthView;
    }
}