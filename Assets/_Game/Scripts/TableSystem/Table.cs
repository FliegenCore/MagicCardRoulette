using _Game.Scripts.Items;
using UnityEngine;

namespace _Game.Scripts.TableSystem
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private ItemZone[] _enemyItemsZone;
        [SerializeField] private ItemZone[] _playerItemsZone;
        [SerializeField] private TakeCardZone _playerTakeCardZone;
        [SerializeField] private TakeCardZone _enemyTakeCardZone;
        
        public TakeCardZone PlayerTakeCardZone => _playerTakeCardZone;
        public TakeCardZone EnemyTakeCardZone => _enemyTakeCardZone;
        public ItemZone[] PlayerItemsZone => _playerItemsZone;
        public ItemZone[] EnemyItemsZone => _enemyItemsZone;
    }
}